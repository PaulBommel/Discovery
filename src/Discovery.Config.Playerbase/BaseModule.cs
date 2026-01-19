using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Config.Playerbase
{
    [DebuggerDisplay($"{{{nameof(InfoText)}}}")]
    public class BaseModule
    {
        public string Nickname { get; init; }
        public string InfoText { get; init; }
        public ImmutableArray<string> CraftLists { get; init; }

        public BaseModule(BaseModuleRecipe recipe)
        {
            if(recipe is null)
                throw new ArgumentNullException(nameof(recipe));

            Nickname = recipe.Nickname;
            InfoText = recipe.InfoText;
            CraftLists = recipe.CraftLists;
        }
    }
}
