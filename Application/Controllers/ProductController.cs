using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Application.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProduct _productRepository;
        private readonly UserManager<StockUser> _userManager;

        public ProductController(IProduct productRepository, UserManager<StockUser> userManager)
        {
            _productRepository = productRepository;
            _userManager = userManager;
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
            try 
            {
                var currentUser = (await _userManager.GetUserAsync(User)).Id.ToString();
                product.UserId = currentUser;
                await _productRepository.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }catch (Exception ex) 
            {
                ModelState.AddModelError("", $"Ocorreu um erro ao adicionar o produto: {ex.Message}");
                return View(product);
            }
        }

        // READ
        // GET: Produto
        public async Task<IActionResult> Index(string name)
        {
            IEnumerable<Product> products;

            var currentUser = _userManager.GetUserAsync(User).Result.Id;
            if (!string.IsNullOrEmpty(name))
            {
                products = _productRepository.GetProductByName(name).Result.Where(p => p.UserId == currentUser);
            }
            else
            {
                products = _productRepository.GetAllProducts().Result.Where(p => p.UserId == currentUser);
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

            try
            {
                var currentUser = (await _userManager.GetUserAsync(User)).Id.ToString();
                product.UserId = currentUser;
                await _productRepository.UpdateProduct(product);
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
