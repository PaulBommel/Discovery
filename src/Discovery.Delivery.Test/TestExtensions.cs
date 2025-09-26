using Discovery.Darkstat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Delivery.Test
{
    public static class TestExtensions
    {
        public static void AreEqual(this Assert _, DeliveryRecord expected, DeliveryRecord actual)
        {
            Assert.AreEqual(expected.Cargo.Length, actual.Cargo.Length);
            for(int i = 0; i < expected.Cargo.Length; ++i)
            {
                Assert.AreEqual(expected.Cargo[i].Amount, actual.Cargo[i].Amount);
                Assert.AreEqual(expected.Cargo[i].Commodity, actual.Cargo[i].Commodity);
            }
            Assert.AreEqual(expected.Credits, actual.Credits);
            Assert.AreEqual(expected.Destination, actual.Destination);
            Assert.AreEqual(expected.ShipType, actual.ShipType);
            //Assert.AreEqual(expected.Source, actual.Source);
            Assert.AreEqual(expected.SourceType, actual.SourceType);
            Assert.AreEqual(expected.Time, actual.Time);
        }

        internal static async Task<IEnumerable<ILocation>> GetLocations(this IDarkstatClient client, CancellationToken token = default)
        {
            var npcTask = client.GetNpcBasesAsync(new() { NicknameFilter = null }, token);
            var pobTask = client.GetPlayerBasesAsync(token);
            var oreTask = client.GetMiningZonesAsync(new() { NicknameFilter = null }, token);
            await Task.WhenAll(npcTask, pobTask, oreTask);
            var npcs = npcTask.Result.Cast<ILocation>();
            var pobs = pobTask.Result.Cast<ILocation>();
            var zones = oreTask.Result.Cast<ILocation>();
            return npcs.Union(pobs).Union(zones);
        }
    }
}
