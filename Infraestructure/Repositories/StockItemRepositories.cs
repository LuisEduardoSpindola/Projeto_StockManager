using Application.Interfaces;
using Domain.Models;
using Infraestructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Infraestructure.Repositories
{
    public class StockItemRepositories : IStockItem
    {
        private readonly StockManagerContext _context;

        public StockItemRepositories(StockManagerContext context)
        {
            _context = context;
        }

        // Criação de novo ItemEstoque
        public async Task<StockItem> CreateStockItem(StockItem stockItem)
        {
            _context.Set<StockItem>().Add(stockItem);
            await _context.SaveChangesAsync();
            return stockItem;
        }

        // Obter todos os ItensEstoque
        public async Task<IEnumerable<StockItem>> GetAllStockItens()
        {
            return await _context.Set<StockItem>().ToListAsync();
        }

        public async Task<StockItem> GetStockById(int stockId)
        {
            var stockItem = await _context.itensEstoque.FindAsync(stockId);
            if (stockItem == null)
            {
                throw new Exception("Não encontrado + ");
            }
            return stockItem;
        }

        public async Task<IEnumerable<StockItem>> GetByProductName(string stockStoreOrProductName)
        {
            if (string.IsNullOrEmpty(stockStoreOrProductName))
            {
                return await _context.Set<StockItem>().ToListAsync();
            }

            return await _context.itensEstoque
                .Where(n => n.StockProduct.ProductName.Contains(stockStoreOrProductName))
                .ToListAsync();
        }

        // Obter ItemEstoque por storeId e productId
        public async Task<StockItem> GetStockItemByStoreAndProduct(int storeId, int productId)
        {
            return await _context.Set<StockItem>()
                .FirstOrDefaultAsync(ie => ie.StockStoreId == storeId && ie.StockProductId == productId);
        }

        // Obter ItemEstoque por nome de Produto ou Loja
        public async Task<StockItem> GetStockDetails(int stockId)
        {
            var stockItem = await _context.itensEstoque
                .Include(s => s.StockProduct)
                .Include(s => s.StockStore)
                .FirstOrDefaultAsync(m => m.StockId == stockId);

            return stockItem;
        }

        // Atualizar ItemEstoque
        public async Task<StockItem> UpdateStockItem(StockItem stockItem)
        {
            var existingItem = await _context.Set<StockItem>()
                .FirstOrDefaultAsync(si => si.StockId == stockItem.StockId);

            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(stockItem);
                await _context.SaveChangesAsync();
                return existingItem;
            }

            throw new KeyNotFoundException($"ItemEstoque com stockId {stockItem.StockId} não foi encontrado.");
        }

        // Deletar ItemEstoque
        public async Task DeleteStockItem(int stockId)
        {
            var stockItem = await _context.Set<StockItem>()
                .FirstOrDefaultAsync(si => si.StockId == stockId);

            if (stockItem != null)
            {
                _context.Set<StockItem>().Remove(stockItem);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"ItemEstoque com stockId {stockId} não foi encontrado.");
            }
        }
    }
}