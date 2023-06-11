using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {

        private readonly IServiceManager _serviceManager;

        public NewsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("highlights")]
        public async Task<IActionResult> GetHighlights(CancellationToken cancellationToken = default)
        {
            var highlights = await _serviceManager.NewsService.GetHighlights(5, cancellationToken);
            return Ok(highlights);
        }
    }
}
