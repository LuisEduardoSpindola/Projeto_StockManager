using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Models;

namespace Application.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProduto _produtoRepository;

        public ProdutoController(IProduto produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        // GET: Produto
        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepository.GetAllProducts();
            return View(produtos);
        }

        // GET: Produto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return View(produto);
            }

            await _produtoRepository.CreateProduct(produto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Produto/Edit/
        public async Task<IActionResult> Edit(int id)
        {
            var produto = await _produtoRepository.GetProductById(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto)
        {
            // Verifique se o ID da rota corresponde ao ID do produto
            if (id != produto.ProdutoId)
            {
                ModelState.AddModelError("IdMismatch", "Os IDs do produto não coincidem. Por favor, tente novamente.");
                return View(produto);
            }

            // Verifique se o modelo é válido
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ocorreu um erro ao atualizar o produto. Tente novamente.");
                return View(produto);
            }

            try
            {
                // Atualize o produto no repositório
                await _produtoRepository.UpdateProduct(produto);
                // Redirecione para a página de índice após a atualização bem-sucedida
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Adicione mensagens de erro ao `ModelState`
                ModelState.AddModelError("", "Ocorreu um erro ao atualizar o produto. Tente novamente.");
                // Exiba o formulário de edição com a mensagem de erro
                return View(produto);
            }
        }



        // GET: Produto/Delete/
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _produtoRepository.GetProductById(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _produtoRepository.DeleteProduct(id);
            }
            catch (Exception ex)
            {
                // Trate a exceção de maneira adequada
                ModelState.AddModelError("", "Ocorreu um erro ao excluir o produto. Tente novamente.");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
