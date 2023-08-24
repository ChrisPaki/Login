using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Login.Permisos
{
    public class validarSesionAttribute: ActionFilterAttribute
    {   
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            if (filterContext.HttpContext.Session.GetString("Usuario") as String == null)
            {

                filterContext.Result = new RedirectResult("~/Login/IniciarSesion");
            }


            base.OnActionExecuting(filterContext);
        }

    }
}
