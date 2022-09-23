using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using System.Linq;

namespace NSE.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        public bool ResponsePossuiErros(ErrorResponseUser errorResponse)
        {

            if (errorResponse != null && errorResponse.Errors.Messages.Any())
            {
                foreach (var item in errorResponse.Errors.Messages)
                {
                    ModelState.AddModelError(string.Empty, item);
                }
                return true;
            }
            return false;
        }
    }
}
