using Microsoft.AspNetCore.Http;
using NSE.WebApp.MVC.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions.Implementations
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Name => _httpContextAccessor.HttpContext.User.Identity.Name;

        public bool EstaAutenticado()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public HttpContext ObterHttpContext()
        {
            return _httpContextAccessor.HttpContext;
        }

        public string ObterUserEmail()
        {
            return EstaAutenticado() ? _httpContextAccessor.HttpContext.User.GetUserEmail() : "";
        }

        public Guid ObterUserId()
        {
            return EstaAutenticado() ? Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserGuid()) : Guid.Empty;
        }

        public string ObterUserToken()
        {
            return EstaAutenticado() ? _httpContextAccessor.HttpContext.User.GetUserToken() : "";
        }

        public bool PossuiRole(string role)
        {
            return _httpContextAccessor.HttpContext.User.IsInRole(role);
        }
    }
    public static class HttpContextExtension
    {
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }
            var claim = principal.FindFirst("email");
            return claim?.Value;
        }
        public static string GetUserGuid(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }
            var claim = principal.FindFirst("sub");
            return claim?.Value;
        }
        public static string GetUserToken(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }
            var claim = principal.FindFirst("JWT");
            return claim?.Value;
        }
    }
}
