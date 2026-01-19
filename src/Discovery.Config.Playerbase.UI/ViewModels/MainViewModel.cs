using Discovery.Darkstat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Discovery.Config.Playerbase.UI.ViewModels
{
    public class MainViewModel : AbstractViewModel
    {

        #region Constructors

        public MainViewModel()
        {
            PlayerBase = new();
        }

        public MainViewModel(BaseModuleRecipeClient client, DarkstatClient darkstat)
        {
            PlayerBase = new();
        }

        #endregion

        public PlayerBaseViewModel PlayerBase { get; }

    }
}
