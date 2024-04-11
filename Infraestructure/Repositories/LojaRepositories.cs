using Application.Interfaces;
using Domain.Models;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class LojaRepositories : ILoja
    {
        private readonly StockManagerContext _context;

        public LojaRepositories(StockManagerContext context)
        {
            _context = context;
        }

        public async Task<Loja> CreateStore(Loja loja)
        {
            _context.Set<Loja>().Add(loja);
            await _context.SaveChangesAsync();
            return loja;
        }

        public async Task DeleteStore(int LojaId)
        {
            var loja = await _context.Set<Loja>().FindAsync(LojaId);
            if (loja != null)
            {
                _context.Set<Loja>().Remove(loja);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Pode lançar uma exceção ou retornar um erro personalizado aqui.
                throw new KeyNotFoundException($"Loja com ID {LojaId} não foi encontrada.");
            }
        }

        public async Task<IEnumerable<Loja>> GetAllStores()
        {
            return await _context.Set<Loja>().ToListAsync();
        }

        public async Task<Loja> GetStoreById(int LojaId)
        {
            return await _context.Set<Loja>().FindAsync(LojaId);
        }

        public async Task<Loja> GetStoreByName(string Nome)
        {
            return await _context.Set<Loja>().FirstOrDefaultAsync(l => l.Nome == Nome);
        }

        public async Task<Loja> UpdateStore(Loja loja)
        {
            var lojaExistente = await _context.Set<Loja>().FindAsync(loja.LojaId);
            if (lojaExistente != null)
            {
                // Atualize as propriedades da loja existente com os valores da nova loja
                _context.Entry(lojaExistente).CurrentValues.SetValues(loja);
                await _context.SaveChangesAsync();
                return lojaExistente;
            }
            else
            {
                // Pode lançar uma exceção ou retornar um erro personalizado aqui.
                throw new KeyNotFoundException($"Loja com ID {loja.LojaId} não foi encontrada.");
            }
        }
    }
}
