using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Config.Test
{
    internal static class TestExtensions
    {
        public static async Task AssertAsyncSequencesEqual<T>(this Assert assert,
                                                              IAsyncEnumerable<T> expected,
                                                              IAsyncEnumerable<T> actual,
                                                              Action<Assert, T, T> AreEqual,
                                                              CancellationToken token = default)
        {
            if(expected is null)
                throw new ArgumentNullException(nameof(expected));
            if (actual is null)
                throw new ArgumentNullException(nameof(actual));
            if (AreEqual is null)
                throw new ArgumentNullException(nameof(AreEqual));

            await using var expectedEnumerator = expected.GetAsyncEnumerator();
            await using var actualEnumerator = actual.GetAsyncEnumerator();

            bool canExpectedMoveNext = false, canActualMoveNext = false;

            do
            {
                canExpectedMoveNext = await expectedEnumerator.MoveNextAsync();
                canActualMoveNext = await actualEnumerator.MoveNextAsync();

                if (!canExpectedMoveNext && !canActualMoveNext)
                    break;

                if (canExpectedMoveNext ^ canActualMoveNext)
                    Assert.Fail("Sequences have different lengths.");

                AreEqual(assert, expectedEnumerator.Current, actualEnumerator.Current);
            } while (!token.IsCancellationRequested);

        }

        public static void AreEqual(this Assert _,
                                    BaseModuleRecipe expected,
                                    BaseModuleRecipe actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.BuildType, actual.BuildType);
            Assert.AreEqual(expected.CargoStorage, actual.CargoStorage);
            Assert.AreEqual(expected.CookingRate, actual.CookingRate);

            Assert.HasCount(expected.CraftLists.Length, actual.CraftLists);
            for (var i = 0; i < expected.CraftLists.Length; ++i)
            {
                Assert.AreEqual(expected.CraftLists[i], actual.CraftLists[i]);
            }

            Assert.AreEqual(expected.CreditCost, actual.CreditCost);
            Assert.AreEqual(expected.InfoText, actual.InfoText);

            Assert.HasCount(expected.Inputs.Length, actual.Inputs);
            for (var i = 0; i < expected.Inputs.Length; ++i)
            {
                Assert.AreEqual(expected.Inputs[i], actual.Inputs[i]);
            }
            Assert.AreEqual(expected.ModuleClass, actual.ModuleClass);
            Assert.AreEqual(expected.Nickname, actual.Nickname);
            Assert.AreEqual(expected.ProducedItem, actual.ProducedItem);
            Assert.AreEqual(expected.RecipeNumber, actual.RecipeNumber);
            Assert.AreEqual(expected.RequiredLevel, actual.RequiredLevel);
        }

        public static void AreEqual(this Assert _,
                                    BaseItemRecipe expected,
                                    BaseItemRecipe actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            Assert.HasCount(expected.AffiliationBonuses.Count, actual.AffiliationBonuses);
            for (var i = 0; i < expected.AffiliationBonuses.Count; ++i)
            {
                var expectedKey = expected.AffiliationBonuses.Keys.ElementAt(i);
                var actualKey = actual.AffiliationBonuses.Keys.ElementAt(i);
                Assert.AreEqual(expectedKey, actualKey);
                Assert.AreEqual(expected.AffiliationBonuses[expectedKey], actual.AffiliationBonuses[actualKey]);
            }

            Assert.AreEqual(expected.CookingRate, actual.CookingRate);
            Assert.AreEqual(expected.CraftType, actual.CraftType);
            Assert.AreEqual(expected.InfoText, actual.InfoText);

            Assert.HasCount(expected.Inputs.Length, actual.Inputs);
            for (var i = 0; i < expected.Inputs.Length; ++i)
            {
                Assert.AreEqual(expected.Inputs[i], actual.Inputs[i]);
            }

            Assert.AreEqual(expected.Nickname, actual.Nickname);

            Assert.HasCount(expected.Outputs.Length, actual.Outputs);
            for (var i = 0; i < expected.Outputs.Length; ++i)
            {
                Assert.AreEqual(expected.Outputs[i], actual.Outputs[i]);
            }

            Assert.AreEqual(expected.RequiredLevel, actual.RequiredLevel);
            Assert.AreEqual(expected.Restricted, actual.Restricted);
            Assert.AreEqual(expected.ShortcutNumber, actual.ShortcutNumber);
        }
    }
}
