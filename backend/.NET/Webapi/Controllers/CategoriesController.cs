using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepositoryManger _repositoryManger;

        public CategoriesController(IRepositoryManger repositoryManger)
        {
            _repositoryManger=repositoryManger;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var categories = await _repositoryManger.CategoryRepository.GetAllAsync();
            return Ok(categories);
        }

    }
}
