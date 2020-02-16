using System.Web.Mvc;
using System.Web.Script.Services;
using Representacoes.Models;
using Representacoes.ViewModels;

namespace Representacoes.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Autenticar(string login, string senha)
        {
            Usuario u = new Usuario().Autenticar(login, senha);
            UsuarioVM usu = u != null ? u.ToViewModel() : null;
            if (usu != null)
            {
                HttpContext.Session["id"] = usu.Id.ToString();
                HttpContext.Session["login"] = usu.Login;
                HttpContext.Session["nome"] = usu.Funcionario.Pessoa.Nome;
                HttpContext.Session["nivel"] = usu.Nivel.Id.ToString();

                return Json(new { msg = "", user = usu.Login });
            }

            return Json(new { msg = "Usuário ou senha incoerretos ou usuário desativado.", user = "" });
        }
        
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Login/Index");
        }
    }
}