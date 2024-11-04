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
