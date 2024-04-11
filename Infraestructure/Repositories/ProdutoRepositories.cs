using Application.Interfaces;
using Domain.Models;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class ProdutoRepositories : IProduto
    {
        private readonly StockManagerContext  _context;

        public ProdutoRepositories(StockManagerContext context)
        {
            _context = context;
        }

        public async Task<Produto> CreateProduct(Produto produto)
        {
            _context.Set<Produto>().Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task DeleteProduct(int Id)
        {
            var produto = await _context.Set<Produto>().FindAsync(Id);
            if (produto != null)
            {
                _context.Set<Produto>().Remove(produto);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Pode lançar uma exceção ou retornar um erro personalizado aqui.
                throw new KeyNotFoundException($"Produto com ID {Id} não foi encontrado.");
            }
        }

        public async Task<IEnumerable<Produto>> GetAllProducts()
        {
            return await _context.Set<Produto>().ToListAsync();
        }

        public async Task<Produto> GetProductById(int Id)
        {
            return await _context.Set<Produto>().FindAsync(Id);
        }

        public async Task<Produto> GetProductByName(string Nome)
        {
            return await _context.Set<Produto>().FirstOrDefaultAsync(p => p.Nome == Nome);
        }

        public async Task<Produto> UpdateProduct(Produto produto)
        {
            var produtoExistente = await _context.Set<Produto>().FindAsync(produto.ProdutoId);
            if (produtoExistente != null)
            {
                // Atualize as propriedades do produto existente com os valores do novo produto
                _context.Entry(produtoExistente).CurrentValues.SetValues(produto);
                await _context.SaveChangesAsync();
                return produtoExistente;
            }
            else
            {
                // Pode lançar uma exceção ou retornar um erro personalizado aqui.
                throw new KeyNotFoundException($"Produto com ID {produto.ProdutoId} não foi encontrado.");
            }
        }
    }
}
