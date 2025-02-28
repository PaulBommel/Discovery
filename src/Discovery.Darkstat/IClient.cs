using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat
{
    public interface IMarkedGoodQueryClient
    {
        Task<MarketGoodResponse[]> GetMarketGoodsPerBasesAsync(string[] baseNicknames, CancellationToken token = default);
        Task<MarketGoodResponse[]> GetCommoditiesPerNicknameAsync(string[] commodityNicknames, CancellationToken token = default);
    }

    public interface INpcBaseQueryClient
    {
        [Obsolete]
        Task<NpcBase[]> GetNpcBasesAsync(CancellationToken token = default);
        Task<NpcBase[]> GetNpcBasesAsync(BaseQueryParameter parameter, CancellationToken token = default);
    }

    public interface IMiningZoneQueryClient
    {
        [Obsolete]
        Task<MiningZone[]> GetMiningZonesAsync(CancellationToken token = default);
        Task<MiningZone[]> GetMiningZonesAsync(BaseQueryParameter parameter, CancellationToken token = default);
    }

    public interface IPlayerBaseQueryClient
    {
        Task<PlayerBase[]> GetPlayerBasesAsync(CancellationToken token = default);
    }

    public interface IRouteQueryClient
    {
        Task<RouteResponse[]> GetTimingsAsync(Route[] routes, CancellationToken token = default);
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
