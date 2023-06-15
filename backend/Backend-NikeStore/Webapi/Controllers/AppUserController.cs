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
        [HttpGet]
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


        [HttpGet("{Id}")]
        public async Task<ActionResult<AppUser>> GetByIdAppUser(string Id)
        {
            var appUser = await _serviceManager.AppUserService.GetByIdAppUserAsync(Id);

            if (appUser == null)
            {
                return NotFound();
            }
            return appUser;
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateAppUser(AppUser appUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var createdAppUser = await _serviceManager.AppUserService.CreateAsync(appUser);
                return CreatedAtAction(nameof(GetByIdAppUser), new { id = createdAppUser.Id }, createdAppUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppUser(string id, AppUser appUser)
        {
            if (id != appUser.Id)
            {
                return BadRequest("The provided id does not match the id in the user data.");
            }
            try
            {
                await _serviceManager.AppUserService.UpdateByIdAppUser(id, appUser);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ DbUpdateConcurrencyException tại đây
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
            return NoContent();
        }
    }
}
