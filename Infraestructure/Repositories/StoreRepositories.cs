using Application.Interfaces;
using Domain.Models;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class StoreRepositories : IStore
    {
        private readonly StockManagerContext _context;

        public StoreRepositories(StockManagerContext context)
        {
            _context = context;
        }

        // Criação de nova Loja
        public async Task<Store> CreateStore(Store store)
        {
            _context.Set<Store>().Add(store);
            await _context.SaveChangesAsync();
            return store;
        }

        // Exclusão de uma Loja
        public async Task DeleteStore(int storeId)
        {
            var store = await _context.Set<Store>().FindAsync(storeId);
            if (store != null)
            {
                _context.Set<Store>().Remove(store);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Loja com ID {storeId} não foi encontrada.");
            }
        }

        // Obtenção de todas as Lojas
        public async Task<IEnumerable<Store>> GetAllStores()
        {
            return await _context.Set<Store>().ToListAsync();
        }

        // Obtenção de Loja por ID
        public async Task<Store> GetStoreById(int storeId)
        {
            var store = _context.Set<Store>().FirstOrDefault(p => p.StoreId == storeId);
            if (store == null)
            {
                throw new Exception("Não encontrado");
            }
            return store;
        }

        // Obtenção de Lojas por nome
        public async Task<IEnumerable<Store>> GetStoresByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Enumerable.Empty<Store>();
            }
            return await _context.Set<Store>()
                .Where(s => s.StoreName.Contains(name))
                .ToListAsync();
        }

        // Atualização de uma Loja
        public async Task<Store> UpdateStore(Store store)
        {
            var existingStore = await _context.Set<Store>().FindAsync(store.StoreId);
            if (existingStore != null)
            {
                _context.Entry(existingStore).CurrentValues.SetValues(store);
                await _context.SaveChangesAsync();
                return existingStore;
            }
            else
            {
                throw new KeyNotFoundException($"Loja com ID {store.StoreId} não foi encontrada.");
            }
        }
    }
}
