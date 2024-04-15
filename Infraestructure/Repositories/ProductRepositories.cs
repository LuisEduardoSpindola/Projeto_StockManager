using Application.Interfaces;
using Domain.Models;
using Infraestructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace Infraestructure.Repositories
{
    public class ProductRepositories : IProduct
    {
        private readonly StockManagerContext _context;

        public ProductRepositories(StockManagerContext context)
        {
            _context = context;
        }

        // Criação de novo Produto
        public async Task<Product> CreateProduct(Product product)
        {
            _context.Set<Product>().Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // Obtenção de todos os Produtos
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Set<Product>().ToListAsync();
        }

        // Obtenção de Produto por ID
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Set<Product>().FindAsync(id);
        }

        // Obtenção de Produtos por nome
        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Enumerable.Empty<Product>();
            }

            return await _context.products
                .Where(n => n.ProductName.Contains(name))
                .ToListAsync();
        }

        // Atualização de Produto existente
        public async Task<Product> UpdateProduct(Product product)
        {
            var searchProduct = await _context.Set<Product>().FindAsync(product.ProductId);
            if (searchProduct != null)
            {
                _context.Entry(searchProduct).CurrentValues.SetValues(product);
                await _context.SaveChangesAsync();
                return searchProduct;
            }
            else
            {
                throw new KeyNotFoundException($"Produto com ID {product.ProductId} não foi encontrado.");
            }
        }

        // Exclusão de Produto por ID
        public async Task DeleteProduct(int id)
        {
            var product = await _context.Set<Product>().FindAsync(id);
            if (product != null)
            {
                _context.Set<Product>().Remove(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Produto com ID {id} não foi encontrado.");
            }
        }
    }
}
