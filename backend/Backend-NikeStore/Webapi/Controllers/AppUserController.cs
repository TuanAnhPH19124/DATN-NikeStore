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
      

        [HttpGet("{Id}")]
        public async Task<ActionResult<AppUser>> GetAppUsers(Guid Id)
        {
            var appUser = await _serviceManager.AppUserService.GetByIdAppUser(Id);

            if (appUser == null)
            {
                return NotFound();
            }
            return appUser;
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AppUserForCreateDto appUserForCreationDto)
        {
            await _serviceManager.AppUserService.CreateAsync(appUserForCreationDto);
            return Ok();
        }

        private object GetAccountById()
        {
            throw new NotImplementedException();
        }
       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppUser(Guid id, AppUser appUser)
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
