using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    [PaginaRestritaSomenteAdm]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = _usuarioRepositorio.BuscarTodos();
            return View(usuarios);
        }

        public IActionResult Criar()
        {
            return View();
        }
        public IActionResult Editar(int Id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(Id);
            return View(usuario);
        }
        public IActionResult ApagarConfirmacao(int Id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(Id);

            return View(usuario);
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _usuarioRepositorio.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso";
                    return RedirectToAction("Index");
                }
                return View(usuario);

            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu usuário, tente novamente, detalhe do erro:{error.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Apagar(int Id)
        {
            try
            {

                bool apagado = _usuarioRepositorio.Apagar(Id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Usuário apagado com sucesso";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não conseguimos apagar seu usuário";

                }
                return RedirectToAction("Index");

            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos apagar seu usuário, detalhe do erro:{error.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Alterar(UsuarioSemSenhaModel usuarioSemSenhaModel)
        {
            try
            {
                UsuarioModel usuario = null;
                if (ModelState.IsValid)
                {
                    usuario = new UsuarioModel()
                    {
                        Id = usuarioSemSenhaModel.Id,
                        Nome = usuarioSemSenhaModel.Nome,
                        Login = usuarioSemSenhaModel.Login,
                        Email = usuarioSemSenhaModel.Email,
                        Perfil = usuarioSemSenhaModel.Perfil
                    };
                    usuario = _usuarioRepositorio.Atualizar(usuario);
                    TempData["MensagemSucesso"] = "Usuário alterado com sucesso";
                    return RedirectToAction("Index");
                }
                return View("Editar", usuario);
            }
            catch (Exception erro)
            {

                TempData["MensagemErro"] = $"Ops, não conseguimos alterar seu usuário, tente novamente, detalhe do erro:{erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
