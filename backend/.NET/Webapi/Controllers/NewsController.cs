using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System;
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
        [HttpPost]
        public async Task<ActionResult<News>>CreateNews(News news)
        {
            if (news == null)
            {
                return BadRequest();
            }

            var createdNews = await _serviceManager.NewsService.CreateAsync(news);
            return CreatedAtAction(nameof(GetNewsById), new { id = createdNews.Id }, createdNews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(Guid id, CancellationToken cancellationToken = default)
        {
            var news = await _serviceManager.NewsService.GetByIdAsync(id, cancellationToken);

            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }
    }
}
