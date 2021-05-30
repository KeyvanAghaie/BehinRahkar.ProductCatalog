using DAL.Entities;
using Services.DTO;
using System.Threading.Tasks;

namespace BAL.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(ProductDTO product);
        Task<bool> DeleteProductAsync(long id);
        Task<PaginationModel<Product>> GetAllProductsAsync(string filter, int? paginationIndex, int? paginationSize);
        Task<Product> GetproductByCodeAsync(string code);
        Task<Product> GetProductByIdAsync(long productId);
        Task<Product> UpdateProductAsync(ProductDTO product);
    }
}