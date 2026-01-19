using Discovery.Darkstat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Discovery.Config.Playerbase.UI.ViewModels
{
    using Commands;
    using PlayerBase = Playerbase.PlayerBase;

    public class PlayerBaseViewModel : AbstractViewModel
    {

        private readonly BaseItemRecipeClient _itemClient;
        private readonly BaseModuleRecipeClient _moduleClient;
        private readonly DarkstatClient _darkstatClient;

        public PlayerBaseViewModel()
        {
            UpdateCommand = new RelayCommand<PlayerBase>(async (playerBase) => await Update(playerBase));
            AddModuleCommand = new RelayCommand<BaseModuleRecipe>(AddModule);
            RemoveModuleCommand = new RelayCommand<BaseModule>(module => PlayerBase.Modules.Remove(module));
        }

        
        public PlayerBaseViewModel(BaseItemRecipeClient itemClient,
                                   BaseModuleRecipeClient moduleClient,
                                   DarkstatClient darkstatClient)
            :this()
        {
            _itemClient = itemClient;
            _moduleClient = moduleClient;
            _darkstatClient = darkstatClient;
        }

        public PlayerBase PlayerBase { get; } = new();
        public BaseModuleRecipe[] Recipes { get; set; }
        
        public ICommand UpdateCommand { get; }

        public ICommand AddModuleCommand { get; }
        public ICommand RemoveModuleCommand { get; }

        private async Task Update(PlayerBase playerBase)
        {
            var bases = await _darkstatClient.GetPlayerBasesAsync();

            var lookup = bases.SingleOrDefault(b => b.Name == playerBase.Name);
            if (lookup is not null)
            {
                playerBase.Name = lookup.Name;
                playerBase.Affiliation = lookup.FactionName;
                playerBase.DefenseMode = lookup.DefenseMode;
                playerBase.Level = (int)lookup.Level;
                playerBase.Health = lookup.Health;
                playerBase.Money = lookup.Money;
                playerBase.Nickname = lookup.Nickname;
                playerBase.Pos = lookup.Pos;
                playerBase.Position = lookup.Position;
                playerBase.ShopItems = lookup.ShopItems;
                playerBase.SystemName = lookup.SystemName;
                playerBase.SystemNickname = lookup.SystemNickname;
            }

            var recipes = await _moduleClient.GetRecipesAsync();
            Recipes = [.. recipes];
        }

        private void AddModule(BaseModuleRecipe recipe)
        {
            if(recipe is null)
                throw new ArgumentNullException(nameof(recipe));

            PlayerBase.Modules.Add(new(recipe));
        }
    }
}
