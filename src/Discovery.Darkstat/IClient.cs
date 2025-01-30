using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat
{
    using NpcQueryClient;
    using OreFieldQueryClient;
    using PobQueryClient;
    using RouteQueryClient;
    public interface IMarkedGoodQueryClient
    {
        Task<MarketGoodData[]> GetMarketGoodsPerBasesAsync(string[] baseNicknames, CancellationToken token = default);
        Task<MarketGoodData[]> GetCommoditiesPerNicknameAsync(string[] commodityNicknames, CancellationToken token = default);
    }

    public interface INpcBaseQueryClient
    {
        Task<NpcData[]> GetNpcBasesAsync(CancellationToken token = default);
    }

    public interface IOreFieldQueryClient
    {
        Task<OreFieldData[]> GetOreFieldsAsync(CancellationToken token = default);
    }

    public interface IPlayerBaseQueryClient
    {
        Task<PobData[]> GetPlayerBasesAsync(CancellationToken token = default);
    }

    public interface IRouteQueryClient
    {
        Task<RouteData[]> GetTimingsAsync(Route[] routes, CancellationToken token = default);
    }

    public interface ICommodityQueryClient
    {
        Task<Commodity[]> GetCommoditiesAsync(CancellationToken token = default);
    }

    public interface IShipInfoQueryClient
    {
        Task<ShipInfo[]> GetShipInfosAsync(CancellationToken token = default);
    }

    public interface IDarkstatClient : IMarkedGoodQueryClient, INpcBaseQueryClient, IOreFieldQueryClient, IPlayerBaseQueryClient, IRouteQueryClient, ICommodityQueryClient, IShipInfoQueryClient
    {

    }
}
