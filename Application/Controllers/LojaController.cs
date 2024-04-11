using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Controllers
{
    public class LojaController : Controller
    {
        private readonly ILoja _lojaRepository;

        public LojaController(ILoja lojaRepository)
        {
            _lojaRepository = lojaRepository;
        }

        // GET: Loja
        public async Task<IActionResult> Index()
        {
            var lojas = await _lojaRepository.GetAllStores();
            return View(lojas);
        }

        // GET: Loja/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Loja/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Loja loja)
        {
            if (!ModelState.IsValid)
            {
                return View(loja);
            }

            await _lojaRepository.CreateStore(loja);
            return RedirectToAction(nameof(Index));
        }

        // GET: Loja/Edit/
        public async Task<IActionResult> Edit(int id)
        {
            var loja = await _lojaRepository.GetStoreById(id);
            if (loja == null)
            {
                return NotFound();
            }
            return View(loja);
        }

        // POST: Loja/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Loja loja)
        {
            if (id != loja.LojaId)
            {
                return BadRequest("Os IDs da loja não correspondem.");
            }

            if (!ModelState.IsValid)
            {
                return View(loja);
            }

            try
            {
                await _lojaRepository.UpdateStore(loja);
            }
            catch (Exception ex)
            {
                // Trate a exceção de maneira adequada
                ModelState.AddModelError("", "Ocorreu um erro ao atualizar a loja. Tente novamente.");
                return View(loja);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Loja/Delete/
        public async Task<IActionResult> Delete(int id)
        {
            var loja = await _lojaRepository.GetStoreById(id);
            if (loja == null)
            {
                return NotFound();
            }
            return View(loja);
        }

        // POST: Loja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _lojaRepository.DeleteStore(id);
            }
            catch (Exception ex)
            {
                // Trate a exceção de maneira adequada
                ModelState.AddModelError("", "Ocorreu um erro ao excluir a loja. Tente novamente.");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
