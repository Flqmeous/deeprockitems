using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    /// <summary>
    /// This is each individual upgrade represented as UI.
    /// </summary>
    public class UpgradeSelectOption : UIElement
    {
        private UpgradeTier _upgrades;
        public UpgradeSelectOption(UpgradeTier upgrades, Upgrade upgrade)
        {
            Upgrade = upgrade;
            _upgrades = upgrades;
            if (Upgrade.IsEquipped)
            {
                DrawColor = Color.Yellow;
            }
            else
            {
                DrawColor = Color.White;
            }
            OnLeftClick += UpgradeSelectOption_OnLeftClick;
        }

        private void UpgradeSelectOption_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            foreach (var upgrade in _upgrades)
            {
                if (Upgrade.InternalName != upgrade.InternalName)
                {
                    upgrade.IsEquipped = false;
                }
            }
            Upgrade.IsEquipped = !Upgrade.IsEquipped;
            (ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.Panel.ParentSlot.ItemInSlot.ModItem as UpgradeableItemTemplate).VerifyUpgrades();
            if (Upgrade.IsEquipped)
            {
                DrawColor = Color.Yellow;
            }
            else
            {
                DrawColor = Color.White;
            }
        }

        /// <summary>
        /// The upgrade that is represented by this UIElement.
        /// </summary>
        public Upgrade Upgrade;
        public Color DrawColor { get; set; } = Color.White;
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Show hover text if the slot is being hovered
            if (IsMouseHovering)
            {
                Main.hoverItemName = $"[c/E3B465:{Upgrade.DisplayName}]" + "\n" +
                                     $"{Upgrade.HoverText}";
            }

            var dimensions = GetDimensions().ToRectangle();
            // Draw slot
            spriteBatch.Draw(TextureAssets.InventoryBack9.Value, dimensions, DrawColor);

            // Draw icon
            float scale = 0.7f;
            Rectangle destination = new((int)(dimensions.Center.X - dimensions.Width * 0.5f * scale), (int)(dimensions.Center.Y - dimensions.Height * 0.5f * scale), (int)(dimensions.Width * scale), (int)(dimensions.Height * scale));
            spriteBatch.Draw(Upgrade.Texture, destination, Color.White);

        }
    }
}
