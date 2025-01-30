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
        Task<RouteData[]> GetRoutesAsync(CancellationToken token = default);
    }

    public interface ICommodityQueryClient
    {
        Task<Commodity[]> GetCommoditiesAsync(CancellationToken token = default);
    }

    public interface IShipInfoQueryClient
    {
        Task<ShipInfo[]> GetShipInfoAsync(CancellationToken token = default);
    }

    public interface IDarkstatClient : INpcBaseQueryClient, IOreFieldQueryClient, IPlayerBaseQueryClient, IRouteQueryClient, ICommodityQueryClient, IShipInfoQueryClient
    {

    }
}
