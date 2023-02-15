using AspNetCoreWebApi.Data;
using AspNetCoreWebApi.Models;
using AspNetCoreWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductCrudService _productCrudService;

        public ProductController(ProductCrudService productCrudService)
        {
            _productCrudService = productCrudService;
        }

        [HttpGet]
        // Use ActionResult return type.
        public ActionResult GetProducts()
        {
            var products = _productCrudService.GetProducts();

            return Ok(products);
        }

        [HttpGet("{productId}")]
        // productId name declaration must be the same.
        public ActionResult GetProduct(Guid productId)
        {
            var product = _productCrudService.GetProduct(productId);

            return Ok(product);
        }

        [HttpPost]
        // FromBody will capture the HTTP body JSON input from HTTP request.
        public ActionResult Create([FromBody] NewProduct newProduct)
        {
            // This will check whether the model (newProduct) data annotation's validations contains error or not.
            if (!ModelState.IsValid)
            {
                // Will return HTTP status code 400, which indicates user input errors due validation etc.
                return ValidationProblem(ModelState);
            }

            var newId = _productCrudService.AddNewProduct(newProduct);

            return Ok($"New Product ID: {newId}");
        }

        [HttpPut]
        public ActionResult Update()
        {
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return Ok();
        }
    }
}
