using crudmysql.Data;
using crudmysql.DTOs;
using crudmysql.DTOs.Pagination;
using crudmysql.DTOs.Product;
using crudmysql.Models;
using crudmysql.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace crudmysql.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IUrlService _urlService;


        public ProductService(ApplicationDbContext context, ILogger<ProductService> logger, IUrlService urlService)
        {
            _context = context;
            _logger = logger;
            _urlService = urlService;
        }

        public async Task<(List<ProductDTO> Data, PaginationInfo Pagination)> GetAllProduct(int page, int perPage)
        {

            var total = await _context.Products.CountAsync();

            var items = await _context.Products
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Image = _urlService.GetImageUrl(p.Image),
                })
                .ToListAsync();

            var pagination = new PaginationInfo
            {
                Page = page,
                PerPage = perPage,
                Total = total,
                TotalPages = (int)Math.Ceiling(total / (double)perPage),
                From = ((page - 1) * perPage) + 1,
                To = Math.Min(page * perPage, total)
            };

            return (items, pagination);
        }



        public async Task<ProductDTO> Create(CreateProductDTO dto)
        {
            // string imagePath = null;

            // if (imageFile != null && imageFile.Length > 0)
            // {
            //     var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            //     var folderPath = Path.Combine("wwwroot", "images");
            //     var filePath = Path.Combine(folderPath, fileName);

            //     // Pastikan foldernya ada
            //     Directory.CreateDirectory(folderPath);

            //     using (var stream = new FileStream(filePath, FileMode.Create))
            //     {
            //         await imageFile.CopyToAsync(stream);
            //     }

            //     imagePath = $"/images/{fileName}";
            // }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Image = "imagePath"
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Image = product.Image
            };
        }



        public async Task<ProductDTO?> getById(int id)
        {
            try
            {
                var Product = await _context.Products.Where(u => u.Id == id).Select(u => new ProductDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Price = u.Price,
                    Description = u.Description,
                    Image = _urlService.GetImageUrl(u.Image),
                }).FirstOrDefaultAsync();

                return Product;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user.", ex);
            }
        }

        public async Task<ProductDTO> Update(Product product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                var productUpdate = await getById(product.Id);
                return productUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user.", ex);

            }
        }

        public async Task<ProductDTO> Delete(int Id)
        {
            var productEntity = await _context.Products.FindAsync(Id);

            if (productEntity == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            else
            {
                // Hapus dari database
                _context.Products.Remove(productEntity);
                await _context.SaveChangesAsync();

                // Kembalikan dalam bentuk DTO
                return new ProductDTO
                {
                    Id = productEntity.Id,
                    Name = productEntity.Name,
                    Price = productEntity.Price,
                    Description = productEntity.Description,
                    Image = productEntity.Image
                };

            }
        }

    }
}
