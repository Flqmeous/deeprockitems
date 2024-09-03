using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using System.Linq;
using Terraria.GameContent;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Upgrades;

namespace deeprockitems.UI.UpgradeUI
{
    /// <summary>
    /// This is the entire panel that contains each tier of upgrade
    /// </summary>
    public class UpgradeSelectionContainer : UIElement
    {
        UpgradeSelectionTier[] upgradeSelectors;
        public UpgradeSelectionContainer(float width, float height)
        {
            Width.Pixels = width;
            Height.Pixels = height;
        }
        public void SetUpgrades(UpgradeList upgrades)
        {
            if (upgrades is null)
            {
                upgradeSelectors = [];
                RemoveAllChildren();
                return;
            }
            upgradeSelectors = upgrades.Select(tiers => new UpgradeSelectionTier(tiers, (int)Height.Pixels)).ToArray();
            const int GAP = 4;
            int computedWidth = (int)((Width.Pixels - upgradeSelectors.Length * GAP) / (float)upgradeSelectors.Length);
            for (int i = 0; i < upgradeSelectors.Length; i++)
            {
                upgradeSelectors[i].Left.Pixels = i * (GAP + computedWidth);
                upgradeSelectors[i].Width.Pixels = computedWidth;
                upgradeSelectors[i].Height.Pixels = Height.Pixels;

                // Append
                Append(upgradeSelectors[i]);
            }
        }
    }
}
