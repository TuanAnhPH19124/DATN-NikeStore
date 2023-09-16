using Domain.DTOs;
using EntitiesDto;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service;
using Service.Abstractions;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IServiceManager _serviceManager;

        public AddressController(AppDbContext dbContext, IServiceManager serviceManager)
        {
            _dbContext = dbContext;
            _serviceManager = serviceManager;
        }

        // GET api/<AddressController>/5
        [HttpGet("{userId}")]
        public async Task<ActionResult> Get(string userId)
        {       
            var addresses = await _serviceManager.AddressService.GetByUserId(userId);
            return Ok(addresses);
        }

        // POST api/<AddressController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddressAPI address)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    
                   var add =  await _serviceManager.AddressService.AddNew(address);
                    transaction.Commit();
                    return Ok(add);
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        // PUT api/<AddressController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put (string id, [FromBody] AddressUpdateAPI address)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != address.Id)
                return BadRequest(new { error = "Id địa chỉ không hợp lệ" });

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _serviceManager.AddressService.Update(address);
                    transaction.Commit();
                    return NoContent();
                }
                catch (System.Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new { error = ex.Message });
                    throw;
                }
            }
        }

        // DELETE api/<AddressController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
