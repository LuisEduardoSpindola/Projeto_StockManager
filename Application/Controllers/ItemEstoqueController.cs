using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Controllers
{
    public class ItemEstoqueController : Controller
    {
        private readonly IItemEstoque _itemEstoqueRepository;
        private readonly IProduto _produtoRepository;
        private readonly ILoja _lojaRepository;

        public ItemEstoqueController(IItemEstoque itemEstoqueRepository, IProduto produtoRepository, ILoja lojaRepository)
        {
            _itemEstoqueRepository = itemEstoqueRepository;
            _produtoRepository = produtoRepository;
            _lojaRepository = lojaRepository;
        }

        // GET: ItemEstoque
        public async Task<IActionResult> Index()
        {
            // Obter a lista de itens de estoque
            var itensEstoque = await _itemEstoqueRepository.GetAllItemsEstoque();

            // Iterar sobre os itens de estoque para buscar detalhes adicionais
            foreach (var item in itensEstoque)
            {
                // Obter a loja e o produto associado a cada item
                var loja = await _lojaRepository.GetStoreById(item.LojaId);
                var produto = await _produtoRepository.GetProductById(item.ProdutoId);

                // Atribuir os nomes da loja e do produto ao item de estoque
                item.Loja.Nome = loja.Nome;
                item.Produto.Nome = produto.Nome;
            }

            // Retornar a view com a lista de itens de estoque
            return View(itensEstoque);
        }


        // GET: ItemEstoque/Create
        public async Task<IActionResult> Create()
        {
            var lojas = await _lojaRepository.GetAllStores();
            var produtos = await _produtoRepository.GetAllProducts();

            ViewBag.Lojas = new SelectList(lojas, "LojaId", "Nome");
            ViewBag.Produtos = new SelectList(produtos, "ProdutoId", "Nome");

            return View();
        }



        // POST: ItemEstoque/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemEstoque itemEstoque)
        {
            if (!ModelState.IsValid)
            {
                return View(itemEstoque);
            }

            await _itemEstoqueRepository.CreateStockItem(itemEstoque);
            return RedirectToAction(nameof(Index));
        }

        // GET: ItemEstoque/Edit/
        public async Task<IActionResult> Edit(int lojaId, int produtoId)
        {
            var itemEstoque = await _itemEstoqueRepository.GetItemEstoqueByStoreAndProduct(lojaId, produtoId);
            if (itemEstoque == null)
            {
                return NotFound();
            }
            return View(itemEstoque);
        }

        // POST: ItemEstoque/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int lojaId, int produtoId, ItemEstoque itemEstoque)
        {
            if (lojaId != itemEstoque.LojaId || produtoId != itemEstoque.ProdutoId)
            {
                return BadRequest("Os IDs da loja ou do produto não correspondem.");
            }

            if (!ModelState.IsValid)
            {
                return View(itemEstoque);
            }

            try
            {
                await _itemEstoqueRepository.UpdateItemEstoque(itemEstoque);
            }
            catch (Exception ex)
            {
                // Trate exceções de maneira adequada
                ModelState.AddModelError("", "Ocorreu um erro ao atualizar o item de estoque.");
                return View(itemEstoque);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ItemEstoque/Delete/
        public async Task<IActionResult> Delete(int lojaId, int produtoId)
        {
            var itemEstoque = await _itemEstoqueRepository.GetItemEstoqueByStoreAndProduct(lojaId, produtoId);
            if (itemEstoque == null)
            {
                return NotFound();
            }
            return View(itemEstoque);
        }

        // POST: ItemEstoque/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int lojaId, int produtoId)
        {
            try
            {
                await _itemEstoqueRepository.DeleteItemEstoque(lojaId, produtoId);
            }
            catch (Exception ex)
            {
                // Trate exceções de maneira adequada
                ModelState.AddModelError("", "Ocorreu um erro ao excluir o item de estoque.");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
