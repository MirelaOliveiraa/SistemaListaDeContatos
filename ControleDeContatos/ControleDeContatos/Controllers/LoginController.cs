using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        public LoginController(IUsuarioRepositorio UsuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = UsuarioRepositorio;
            _sessao = sessao;
        }
        public IActionResult Index()
        {
            //Se o usuario estiver logado, redirecionar para a home

            if (_sessao.BuscarSessaoDoUsuario() != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    if (usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoDoUsuario(usuario);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Senha incorreta. Por favor, tente novamente";
                        }

                    }
                    else
                    {

                        TempData["MensagemErro"] = $"Usuário e/ou senha incorreto(s). Por favor tente novamente";
                    }
                }

                return View("Index");
            }

            catch (Exception erro)
            {

                TempData["MensagemErro"] = $"Ops, não conseguimos realizar seu login, tente novamente, detalhe do erro:{erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
