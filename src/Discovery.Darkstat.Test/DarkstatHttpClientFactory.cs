using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Darkstat.Test
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
    internal class DarkstatStagingHttpClientFactory : IHttpClientFactory
    {
        private readonly Lazy<HttpClient> _client = new(() => new()
        {
            BaseAddress = new("https://darkstat-staging.dd84ai.com"),
            DefaultRequestHeaders = {
                 { HttpRequestHeader.Accept.ToString(), [ MediaTypeNames.Application.Json ] }
             }
        });
        public HttpClient CreateClient(string name)
         => _client.Value;
    }
}
