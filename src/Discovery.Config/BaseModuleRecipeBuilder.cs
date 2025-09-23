using System;
using System.Collections.Generic;

namespace Discovery.Config
{
    internal class BaseModuleRecipeBuilder : AbstractBaseRecipeBuilder
    {
        public string Nickname { get; set; }
        public string InfoText { get; set; }
        public string BuildType { get; set; }
        public int RecipeNumber { get; set; }
        public int ModuleClass { get; set; }
        public int CookingRate { get; set; }
        public int RequiredLevel { get; set; }
        public string ProducedItem { get; set; }
        public int CreditCost { get; set; }
        public int CargoStorage { get; set; }
        public List<string> CraftLists { get; } = [];

        public BaseModuleRecipe Build() => new()
        {
            Nickname = Nickname,
            InfoText = InfoText,
            BuildType = BuildType,
            RecipeNumber = RecipeNumber,
            ModuleClass = ModuleClass,
            CookingRate = CookingRate,
            RequiredLevel = RequiredLevel,
            ProducedItem = ProducedItem,
            Inputs = [.. Inputs],
            CreditCost = CreditCost,
            CargoStorage = CargoStorage,
            CraftLists = [.. CraftLists]
        };
    }
}
