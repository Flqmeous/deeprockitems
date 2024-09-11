using Terraria.ModLoader;
using Terraria.UI;
using System.Linq;
using deeprockitems.Content.Items;
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
            OnLeftClick += UpgradeSelectionContainer_OnLeftClick;
        }

        private void UpgradeSelectionContainer_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
            // Determine which upgrade was clicked:
            if (evt.Target is not UpgradeSelectOption option) return;

            // De-equip all other upgrades in this tier
            if (option.Parent is not UpgradeSelectionTier tier) return;

            foreach (var upgrade in tier.Options)
            {
                // Keep this state the same
                if (option == upgrade) continue;

                // De equip others
                upgrade.Upgrade.IsEquipped = false;
                upgrade.VerifyDrawColor();
            }

            // Change state of this option element
            option.Upgrade.IsEquipped = !option.Upgrade.IsEquipped;
            option.VerifyDrawColor();
            // Verify stat changes
            (ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.Panel.ParentSlot.ItemInSlot.ModItem as IUpgradable).VerifyUpgrades();
        }

        public void SetUpgrades(UpgradeList upgrades)
        {
            // If the upgrades are not set, the parent item was removed. Undo everything.
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
