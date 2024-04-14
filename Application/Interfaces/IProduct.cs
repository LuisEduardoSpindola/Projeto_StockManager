using Domain.Models;

namespace Application.Interfaces
{
    public interface IProduct
    {
        Task<Product> CreateProduct(Product product);
        Task<Product> GetProductById(int productId);
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(int productId);
    }
}
