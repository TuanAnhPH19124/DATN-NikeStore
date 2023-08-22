using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Threading.Tasks;
using System.Threading;
using System;
using EntitiesDto;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Persistence;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Mapster;
using EntitiesDto.User;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AppUserController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAllAppUser()
        {
            try
            {
                var appUser = await _serviceManager.AppUserService.GetAllAppUserAsync();
                if (appUser == null || !appUser.Any())
                {
                    return NotFound();
                }
                return Ok(appUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }


        [HttpGet("Get/{Id}")]
        public async Task<ActionResult<AppUser>> GetByIdAppUser(string Id)
        {
            var appUser = await _serviceManager.AppUserService.GetByIdAppUserAsync(Id);

            if (appUser == null)
            {
                return NotFound();
            }
            return appUser;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppUser(string id, AppUserDto appUserDto)
        {
            if (id != appUserDto.Id)
            {
                return BadRequest("The provided id does not match the id in the user data.");
            }
            try
            {
                var user = appUserDto.Adapt<AppUser>();
                await _serviceManager.AppUserService.UpdateByIdAppUser(id, user);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
            return NoContent();
        }

        [HttpGet("phone=/{number}")]
        public async Task<IActionResult> GetUserByPhoneNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return BadRequest(new { error = "Không được để trống số điện thoại" });
            }
            if (number.Length < 10 || number.Length > 10)
                return BadRequest(new { error = "Số điện thoại phải đủ 10 số" });

            var result = await _serviceManager.AppUserService.GetUserByPhoneNumber(number);
            return Ok(result);
        }
        
        [HttpPut("{id}/UpdateUserByAdmin")]
        public async Task<IActionResult> UpdateAppUserByAdmin(string id, AppUserByAdmin appUserByAdmin)
        {
            if (id != appUserByAdmin.Id)
            {
                return BadRequest("The provided id does not match the id in the user data.");
            }
            try
            {
                var user = appUserByAdmin.Adapt<AppUser>();
                await _serviceManager.AppUserService.UpdateByIdAppUserByAdmin(id, user);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
            return NoContent();
        }
    }
}
