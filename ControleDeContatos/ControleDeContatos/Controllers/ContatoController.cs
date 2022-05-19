using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    [PaginaParaUsuarioLogado]
    public class ContatoController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;

        public ContatoController(IContatoRepositorio contatoRepositorio)
        {
            _contatoRepositorio = contatoRepositorio;
        }

        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Index()
        {

            List<ContatoModel> contatos = _contatoRepositorio.BuscarTodos();
            return View(contatos);
        }
        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _contatoRepositorio.Adicionar(contato);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso";
                    return RedirectToAction("Index");
                }
                return View(contato);

            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu contato, tente novamente, detalhe do erro:{error.Message}";
                return RedirectToAction("Index");
            }
        }
        public IActionResult Editar(int Id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(Id);
            return View(contato);
        }
        public IActionResult ApagarConfirmacao(int Id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(Id);

            return View(contato);
        }

        public IActionResult Apagar(int Id)
        {
            try
            {

                bool apagado = _contatoRepositorio.Apagar(Id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Contato apagado com sucesso";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não conseguimos apagar seu contato";

                }
                return RedirectToAction("Index");

            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos apagar seu contato, detalhe do erro:{error.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(ContatoModel contato)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _contatoRepositorio.Atualizar(contato);
                    TempData["MensagemSucesso"] = "Contato alterado com sucesso";
                    return RedirectToAction("Index");
                }
                return View("Editar", contato);
            }
            catch (Exception erro)
            {

                TempData["MensagemErro"] = $"Ops, não conseguimos alterar seu contato, tente novamente, detalhe do erro:{erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
