using Application.Interfaces;
using Domain.Models;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class ItemEstoqueRepositories : IItemEstoque
    {
        private readonly StockManagerContext _context;

        public ItemEstoqueRepositories(StockManagerContext context)
        {
            _context = context;
        }

        public async Task<ItemEstoque> CreateStockItem(ItemEstoque itemEstoque)
        {
            _context.Set<ItemEstoque>().Add(itemEstoque);
            await _context.SaveChangesAsync();
            return itemEstoque;
        }

        public async Task DeleteItemEstoque(int lojaId, int produtoId)
        {
            var itemEstoque = await GetItemEstoqueByStoreAndProduct(lojaId, produtoId);
            if (itemEstoque != null)
            {
                _context.Set<ItemEstoque>().Remove(itemEstoque);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Pode lançar uma exceção ou retornar um erro personalizado aqui.
                throw new KeyNotFoundException($"ItemEstoque com LojaId {lojaId} e ProdutoId {produtoId} não foi encontrado.");
            }
        }

        public async Task<IEnumerable<ItemEstoque>> GetAllItemsEstoque()
        {
            return await _context.Set<ItemEstoque>().ToListAsync();
        }

        public async Task<ItemEstoque> GetItemEstoqueByStoreAndProduct(int lojaId, int produtoId)
        {
            return await _context.Set<ItemEstoque>().FirstOrDefaultAsync(ie => ie.LojaId == lojaId && ie.ProdutoId == produtoId);
        }

        public async Task<ItemEstoque> UpdateItemEstoque(ItemEstoque itemEstoque)
        {
            var itemEstoqueExistente = await GetItemEstoqueByStoreAndProduct(itemEstoque.LojaId, itemEstoque.ProdutoId);
            if (itemEstoqueExistente != null)
            {
                // Atualize as propriedades do item existente com os valores do novo itemEstoque
                _context.Entry(itemEstoqueExistente).CurrentValues.SetValues(itemEstoque);
                await _context.SaveChangesAsync();
                return itemEstoqueExistente;
            }
            else
            {
                // Pode lançar uma exceção ou retornar um erro personalizado aqui.
                throw new KeyNotFoundException($"ItemEstoque com LojaId {itemEstoque.LojaId} e ProdutoId {itemEstoque.ProdutoId} não foi encontrado.");
            }
        }

        public async Task<ItemEstoque> UpdateQuantidade(int lojaId, int produtoId, int novaQuantidade)
        {
            var itemEstoque = await GetItemEstoqueByStoreAndProduct(lojaId, produtoId);
            if (itemEstoque != null)
            {
                // Atualiza a quantidade
                itemEstoque.Quantidade = novaQuantidade;
                await _context.SaveChangesAsync();
                return itemEstoque;
            }
            else
            {
                // Pode lançar uma exceção ou retornar um erro personalizado aqui.
                throw new KeyNotFoundException($"ItemEstoque com LojaId {lojaId} e ProdutoId {produtoId} não foi encontrado.");
            }
        }
    }
}
