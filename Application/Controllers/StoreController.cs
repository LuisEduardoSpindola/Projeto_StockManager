using Application.Interfaces;
using Application.ViaCepAPI;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly IStore _storeRepository;
        private readonly UserManager<StockUser> _userManager;
        private readonly ViaCEPService _viaCEPService;

        public StoreController(IStore storeRepository, UserManager<StockUser> userManager, ViaCEPService viaCEPService)
        {
            _storeRepository = storeRepository;
            _userManager = userManager;
           _viaCEPService = viaCEPService;
        }

        // POST: Loja/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Loja/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Store store)
        {
            try
            {
                var addressInfo = await _viaCEPService.GetAddressInfo(store.CEP);


                store.Adress = addressInfo.Logradouro;
                store.City = addressInfo.Cidade;
                store.Neighborhood = addressInfo.Bairro;

                var currentUser = (await _userManager.GetUserAsync(User)).Id.ToString();
                store.UserId = currentUser;
                await _storeRepository.CreateStore(store);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocorreu um erro ao adicionar a loja: {ex.Message}");
                return View(store);
            }
        }

        // GET: Loja
        public async Task<IActionResult> Index(string name)
        {
            IEnumerable<Store> stores;
            var currentUser = (await _userManager.GetUserAsync(User)).Id.ToString();


            if (!string.IsNullOrEmpty(name)) 
            {
                stores = _storeRepository.GetStoresByName(name).Result.Where(p => p.UserId == currentUser);
            }
            else
            {
                stores = _storeRepository.GetAllStores().Result.Where(p => p.UserId == currentUser);
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
        public async Task<IActionResult> Edit(int id, Store store)
        {
            if (id != store.StoreId)
            {
                return BadRequest("Os IDs da loja não correspondem.");
            }

            if (!ModelState.IsValid)
            {
                return View(store);
            }

            try
            {
                var addressInfo = await _viaCEPService.GetAddressInfo(store.CEP);


                store.Adress = addressInfo.Logradouro;
                store.City = addressInfo.Cidade;
                store.Neighborhood = addressInfo.Bairro;

                var currentUser = (await _userManager.GetUserAsync(User)).Id.ToString();
                store.UserId = currentUser;
                await _storeRepository.UpdateStore(store);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","Ocorreu um erro ao atualizar a loja. Tente novamente.");
                return View(store);
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
                ModelState.AddModelError("", "Ocorreu um erro ao excluir a loja. Tente novamente.");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
