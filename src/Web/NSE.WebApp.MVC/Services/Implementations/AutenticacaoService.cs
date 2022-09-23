using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.AbstractsClass;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Models.APIIdentidade;
using NSE.WebApp.MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Implementations
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        private readonly AppSettings _appSettings;

        public AutenticacaoService(HttpClient httpClient, 
            IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
        }

        public async Task<UsuarioAutenticacao> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = ObterStringContent(usuarioLogin);
            var response = await _httpClient.PostAsync($"{_appSettings.APIIdentidadeURL}/api/identidade/login", loginContent);
            
            if (!TratarErrosResponse(response))
            {
                return new UsuarioAutenticacao
                {
                    ErrorResponseUser = await DesserializarObjeto<ErrorResponseUser>(response)
                    
                };
            }

            return await DesserializarObjeto<UsuarioAutenticacao>(response);
        }

        public async Task<UsuarioAutenticacao> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = ObterStringContent(usuarioRegistro);


            var response = await _httpClient.PostAsync($"{_appSettings.APIIdentidadeURL}/api/identidade/nova-conta", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioAutenticacao
                {
                    ErrorResponseUser = await DesserializarObjeto<ErrorResponseUser>(response)
                };
            }
            return await DesserializarObjeto<UsuarioAutenticacao>(response);
        }
    }
}
