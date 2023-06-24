using Domain.Entities;
using Domain.Models;
using EntitiesDto;
using EntitiesDto.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Persistence.Ultilities;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Records.NotUsed;

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

        public AuthenticationController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _signInManager=signInManager;
            _userManager=userManager;
            _configuration=configuration;
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
                        token = JwtService.GenerateJwtToken(exsisted, roles,_configuration);
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
            if (ModelState.IsValid)
            {
                var user_exits = await _userManager.FindByEmailAsync(appUser.Email);

                if (user_exits != null)
                {
                    var is_correct = await _userManager.CheckPasswordAsync(user_exits, appUser.Password);

                    if (is_correct)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user_exits, appUser.Password, false, false);
                        if (result.Succeeded)
                        {
                            var roles = await _userManager.GetRolesAsync(user_exits);

                            var token = JwtService.GenerateJwtToken(user_exits, roles, _configuration);
                            return Ok(new
                            {
                                Message = "dang nhap thang cong",
                                Token = token
                            });
                        }

                        return BadRequest("Dang nhap that bai");
                    }

                    return BadRequest("Mat khau chua chinh xac");
                }

                return NotFound("Email khong ton tai");
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] AppUserForCreateDto appUser)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(appUser.Email);
                if (userExists == null)
                {
                    var newUser = new AppUser()
                    {
                        Email = appUser.Email,
                        UserName = appUser.UserName
                    };
                    var newUserResponse = await _userManager.CreateAsync(newUser, appUser.Password);
                    if (newUserResponse.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, "User");
                        var roles = await _userManager.GetRolesAsync(newUser);
                        var token = JwtService.GenerateJwtToken(newUser, roles, _configuration);
                        return Ok(new
                        {
                            Message = "dang ki thanh cong",
                            Token = token
                        });
                    }
                    return BadRequest(newUserResponse.Errors);
                }
                return BadRequest("Tai khoan da ton tai");
            }
            return BadRequest("Khong duoc bo trong");
        }

        [Route("confrimemail")]
        [HttpGet]
        public async Task<IActionResult> ConfrimEmail(string userid, string code)
        {

            return await Task.FromResult(Ok());
        }


    }
}
