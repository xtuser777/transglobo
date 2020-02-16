using Representacoes.Filters;
using System;
using System.Web;
using System.Web.Mvc;

namespace Representacoes.Controllers
{
    [ValidarUsuario]
    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Index()
        {
            return View();
        }
    }
}