using Domain.Models;

namespace Application.Interfaces
{
    public interface IStore
    {
        Task<Store> CreateStore(Store store);
        Task<Store> GetStoreById(int storeId);
        Task<IEnumerable<Store>> GetStoresByName(string name);
        Task<IEnumerable<Store>> GetAllStores();
        Task<Store> UpdateStore(Store store);
        Task DeleteStore(int storeId);
    }
}
