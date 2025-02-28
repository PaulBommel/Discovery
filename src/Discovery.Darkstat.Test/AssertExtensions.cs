using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Discovery.Darkstat.Test
{
    using Darkstat.NpcQueryClient;
    using Darkstat.OreFieldQueryClient;
    using Darkstat.PobQueryClient;

    using Discovery.Darkstat;

    using System.Diagnostics.CodeAnalysis;

    internal static class AssertExtensions
    {
        public static void HasPlausibleData(this Assert _, ILocation pointOfInterest)
        {
            Assert.IsNotNull(pointOfInterest);
            Assert.IsFalse(string.IsNullOrWhiteSpace(pointOfInterest.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(pointOfInterest.Nickname), $"The property '{nameof(ILocation.Nickname)}' for {pointOfInterest.Name} is empty.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(pointOfInterest.FactionNickname), $"The property '{nameof(ILocation.FactionNickname)}' for {pointOfInterest.Name} is empty.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(pointOfInterest.RegionName), $"The property '{nameof(ILocation.RegionName)}' for {pointOfInterest.Name} is empty.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(pointOfInterest.SystemName), $"The property '{nameof(ILocation.SystemName)}' for {pointOfInterest.Name} is empty.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(pointOfInterest.SystemNickname), $"The property '{nameof(ILocation.SystemNickname)}' for {pointOfInterest.Name} is empty.");
        }

        public static void AreEqual(this Assert _, NpcBase expected, NpcBase actual)
        {
            Assert.AreEqual(expected.Archetypes?.Length ?? 0, actual.Archetypes?.Length ?? 0);
            if (expected.Archetypes is not null)
                for (int i = 0; i < expected.Archetypes.Length; ++i)
                    Assert.AreEqual(expected.Archetypes[i], actual.Archetypes[i]);
            Assert.AreEqual(expected.BgcsBaseRunBy, actual.BgcsBaseRunBy);
            Assert.AreEqual(expected.FactionNickname, actual.FactionNickname);
            Assert.AreEqual(expected.File, actual.File);
            Assert.AreEqual(expected.InfocardId, actual.InfocardId);
            Assert.AreEqual(expected.InfocardKey, actual.InfocardKey);
            Assert.AreEqual(expected.IsReachhable, actual.IsReachhable);
            Assert.AreEqual(expected.IsTransportUnreachable, actual.IsTransportUnreachable);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Nickname, actual.Nickname);
            Assert.AreEqual(expected.Position, actual.Position);
            Assert.AreEqual(expected.RegionName, actual.RegionName);
            Assert.AreEqual(expected.SectorCoord, actual.SectorCoord);
            Assert.AreEqual(expected.StridName, actual.StridName);
            Assert.AreEqual(expected.SystemName, actual.SystemName);
            Assert.AreEqual(expected.SystemNickname, actual.SystemNickname);
        }

        public static void AreEqual(this Assert _, MiningZone expected, MiningZone actual)
        {
            Assert.AreEqual(expected.Archetypes?.Length ?? 0, actual.Archetypes?.Length ?? 0);
            if (expected.Archetypes is not null)
                for (int i = 0; i < expected.Archetypes.Length; ++i)
                    Assert.AreEqual(expected.Archetypes[i], actual.Archetypes[i]);
            Assert.AreEqual(expected.BgcsBaseRunBy, actual.BgcsBaseRunBy);
            Assert.AreEqual(expected.FactionNickname, actual.FactionNickname);
            Assert.AreEqual(expected.File, actual.File);
            Assert.AreEqual(expected.InfocardId, actual.InfocardId);
            Assert.AreEqual(expected.InfocardKey, actual.InfocardKey);
            Assert.AreEqual(expected.IsReachhable, actual.IsReachhable);
            Assert.AreEqual(expected.IsTransportUnreachable, actual.IsTransportUnreachable);
            Assert.That.AreEqual(expected.MiningInfo, actual.MiningInfo);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Nickname, actual.Nickname);
            Assert.AreEqual(expected.Position, actual.Position);
            Assert.AreEqual(expected.RegionName, actual.RegionName);
            Assert.AreEqual(expected.SectorCoord, actual.SectorCoord);
            Assert.AreEqual(expected.StridName, actual.StridName);
            Assert.AreEqual(expected.SystemName, actual.SystemName);
            Assert.AreEqual(expected.SystemNickname, actual.SystemNickname);
        }

        public static void AreEqual(this Assert _, MiningInfo expected, MiningInfo actual)
        {
            Assert.AreEqual(expected.DynamicLootDifficulty, actual.DynamicLootDifficulty);
            Assert.AreEqual(expected.DynamicLootMax, actual.DynamicLootMax);
            Assert.AreEqual(expected.DynamicLootMin, actual.DynamicLootMin);
            Assert.That.AreEqual(expected.MinedGood, actual.MinedGood);
            Assert.AreEqual(expected.DynamicLootDifficulty, actual.DynamicLootDifficulty);
            Assert.AreEqual(expected.DynamicLootDifficulty, actual.DynamicLootDifficulty);
        }

        public static void AreEqual(this Assert _, MinedGood expected, MinedGood actual)
        {
            Assert.AreEqual(expected.BaseSells, actual.BaseSells);
            Assert.AreEqual(expected.HpType, actual.HpType);
            Assert.AreEqual(expected.Infocard, actual.Infocard);
            Assert.AreEqual(expected.IsServerSideOverride, actual.IsServerSideOverride);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Nickname, actual.Nickname);
            Assert.AreEqual(expected.PriceBase, actual.PriceBase);
            Assert.AreEqual(expected.PriceModifier, actual.PriceModifier);
            Assert.AreEqual(expected.PriceToBuy, actual.PriceToBuy);
            Assert.AreEqual(expected.PriceToSell, actual.PriceToSell);
            Assert.AreEqual(expected.RepRequired, actual.RepRequired);
            Assert.AreEqual(expected.ShipClass, actual.ShipClass);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.Volume, actual.Volume);
        }

        public static void AreEqual(this Assert _, PlayerBase expected, PlayerBase actual)
        {
            Assert.AreEqual(expected.DefenseMode, actual.DefenseMode);
            Assert.AreEqual(expected.FactionName, actual.FactionName);
            Assert.AreEqual(expected.FactionNickname, actual.FactionNickname);
            Assert.AreEqual(expected.ForumThreadUrl, actual.ForumThreadUrl);
            Assert.AreEqual(expected.Health, actual.Health);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Money, actual.Money);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Nickname, actual.Nickname);
            Assert.AreEqual(expected.Pos, actual.Pos);
            Assert.AreEqual(expected.Position, actual.Position);
            Assert.AreEqual(expected.RegionName, actual.RegionName);
            Assert.AreEqual(expected.SectorCoord, actual.SectorCoord);
            Assert.AreEqual(expected.ShopItems?.Length ?? 0, actual.ShopItems?.Length ?? 0);
            if (expected.ShopItems is not null)
                for (int i = 0; i < expected.ShopItems.Length; ++i)
                    Assert.That.AreEqual(expected.ShopItems[i], actual.ShopItems[i]);
            Assert.AreEqual(expected.SystemName, actual.SystemName);
            Assert.AreEqual(expected.SystemNickname, actual.SystemNickname);
        }

        public static void AreEqual(this Assert _, ShopItem expected, ShopItem actual)
        {
            Assert.AreEqual(expected.Category, actual.Category);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.MaxStock, actual.MaxStock);
            Assert.AreEqual(expected.MinStock, actual.MinStock);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Nickname, actual.Nickname);
            Assert.AreEqual(expected.Price, actual.Price);
            Assert.AreEqual(expected.Quantity, actual.Quantity);
            Assert.AreEqual(expected.SellPrice, actual.SellPrice);
        }

        public static void AreEqual(this Assert _, MarketGood expected, MarketGood actual)
        {
            Assert.AreEqual(expected.BaseName, actual.BaseName);
            Assert.AreEqual(expected.BaseNickname, actual.BaseNickname);
            Assert.AreEqual(expected.BaseSells, actual.BaseSells);
            Assert.AreEqual(expected.Category, actual.Category);
            Assert.AreEqual(expected.FactionName, actual.FactionName);
            Assert.AreEqual(expected.Nickname, actual.Nickname);
            Assert.AreEqual(expected.HpType, actual.HpType);
            Assert.AreEqual(expected.IsServerOverride, actual.IsServerOverride);
            Assert.AreEqual(expected.LevelRequired, actual.LevelRequired);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Nickname, actual.Nickname);
            Assert.AreEqual(expected.Position, actual.Position);
            Assert.AreEqual(expected.PriceBase, actual.PriceBase);
            Assert.AreEqual(expected.PriceBaseBuysFor, actual.PriceBaseBuysFor);
            Assert.AreEqual(expected.PriceBaseSellsFor, actual.PriceBaseSellsFor);
            Assert.AreEqual(expected.RegionName, actual.RegionName);
            Assert.AreEqual(expected.RepRequired, actual.RepRequired);
            Assert.AreEqual(expected.SectorCoord, actual.SectorCoord);
            Assert.AreEqual(expected.ShipClass, actual.ShipClass);
            Assert.AreEqual(expected.ShipNickname, actual.ShipNickname);
            Assert.AreEqual(expected.SystemName, actual.SystemName);
            Assert.AreEqual(expected.Volume, actual.Volume);
        }
    }
}
