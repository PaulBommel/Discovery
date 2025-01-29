using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;

namespace Discovery.Darkstat.Test
{
    using NpcQueryClient;
    using OreFieldQueryClient;
    using PobQueryClient;
    using RouteQueryClient;

    [TestClass]
    public static class TestAssembly
    {
        public static readonly EndpointProvider Darkstat = new(new DarkstatHttpClientFactory());
        public static readonly EndpointProvider DarkstatStaging = new(new DarkstatStagingHttpClientFactory());

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {

        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {

        }

    }

    public class EndpointProvider
    {
        public EndpointProvider(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
            NpcQueryClient = new(clientFactory);
            MarketGoodQueryClient = new(clientFactory);
            CommodityQueryClient = new(clientFactory);
            OreFieldQueryClient = new(clientFactory);
            PobQueryClient = new(clientFactory);
            ShipInfoQueryClient = new(clientFactory);
            RouteQueryClient = new(clientFactory);
            RouteBuilder = new(NpcQueryClient, OreFieldQueryClient, PobQueryClient);
        }
        public IHttpClientFactory ClientFactory { get; }
        public NpcQueryClient NpcQueryClient { get; }
        public MarketGoodQueryClient MarketGoodQueryClient { get; }
        public CommodityQueryClient CommodityQueryClient { get; }
        public OreFieldQueryClient OreFieldQueryClient { get; }
        public PobQueryClient PobQueryClient { get; }
        public ShipInfoQueryClient ShipInfoQueryClient { get; }
        public RouteQueryClient RouteQueryClient { get; }
        public RouteBuilder RouteBuilder { get; }

    }
}
