using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace deeprockitems.Common
{
    /// <summary>
    /// This class exists solely to load some important assets.
    /// </summary>
    public class AssetLoadFix : ModSystem
    {
        public override void SetStaticDefaults() {
            _ = Assets.UI.SelectButton.Value;
        }
    }
}
