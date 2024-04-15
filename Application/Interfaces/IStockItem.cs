using Domain.Models;

namespace Application.Interfaces
{
    public interface IStockItem
    {
        Task<StockItem> CreateStockItem(StockItem stockItem);
        Task<IEnumerable<StockItem>> GetAllStockItens();
        Task<StockItem> GetStockById(int stockId);
        Task<IEnumerable<StockItem>> GetByProductName(string stockStoreOrProductName);
        Task<StockItem> GetStockDetails(int stockId);
        Task<StockItem> UpdateStockItem(StockItem stockItem);
        Task DeleteStockItem(int stockId);
    }
}
