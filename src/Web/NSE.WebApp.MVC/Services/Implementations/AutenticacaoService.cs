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
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        private string urlBase = "https://localhost:44313";
        private JsonSerializerOptions _JsonOptions;

        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _JsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<UsuarioAutenticacao> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = new StringContent(
                JsonSerializer.Serialize(usuarioLogin),
                Encoding.UTF8,
                "application/json"
                );
            var response = await _httpClient.PostAsync($"{urlBase}/api/identidade/login", loginContent);
            
            return JsonSerializer.Deserialize<UsuarioAutenticacao>(await response.Content.ReadAsStringAsync(), _JsonOptions);
        }

        public Task<UsuarioAutenticacao> Registro(UsuarioRegistro usuarioRegistro)
        {
            throw new NotImplementedException();
        }
    }
}
