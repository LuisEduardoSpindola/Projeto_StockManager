using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Application.Interfaces;

namespace Infraestructure.Controllers
{
    public class StockItemsController : Controller
    {
        private readonly IStockItem _stockItemRepository;
        private readonly IProduct _productRepository;
        private readonly IStore _storeRepository;

        public StockItemsController(IStockItem stockItemRepository, IProduct productRepository, IStore storeRepository)
        {

            _stockItemRepository = stockItemRepository;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
        }


        //CREATE
        //GET: ItemEstoque/Create
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

        // POST: ItemEstoque/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockItem itemEstoque)
        {
            if (!ModelState.IsValid)
            {
                var stores = await _storeRepository.GetAllStores();
                var products = await _productRepository.GetAllProducts();

                ViewBag.Stores = new SelectList(stores, "StoreId", "StoreName");
                ViewBag.Products = new SelectList(products, "ProductId", "ProductName");
                return View(itemEstoque);
            }

            await _stockItemRepository.CreateStockItem(itemEstoque);
            return RedirectToAction(nameof(Index));
        }

        // GET: StockItems
        public async Task<IActionResult> Index(string name)
        {
            IEnumerable<StockItem> stockItens;

            if (string.IsNullOrEmpty(name))
            {
                stockItens = await _stockItemRepository.GetAllStockItens();
            }
            else
            {
                stockItens = await _stockItemRepository.GetAllStockItens();
            }

            // Atualizar nomes de Loja e Produto
            foreach (var item in stockItens)
            {
                var store = await _storeRepository.GetStoreById(item.StockStoreId);
                var product = await _productRepository.GetProductById(item.StockProductId);

                item.StockStore.StoreName = store.StoreName;
                item.StockProduct.ProductName = product.ProductName;
            }

            return View(stockItens);
        }

        // GET: StockItems/Details/5
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

        // GET: StockItems/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _productRepository.GetAllProducts();
            var stores = await _storeRepository.GetAllStores();

            var stockItem = await _stockItemRepository.GetStocktById(id);
            ViewData["StockProductId"] = new SelectList(products, "ProductId", "ProductName", stockItem.StockProductId);
            ViewData["StockStoreId"] = new SelectList(stores, "StoreId", "Adress", stockItem.StockStoreId);
            return View(stockItem);
        }

        //---------------------------------------------------------------------------------------------------------------

        // POST: StockItems/Edit/


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

            var stockItem = await _stockItemRepository.GetStocktById(id);
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
