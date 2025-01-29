using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.TradeMonitor.Test
{
    internal class DarkstatHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
         => new()
         {
             BaseAddress = new("https://darkstat.dd84ai.com"),
             DefaultRequestHeaders = {
                 { HttpRequestHeader.Accept.ToString(), [ MediaTypeNames.Application.Json ] }
             }
         };
    }
    internal class DarkstatStagingHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
         => new()
         {
             BaseAddress = new("https://darkstat-staging.dd84ai.com"),
             DefaultRequestHeaders = {
                 { HttpRequestHeader.Accept.ToString(), [ MediaTypeNames.Application.Json ] }
             }
         };
    }
}
