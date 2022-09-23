using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Models.APIIdentidade;
using NSE.WebApp.MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);
            var result = await _autenticacaoService.Registro(usuarioRegistro);
            
            if (ResponsePossuiErros(result.ErrorResponseUser))
            {
                return View(usuarioRegistro);
            }
            await RealizarLogin(result);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("login")]
        public IActionResult Login(string ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string returnurl = null)
        {
            if (!ModelState.IsValid) return View(usuarioLogin);

            var response = await _autenticacaoService.Login(usuarioLogin);

            if (ResponsePossuiErros(response.ErrorResponseUser))
            {
                return View(usuarioLogin);
            }

            await RealizarLogin(response);

            if (string.IsNullOrEmpty(returnurl)) return RedirectToAction("Index", "Home");


            return LocalRedirect(returnurl);

        }

        [HttpGet("sair")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        private async Task RealizarLogin(UsuarioAutenticacao usuarioAutenticacao)
        {
            var token = ObterTokenFormatado(usuarioAutenticacao.AccessToken);

            var claims = new List<System.Security.Claims.Claim>();
            claims.Add(new System.Security.Claims.Claim("JWT", usuarioAutenticacao.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private static JwtSecurityToken ObterTokenFormatado(string token)
        {
            return new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        }

    }
}
