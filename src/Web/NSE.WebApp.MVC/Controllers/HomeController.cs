using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            
        }

        public IActionResult Index()
        {
            ViewData["data-bloqueio"] = DateTime.Now;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modalErro = new ErrorViewModel();

            if (id == 500)
            {
                modalErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou cantate o nosso suporte.";
                modalErro.Titulo = "Ocorreu um erro!";
                modalErro.CodeErro = id;
            }else if (id == 404)
            {
                modalErro.Mensagem = "A página que você procura não existe! <br/>Em caso de dúvidas, entre em contato com nosso suporte.";
                modalErro.Titulo = "Opss! Página não encontrada";
                modalErro.CodeErro = id;
            }
            else if (id == 404)
            {
                modalErro.Mensagem = "Você não tem permissão para fazer isso.";
                modalErro.Titulo = "Acesso Negado";
                modalErro.CodeErro = id;
            }
            else
            {
                return StatusCode(id);
            }

            return View("Error", modalErro);
        }
    }
}
