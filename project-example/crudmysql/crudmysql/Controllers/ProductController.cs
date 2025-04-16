using crudmysql.DTOs.Product;
using crudmysql.Helpers;
using crudmysql.Models;
using crudmysql.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crudmysql.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            try
            {
                var (data, pagination) = await _productService.GetAllProduct(page, limit);
                return ResponseFormatter.Success(data,"Success",200,pagination);

            }
            catch (Exception ex)
            {
               return ResponseFormatter.Error(ex.Message,null);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return ResponseFormatter.ValidationError(dto);
            }
            try
            {
                var result = await _productService.Create(dto);
                return ResponseFormatter.Success(result);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Error(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {

                var ProductItem = await _productService.getById(id);
                if (ProductItem == null)
                {
                    return ResponseFormatter.NotFound("Product not found");
                }
                return ResponseFormatter.Success(ProductItem, "Product fetch successfully");
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Error(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] UpdateProductDTO dto)
        {
            try
            {
                var item = await _productService.getById(Id);
                if (item == null)
                {
                    return ResponseFormatter.NotFound("Product Not Found");
                }

                var product = new Product
                {
                    Id = Id,
                    Name = dto.Name,
                    Price = dto.Price,
                    Description = dto.Description,
                    Image = dto.Image
                };

                var result = await _productService.Update(product);
                return ResponseFormatter.Success(result, "Product updated successfully");
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Error(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var product = await _productService.Delete(Id);
                return ResponseFormatter.Success(product);
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseFormatter.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.ServerError(ex.Message);
            }
        }


    }
}
