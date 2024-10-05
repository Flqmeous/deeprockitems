using deeprockitems.Content.Upgrades;
using System.Linq;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    /// <summary>
    /// This is the indivudal tier of each upgrade.
    /// </summary>
    public class UpgradeSelectionTier : UIElement
    {
        public UpgradeSelectionTier(UpgradeTier upgrades, int heightToWorkWith)
        {
            Options = upgrades.Select(upgrade => new UpgradeSelectOption(upgrades, upgrade)).ToArray();
            // Space out elements to fit in the height allowed.
            const int PADDING = 2;
            int computedHeight = (int)((heightToWorkWith - upgrades.Length * PADDING) / 3.5f);
            for (int i = 0; i < upgrades.Length; i++)
            {
                Options[i].Top.Pixels = (i + 1) * PADDING + i * computedHeight;
                Options[i].Width.Pixels = Options[i].Height.Pixels = computedHeight;
                Append(Options[i]);
            }
        }
        public UpgradeSelectOption[] Options;
    }
}
