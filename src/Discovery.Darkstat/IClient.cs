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
        Task<MarketGoodResponse[]> GetMarketGoodsPerBasesAsync(string[] baseNicknames, CancellationToken token = default);
        Task<MarketGoodResponse[]> GetCommoditiesPerNicknameAsync(string[] commodityNicknames, CancellationToken token = default);
    }

    public interface INpcBaseQueryClient
    {
        Task<NpcBase[]> GetNpcBasesAsync(CancellationToken token = default);
    }

    public interface IMiningZoneQueryClient
    {
        Task<MiningZone[]> GetMiningZonesAsync(CancellationToken token = default);
    }

    public interface IPlayerBaseQueryClient
    {
        Task<PlayerBase[]> GetPlayerBasesAsync(CancellationToken token = default);
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

    public interface IDarkstatClient : IMarkedGoodQueryClient, INpcBaseQueryClient, IMiningZoneQueryClient, IPlayerBaseQueryClient, IRouteQueryClient, ICommodityQueryClient, IShipInfoQueryClient
    {

    }
}
