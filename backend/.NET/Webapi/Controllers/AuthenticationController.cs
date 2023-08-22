using Domain.Entities;
using EntitiesDto;
using EntitiesDto.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Nest;
using Persistence.Ultilities;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Webapi.Hubs;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<CustomerHub> _contextHub;

        private readonly IServiceManager _serviceManager;

        public AuthenticationController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration, IHubContext<CustomerHub> contextHub, IServiceManager serviceManager)
    {
          _signInManager = signInManager;
          _userManager = userManager;
          _configuration = configuration;
          _contextHub = contextHub;
          _serviceManager = serviceManager;
    }

        [HttpPost("CreateEmployeeAccount")]
        public async Task<IActionResult> CreateEmployeeAccount([FromBody]string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    PhoneNumber = phoneNumber,
                    Error = "Không được bỏ trống số điện thoại!"
                });
            }

            string userName = phoneNumber.Substring(phoneNumber.Length - 6);
            var userNameExisted = await _userManager.FindByNameAsync(userName);
            if (userNameExisted != null)
            {
                return Conflict(new
                {
                    UserName = userName,
                    Error = "UserName này đã tồn tại!"
                });
            }

            var newUser = new AppUser()
            {
                UserName = userName,
                PhoneNumber = phoneNumber
            };
            var passWord = "!123@Abc";
            var newUserResponse = await _userManager.CreateAsync(newUser, passWord);
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Admin");
                return Ok(new
                {
                    User = newUser,
                });
           
            }
            else
            {
                foreach (var error in newUserResponse.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpGet("google")]
        public IActionResult GoogleLogin()

        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback)),
                Items =
                {
                    { "LoginProvider", "Google" }
                }
            };
            return Challenge(authenticationProperties, "Google");
        }

        [AllowAnonymous]
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Google");
            if (authenticateResult?.Principal is { Identity: { IsAuthenticated: true } })
            {
                var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
                var exsisted = await _userManager.FindByEmailAsync(email);
                string token = null;
                IEnumerable<string> roles = null;
                if (exsisted == null)
                {
                    var user = new AppUser
                    {
                        Email = email,
                        UserName = authenticateResult.Principal.FindFirstValue(ClaimTypes.Surname).Trim(),
                        EmailConfirmed = true

                    };

                    var result = await _userManager.CreateAsync(user, "A012292@a");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        roles = await _userManager.GetRolesAsync(user);
                        token = JwtService.GenerateJwtToken(exsisted, roles, _configuration);
                        return Ok(new
                        {
                            Message = "Thanh cong",
                            Token = token,
                        });
                    }
                    return BadRequest("Error");

                }
                roles = await _userManager.GetRolesAsync(exsisted);
                token = JwtService.GenerateJwtToken(exsisted, roles, _configuration);
                return Ok(new
                {
                    Message = "Thanh cong",
                    Token = token,
                });
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] AppUserForLogin appUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    data = ModelState,
                    error = "Không được để trống thông tin đăng nhập!"
                });
            }

            var user_exits = new AppUser();
            if (!string.IsNullOrEmpty(appUser.UserName))
            {
                var userCheck = await _userManager.FindByNameAsync(appUser.UserName);
                if (userCheck == null)
                {
                    return NotFound(new
                    {
                        email = appUser.UserName,
                        error = "UserName này không tôn tại!"
                    });
                }
                user_exits = userCheck;
            }
            else
            {
                var userCheck = await _userManager.FindByEmailAsync(appUser.Email);
                if (userCheck == null)
                {
                    return NotFound(new
                    {
                        email = appUser.Email,
                        error = "Email này không tôn tại!"
                    });
                }
                user_exits = userCheck;
            }
           

            var passwordCorrect = await _userManager.CheckPasswordAsync(user_exits, appUser.Password);
            if (!passwordCorrect)
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user_exits);
            var token = JwtService.GenerateJwtToken(user_exits, roles, _configuration);
            return Ok(new
            {
                User = user_exits.UserName,
                Token = token
            });
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] AppUserForCreateDto appUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    data = ModelState,
                    error = "Không được bỏ trống thông tin đăng ký"
                });
            }

            var emailExisted = await _userManager.FindByEmailAsync(appUser.Email);
            if (emailExisted != null)
            {
                return Conflict(new
                {
                    Email = appUser.Email,
                    Error = "Email này đã tồn tại vui lòng đăng nhập hoặc lấy lại mật khẩu!"
                });
            }

            #region tạo username tự động
            string[] nameParts = appUser.FullName.Split(' ');
            string lastName = nameParts[nameParts.Length - 1];
            string lastNameWithoutDiacritics = new string(lastName.Normalize(NormalizationForm.FormD)
            .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            .ToArray()).ToLower();
            string firstNameInitials = "";
            foreach (var namePart in nameParts)
            {
                if (namePart.Length > 0)
                {
                    firstNameInitials += namePart[0];
                }
            }
            var sixDigit = appUser.PhoneNumber.Substring(appUser.PhoneNumber.Length - 3);
            var userName = lastNameWithoutDiacritics + firstNameInitials.ToLower() + sixDigit;
            #endregion

            var userNameExisted = await _userManager.FindByNameAsync(userName);
            if (userNameExisted != null)
            {
                return Conflict(new
                {
                    UserName = userName,
                    Error = "UserName này đã tồn tại vui lòng chọn một tên khác!"
                });
            }

            var newUser = new AppUser()
            {
                Email = appUser.Email,
                PhoneNumber = appUser.PhoneNumber,
                FullName = appUser.FullName,
                UserName = userName
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, appUser.Password);
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "User");
                var roles = await _userManager.GetRolesAsync(newUser);
                var token = JwtService.GenerateJwtToken(newUser, roles, _configuration);
                return Ok(new
                {
                    User = newUser.UserName,
                    Token = token
                });
            }
            else
            {
                foreach (var error in newUserResponse.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }
           
        }

        [Route("confrimemail")]
        [HttpGet]
        public async Task<IActionResult> ConfrimEmail(string userid, string code)
        {

            return await Task.FromResult(Ok());
        }

        [HttpGet("getUserData")]
        public IActionResult GetUserData()
        {
            if (Request.Cookies.TryGetValue("username", out var username))
            {
                return Ok($"Welcome back, {username}");
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("username", new CookieOptions { Path = "/", Expires = DateTimeOffset.Now.AddDays(-1) });
            return Ok("Dang xuat thanh cong");
        }

        [AllowAnonymous]
       

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var currentUser = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            if (currentUser == null)
            {
                return NotFound("Không tìm thấy người dùng đang đăng nhập.");
            }

            try
            {
                var result = await _userManager.ChangePasswordAsync(currentUser, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "Đổi mật khẩu thành công." });
                }

                return BadRequest(new { Message = "Không thể đổi mật khẩu." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _serviceManager.AppUserService.ForgotPassword(forgotPasswordDto.Email);

                return Ok("Password reset email sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
