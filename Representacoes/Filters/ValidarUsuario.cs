using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Representacoes.Filters
{
    public class ValidarUsuario : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var id = (string)filterContext.HttpContext.Session["id"];
            var nivel = (string)filterContext.HttpContext.Session["nivel"];
            var controller = (string)filterContext.RouteData.Values["controller"];
            var action = (string)filterContext.RouteData.Values["action"];
            //var ipCliente = context.HttpContext.Connection.RemoteIpAddress;
            //var browser = context.HttpContext.Request.Headers["User-Agent"].ToString();
            //var urlReferrer = context.HttpContext.Request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(id))
            {
                filterContext.Result = new RedirectResult("/Login/Logout");
            }
            else
            {
                if ((nivel == "2" || nivel == "3") &&
                    new[] { "funcionario", "parametrizacao" }.Contains(controller.ToLower()) &&
                    new[] { "index", "detalhes", "novo" }.Contains(action.ToLower()))
                {
                    filterContext.Result = new RedirectResult("/Home/Denied");
                }
            }
        }
    }
}