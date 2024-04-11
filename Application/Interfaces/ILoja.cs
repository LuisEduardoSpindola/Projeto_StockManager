using Domain.Models;

namespace Application.Interfaces
{
    public interface ILoja
    {
        Task<Loja> CreateStore(Loja produto);
        Task<Loja> GetStoreById(int LojaId);
        Task<Loja> GetStoreByName(string Nome);
        Task<IEnumerable<Loja>> GetAllStores();
        Task<Loja> UpdateStore(Loja produto);
        Task DeleteStore(int LojaId);
    }
}
