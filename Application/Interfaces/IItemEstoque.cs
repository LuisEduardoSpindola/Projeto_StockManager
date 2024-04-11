using Domain.Models;

namespace Application.Interfaces
{
    public interface IItemEstoque
    {
        Task<ItemEstoque> CreateStockItem(ItemEstoque itemEstoque);
        Task<ItemEstoque> GetItemEstoqueByStoreAndProduct(int lojaId, int produtoId);
        Task<IEnumerable<ItemEstoque>> GetAllItemsEstoque();
        Task<ItemEstoque> UpdateItemEstoque(ItemEstoque itemEstoque);
        Task DeleteItemEstoque(int lojaId, int produtoId);
        Task<ItemEstoque> UpdateQuantidade(int lojaId, int produtoId, int novaQuantidade);
    }
}
