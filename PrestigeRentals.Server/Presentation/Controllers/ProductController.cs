using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Interfaces;
using PrestigeRentals.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace PrestigeRentals.Presentation.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProducts")]
        [SwaggerOperation(Summary = "Gets all products", Description = "Fetches a list of all products from the system")]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return Ok(_productService.GetAllProducts());
        }
    }
}
