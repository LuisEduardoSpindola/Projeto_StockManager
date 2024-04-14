using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProduct _productRepository;

        public ProductController(IProduct productRepository)
        {
            _productRepository = productRepository;
        }

        // CREATE
        // GET: Produto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            await _productRepository.CreateProduct(product);
            return RedirectToAction(nameof(Index));
        }

        // READ
        // GET: Produto
        public async Task<IActionResult> Index(string name)
        {
            IEnumerable<Product> products;

            if (!string.IsNullOrEmpty(name))
            {
                products = await _productRepository.GetProductByName(name);
            }
            else
            {
                products = await _productRepository.GetAllProducts();
            }

            return View(products);
        }

        // GET: Produto/Details/
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // UPDATE
        // GET: Produto/Edit/
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest("O ID fornecido não corresponde ao produto a ser editado.");
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            try
            {
                await _productRepository.UpdateProduct(product);
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", $"Ocorreu um erro ao atualizar o produto: {ex.Message}");
                return View(product);
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("", $"Produto com ID {product.ProductId} não foi encontrado: {ex.Message}");
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE
        // GET: Produto/Delete/
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Produto/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productRepository.DeleteProduct(id);
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("", $"Produto com ID {id} não foi encontrado: {ex.Message}");
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
