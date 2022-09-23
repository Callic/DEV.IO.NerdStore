using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NSE.WebApp.MVC.Models
{
    public class ErrorViewModel
    {
        public int CodeErro { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
    }
    public class ErrorResponseUser
    {
        public string Title { get; set; }
        public int Status { get; set; }

        public MessagesErrorResponseUser Errors { get; set; }
    }
    public class MessagesErrorResponseUser
    {
        [JsonPropertyName("Mensagens")]
        public IEnumerable<string> Messages { get; set; }
    }
}
