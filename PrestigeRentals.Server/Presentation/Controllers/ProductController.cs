using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Interfaces;
using PrestigeRentals.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace PrestigeRentals.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]/GetAllProducts")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

    }
}
