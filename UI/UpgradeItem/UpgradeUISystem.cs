﻿using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using deeprockitems.Content.Items.Weapons;

namespace deeprockitems.UI.UpgradeItem
{
    public class UpgradeUISystem : ModSystem
    {
        public static UpgradeUIState UpgradeUIPanel { get; set; }
        public static UserInterface Interface { get; set; }
        public static UpgradeableItemTemplate TheParentItem { get => UpgradeUIPanel.ParentSlot.ItemToDisplay.ModItem as UpgradeableItemTemplate; }

        internal static bool BlockItemSlotActionsDetour { get; set; }


        public override void Load()
        {
            UpgradeUIPanel = new();
            UpgradeUIPanel.Activate();
            Interface = new();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            Interface?.Update(gameTime);
            if (!Main.playerInventory)
            {
                Interface.SetState(null);
            }
            if (UpgradeUIPanel.ParentSlot.ItemToDisplay.type == 0)
            {
                Interface.SetState(null);
            }

            InterfaceBlocker.BlockItemSlotLogic = false; // Reset blocking logic
            if (UpgradeUIPanel.dragPanel.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
                InterfaceBlocker.BlockItemSlotLogic = true;
            }


        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "YourMod: A Description",
                    delegate
                    {
                        Interface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}