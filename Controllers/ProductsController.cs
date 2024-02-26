using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContosoCrafts.WebSite.Controllers
{
    [Route("[controller]")] // url route, in this case the name of our controller: /products
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Properties
        public JsonFileProductService ProductService { get; }

        // Constructor, injecting service dependency
        public ProductsController(JsonFileProductService productService) 
        {
            this.ProductService = productService; // Assign ProductService property the service we are injecting
        }

        // Methods, endpoints
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return ProductService.GetProducts();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Product product)
        {
            ProductService.AddProduct(product);
            return Ok("New product added");
        }

        [HttpPatch]
        [Route("Rate")] // /products/rate
        public ActionResult Patch(
            [FromQuery] string ProductId,
            [FromBody] int Rating)
        {
            ProductService.AddRating(ProductId, Rating);
            return Ok("Product Rated");
        }

        //[HttpDelete]
        //[Route("Remove")] // /products/remove
        //public ActionResult Delete([FromQuery] string ProductId)
        //{
        //    ProductService.RemoveProduct(ProductId);
        //    return Ok($"Product with id {ProductId} deleted");
        //}
    }
}
