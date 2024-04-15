using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly IStore _storeRepository;

        public StoreController(IStore storeRepository)
        {
            _storeRepository = storeRepository;
        }

        // POST: Loja/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Loja/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Store loja)
        {
            if (!ModelState.IsValid)
            {
                return View(loja);
            }

            await _storeRepository.CreateStore(loja);
            return RedirectToAction(nameof(Index));
        }

        // GET: Loja
        public async Task<IActionResult> Index(string name)
        {
            IEnumerable<Store> stores;

            if (!string.IsNullOrEmpty(name)) 
            {
                stores = await _storeRepository.GetStoresByName(name);
            }
            else
            {
                stores = await _storeRepository.GetAllStores();
            }

            return View(stores);
        }

        //GET: Loja/Details
        public async Task<IActionResult> Details(int id) 
        {
            var stores = await _storeRepository.GetStoreById(id);
            if (stores == null) 
            {
                return NotFound();
            }
            return View(stores); 
        }

        // GET: Loja/Edit/
        public async Task<IActionResult> Edit(int id)
        {
            var stores = await _storeRepository.GetStoreById(id);
            if (stores == null)
            {
                return NotFound();
            }
            return View(stores);
        }

        // POST: Loja/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Store stores)
        {
            if (id != stores.StoreId)
            {
                return BadRequest("Os IDs da loja não correspondem.");
            }

            if (!ModelState.IsValid)
            {
                return View(stores);
            }

            try
            {
                await _storeRepository.UpdateStore(stores);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","Ocorreu um erro ao atualizar a loja. Tente novamente.");
                return View(stores);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Loja/Delete/
        public async Task<IActionResult> Delete(int id)
        {
            var loja = await _storeRepository.GetStoreById(id);
            if (loja == null)
            {
                return NotFound();
            }
            return View(loja);
        }

        // POST: Loja/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _storeRepository.DeleteStore(id);
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
