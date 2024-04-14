using Domain.Models;

namespace Application.Interfaces
{
    public interface IStockItem
    {
        Task<StockItem> CreateStockItem(StockItem stockItem);
        Task<IEnumerable<StockItem>> GetAllStockItens();
        Task<StockItem> GetStocktById(int stockId);
        Task<StockItem> GetStockDetails(int stockId);
        Task<StockItem> UpdateStockItem(StockItem stockItem);
        Task DeleteStockItem(int stockId);
    }
}
