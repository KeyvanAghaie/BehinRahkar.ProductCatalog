using BAL.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.DTO;
using System;
using System.Threading.Tasks;

namespace Behin.Product.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProductService _productService;

        
        public ProductsController(ILogger logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        #region Get
        [HttpGet(nameof(GetProductById))]
        public async Task<IActionResult> GetProductById(long productId)
        {
            try
            {
                var response = await _productService.GetProductByIdAsync(productId);
                if (response == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred when fetching the product cataloge with Id {productId}.", productId);
                return StatusCode(500);
            }
        }


        [HttpGet(nameof(GetProductByCode))]
        public async Task<IActionResult> GetProductByCode(string code)
        {
            try
            {
                var response = await _productService.GetproductByCodeAsync(code);
                if (response == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred when fetching the product cataloge with Code : ", code);
                return StatusCode(500);
            }
        }


        [HttpGet(nameof(GetAllProducts))]
        public async Task<IActionResult> GetAllProducts(string filter, int? paginationIndex, int? paginationSize)
        {
            try
            {
                var response = await _productService.GetAllProductsAsync(filter, paginationIndex, paginationSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred when evaluating the products cataloge list.");
                return StatusCode(500);
            }
        }



        #endregion

        #region Create

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(product);
            try
            {
                var productDb = await _productService.CreateProductAsync(product);
                return Ok(productDb);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred when creating new product. {@product}  Please See the logs", product);
                return StatusCode(500);
            }
        }

        #endregion Create

        #region Update
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(product);
            try
            {
                var productDb = await _productService.UpdateProductAsync(product);
                return Ok(productDb);
            }
            catch (DataNotFoundException ex)
            {
                _logger.Error(ex, "Box with Id {Id} not found.", product.Id);
                return NotFound(product.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex , "An error occurred when editing new product. {@product} Please See the logs", product);
                return StatusCode(500);
            }
        }
        #endregion  Update

        #region Delete

        [HttpDelete(nameof(DeleteProduct))]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            try
            {
                var productDb = await GetProductById(id);
                await _productService.DeleteProductAsync(id);
                return Ok(productDb);
            }
            catch (DataNotFoundException ex)
            {
                _logger.Error(ex, "product with Id {Id} not found.", id);
                return NotFound(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred when deleting the product with Id {id}.", id);
                return StatusCode(500);
            }
        }

        #endregion Delete


    }
}
