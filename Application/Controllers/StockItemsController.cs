using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infraestructure.Controllers
{
    [Authorize]
    public class StockItemsController : Controller
    {
        private readonly IStockItem _stockItemRepository;
        private readonly IProduct _productRepository;
        private readonly IStore _storeRepository;
        private readonly UserManager<StockUser> _userManager;

        public StockItemsController(IStockItem stockItemRepository, IProduct productRepository, IStore storeRepository, UserManager<StockUser> userManager)
        {

            _stockItemRepository = stockItemRepository;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
            _userManager = userManager;
        }


        //CREATE
        //GET: StockItems/Create
        public async Task<IActionResult> Create()
        {

            var stores = await _storeRepository.GetAllStores();
            var products = await _productRepository.GetAllProducts();

            ViewBag.Stores = new SelectList(stores, "StoreId", "StoreName");
            ViewBag.Products = new SelectList(products, "ProductId", "ProductName");

            if (ViewBag.Stores != null && ViewBag.Products != null)
            {
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Erro ao carregar lojas ou produtos.");
                return View();
            }
        }

        // POST: StockItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockItem stockItem)
        {

            try 
            {
                var currentUser = (await _userManager.GetUserAsync(User)).Id.ToString();
                stockItem.UserId = currentUser;
                var stores = await _storeRepository.GetAllStores();
                var products = await _productRepository.GetAllProducts();

                ViewBag.Stores = new SelectList(stores, "StoreId", "StoreName");
                ViewBag.Products = new SelectList(products, "ProductId", "ProductName");
                await _stockItemRepository.CreateStockItem(stockItem);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", $"Ocorreu um erro ao adicionar o registro: {ex.Message}");
                return View(stockItem);
            }
        }

        // GET: StockItems
        public async Task<IActionResult> Index(string name)
        {
            IEnumerable<StockItem> stockItens;
            var currentUser = (await _userManager.GetUserAsync(User)).Id.ToString();

            if (!string.IsNullOrEmpty(name))
            {
                stockItens =  _stockItemRepository.GetByProductName(name).Result.Where(p => p.UserId == currentUser);
            }
            else
            {
                stockItens = _stockItemRepository.GetAllStockItens().Result.Where(p => p.UserId == currentUser);
            }

            foreach (var item in stockItens)
            {
                var store = await _storeRepository.GetStoreById(item.StockStoreId);
                var product = await _productRepository.GetProductById(item.StockProductId);

                item.StockStore.StoreName = store.StoreName;
                item.StockProduct.ProductName = product.ProductName;
            }

            return View(stockItens);
        }

        // GET: StockItems/Details/
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockItem = await _stockItemRepository.GetStockDetails(id);

            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // GET: StockItems/Edit/{Id}
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else 
            {
                var products = await _productRepository.GetAllProducts();
                var stores = await _storeRepository.GetAllStores();

                var stockItem = await _stockItemRepository.GetStockById(id);
                ViewData["StockProductId"] = new SelectList(products, "ProductId", "ProductName", stockItem.StockProductId);
                ViewData["StockStoreId"] = new SelectList(stores, "StoreId", "StoreName", stockItem.StockStoreId);
                return View(stockItem);
            }
        }

        // POST: StockItems/Edit/{Id}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int stockId, StockItem stockItem)
        {
            if (stockId != stockItem.StockId)
            {
                return BadRequest("Os IDs não correspondem.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = (await _userManager.GetUserAsync(User)).Id.ToString();
                    stockItem.UserId = currentUser;
                    await _stockItemRepository.UpdateStockItem(stockItem);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao atualizar item de estoque: {ex.Message}");
                }
            }

            // Em caso de erro, reatribuir os valores dos produtos e lojas para as views
            var products = await _productRepository.GetAllProducts();
            var stores = await _storeRepository.GetAllStores();

            ViewBag.Products = new SelectList(products, "productId", "Nome", stockItem.StockProductId);
            ViewBag.Stores = new SelectList(stores, "storeId", "Nome", stockItem.StockStoreId);

            return View(stockItem);
        }

        // GET: StockItem/Delete
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _productRepository.GetAllProducts();
            var stores = await _storeRepository.GetAllStores();

            var stockItem = await _stockItemRepository.GetStockById(id);
            ViewData["StockProductId"] = new SelectList(products, "ProductId", "ProductName", stockItem.StockProductId);
            ViewData["StockStoreId"] = new SelectList(stores, "StoreId", "Adress", stockItem.StockStoreId);
            return View(stockItem);
        }

        // POST: StockItem/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int stockId)
        {
            try
            {
                await _stockItemRepository.DeleteStockItem(stockId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao excluir item de estoque: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
