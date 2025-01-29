using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Config.Test
{
    internal class PublicHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
         => new()
         {
             BaseAddress = new("https://discoverygc.com/gameconfigpublic/"),
             DefaultRequestHeaders = {
                 { HttpRequestHeader.Accept.ToString(), [ MediaTypeNames.Text.Plain ] }
             }
         };
    }
}
