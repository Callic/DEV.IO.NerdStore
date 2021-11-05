using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Models.APIIdentidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<UsuarioAutenticacao> Login(UsuarioLogin usuarioLogin);
        Task<UsuarioAutenticacao> Registro(UsuarioRegistro usuarioRegistro);
    }
}
