using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;

namespace Discovery.Delivery.Debugger
{
    internal class DarkstatHttpClientFactory : IHttpClientFactory
    {
        private readonly Lazy<HttpClient> _client = new(() => new()
        {
            BaseAddress = new("https://darkstat.dd84ai.com"),
            DefaultRequestHeaders = {
                 { HttpRequestHeader.Accept.ToString(), [ MediaTypeNames.Application.Json ] }
             }
        });
        public HttpClient CreateClient(string name)
         => _client.Value;
    }
}
