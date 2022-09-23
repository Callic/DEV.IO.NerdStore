using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.AbstractsClass
{
    public abstract class Service
    {

        protected StringContent ObterStringContent(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json"
                );
        }

        protected bool TratarErrosResponse(HttpResponseMessage httpResponseMessage)
        {
            switch ((int)httpResponseMessage.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(httpResponseMessage.StatusCode);
                case 400:
                    return false;
            }

            httpResponseMessage.EnsureSuccessStatusCode();

            return true;
        }

        protected async Task<T> DesserializarObjeto<T>(HttpResponseMessage responseMessage)
        {
            var _JsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), _JsonOptions);
        }
    }
}
