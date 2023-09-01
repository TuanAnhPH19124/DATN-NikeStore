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
            _dbContext=dbContext;
            _serviceManager=serviceManager;
        }

        [HttpGet]
        public string[] Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AddressController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AddressController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]AddressAPI address)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _serviceManager.AddressService.AddNew(address);
                    transaction.Commit();
                    return Ok();
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
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AddressController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
