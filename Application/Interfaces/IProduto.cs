using Domain.Models;

namespace Application.Interfaces
{
    public interface IProduto
    {
        Task<Produto> CreateProduct(Produto produto);
        Task<Produto> GetProductById(int ProdutoId);
        Task<Produto> GetProductByName(string Nome);
        Task<IEnumerable<Produto>> GetAllProducts();
        Task<Produto> UpdateProduct(Produto produto);
        Task DeleteProduct(int ProdutoId);
    }
}
