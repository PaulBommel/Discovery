﻿using System.Net;
using System.Net.Http;
using System.Net.Mime;

namespace Discovery.Prototypes.TradeMonitor
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
}
