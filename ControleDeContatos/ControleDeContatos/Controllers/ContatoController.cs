using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
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
            _contatoRepositorio.Adicionar(contato);
            return RedirectToAction("Index");
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

            _contatoRepositorio.Apagar(Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Alterar (ContatoModel contato)
        {

            _contatoRepositorio.Atualizar(contato);
            return RedirectToAction("Index");
        }
    }
}
