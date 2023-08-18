using Domain.Entities;
using EntitiesDto.Product;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Service.Abstractions;
using System.Linq;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Webapi.Controllers
{
   

    public class ProductsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IElasticClient _elasticClient;

        public ProductsController(IServiceManager serviceManager, IElasticClient elasticClient)
        {
            _serviceManager=serviceManager;
            _elasticClient=elasticClient;
        }


        [HttpGet(Name = "SearchProduct")]
        public async Task<IActionResult> GetSearchResult(string keyword)
        {
            var result = await _elasticClient.SearchAsync<Product>(
                    s => s.Query(
                            q => q.QueryString(
                                    d => d.Query('*'+keyword+'*')
                                )
                        ).Size(100)
                );

            return Ok(result.Documents.ToList());
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Post([FromForm]ProductForRequestPostDto product)
        {
            #region example
            //var products = await _serviceManager.ProductService.SelectByCategoryOnCacheAsync(categoryId);
            //var indexManyResponse = _elasticClient.IndexMany(products);
            //if (indexManyResponse.Errors)
            //{
            //    var error = indexManyResponse.ItemsWithErrors.Select(item => item.Error);
            //    return BadRequest(error);

            //}
            #endregion

            #region Add
            if (product.Images.Count() > 0)
            {
                return await Task.FromResult(Ok(product));
            }
            return BadRequest(new { Warning = "Cần ảnh cho sản phẩm này" });
            #endregion
        }

    }
}
