using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Models.APIIdentidade
{
    public class UsuarioAutenticacao
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken UsuarioToken { get; set; }
    }
    public class UsuarioToken
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
    public class Claim
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
