using crudmysql.DTOs.Pagination;
using crudmysql.DTOs.Product;
using crudmysql.Models;

public interface IProductService
{
    Task<(List<ProductDTO> Data, PaginationInfo Pagination)> GetAllProduct(int page, int perPage);
    Task<ProductDTO> Create(CreateProductDTO dto);
    Task<ProductDTO?> getById(int Id);
    Task<ProductDTO> Update(Product product);
    Task<ProductDTO> Delete(int Id);
}