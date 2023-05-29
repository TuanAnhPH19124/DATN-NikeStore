using EntitiesDto;
using EntitiesDto.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthenticationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


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

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback(CancellationToken cancellationToken)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Google");
            if (authenticateResult?.Principal is { Identity: { IsAuthenticated: true } })
            {
                var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
                var exsisted = await _serviceManager.AppUserService.GetauthenticationByGoogle(email);
                if (exsisted == null)
                {
                    var user = new AppUserForCreateDto
                    {
                        Email = email,
                        PhoneNumber = authenticateResult.Principal.FindFirstValue(ClaimTypes.OtherPhone),
                        FullName  = authenticateResult.Principal.FindFirstValue(ClaimTypes.Name)
                    };

                    await _serviceManager.AppUserService.CreateAsync(user, cancellationToken);
                    return Ok(user);
                }

                return Ok(exsisted); // Trả về thông tin người dùng cho frontend
            }

            return Unauthorized();
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] AppUserForLogin appUser, CancellationToken cancellationToken)
        {
            var user = await _serviceManager.AppUserService.GetauthenticationByLogin(appUser, cancellationToken);
            if (user == null)
            {
                return BadRequest("Tài hoặc mật khẩu không đúng");
            }
            return Ok(user);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] AppUserForCreateDto appUser, CancellationToken cancellationToken)
        {
            var user = await _serviceManager.AppUserService.CreateAsync(appUser, cancellationToken);
            return Ok(user);
        }
    }
}
