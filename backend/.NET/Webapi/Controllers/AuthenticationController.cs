using Domain.Entities;
using EntitiesDto;
using EntitiesDto.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nest;
using Persistence;
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
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<CustomerHub> _contextHub;
        private readonly IServiceManager _serviceManager;


     
        public AuthenticationController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration, IHubContext<CustomerHub> contextHub, IServiceManager serviceManager, AppDbContext context)
        {
          _signInManager = signInManager;
          _userManager = userManager;
          _configuration = configuration;
          _contextHub = contextHub;
          _serviceManager = serviceManager;
          _context = context;
        }

        [HttpPost("CreateEmployeeAccount")]
        public async Task<IActionResult> CreateEmployeeAccount([FromBody]string phoneNumber,string email,string fullName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    PhoneNumber = phoneNumber,
                    Error = "Không được bỏ trống số điện thoại!",
                    Email = email,
                    ErrorEmail = "Không được bỏ trống email",
                    FullName = email,
                    ErrorFullName = "Không được bỏ trống Họ tên"
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
                PhoneNumber = phoneNumber,
                Email = email,
                FullName = fullName
            };
            var passWord = "!123@Abc";
            var newUserResponse = await _userManager.CreateAsync(newUser, passWord);
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Employee");
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
        [HttpPost("userSignIn")]
        public async Task<IActionResult> UserSignIn([FromBody] AppUserForLogin appUser)
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
            if (!string.IsNullOrEmpty(appUser.Account))
            {
                var userCheckByNameAndEmail = await _userManager.FindByNameAsync(appUser.Account);
                if (userCheckByNameAndEmail == null)
                {
                    userCheckByNameAndEmail = await _userManager.FindByEmailAsync(appUser.Account);
                }
                
                if (userCheckByNameAndEmail == null)
                {
                    return NotFound(new
                    {
                        // account = appUser.Account,
                        // error = "Tài khoàn này không tồn tại!"
                    });
                }
                user_exits = userCheckByNameAndEmail;
            }

            if (user_exits.Status == 0)
                return BadRequest(new {error = "Tài khoản này không thể được truy cập!"});
            

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
            if (!string.IsNullOrEmpty(appUser.Account))
            {
                var userCheckByNameAndEmail = await _userManager.FindByNameAsync(appUser.Account);
                if (userCheckByNameAndEmail == null)
                {
                    userCheckByNameAndEmail = await _userManager.FindByEmailAsync(appUser.Account);
                }
                
                if (userCheckByNameAndEmail == null)
                {
                    return NotFound(new
                    {
                        // account = appUser.Account,
                        // error = "Tài khoàn này không tồn tại!"
                    });
                }
                user_exits = userCheckByNameAndEmail;
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
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordRequest)
        {
            var user = await _userManager.FindByNameAsync(changePasswordRequest.UserName);

            if (user == null)
            {
                return NotFound(new
                {
                    error = "Người dùng không tồn tại!"
                });
            }

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, changePasswordRequest.CurrentPassword);
            if (!passwordCorrect)
            {
                return Unauthorized(new
                {
                    error = "Mật khẩu hiện tại không chính xác!"
                });
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(new
                {
                    error = "Không thể thay đổi mật khẩu. Vui lòng thử lại sau."
                });
            }

            return Ok(new
            {
                message = "Mật khẩu đã được thay đổi thành công."
            });
        }


        [AllowAnonymous]

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _serviceManager.AppUserService.ForgotPassword(forgotPasswordDto.Email);

                if (!result.Result)
                {
                    // Xử lý trường hợp thất bại
                    return BadRequest(new { error = result.Error });
                }

                // Xử lý trường hợp thành công
                return Ok("Password reset email sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }

}

