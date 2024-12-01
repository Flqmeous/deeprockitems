﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeUIPlayer : ModPlayer
    {
        public static Point UpgradeStationLocation;
        public override void SetStaticDefaults() {
            _upgradeSystem = ModContent.GetInstance<UpgradeSystem>();
        }
        public void CloseUI() {
            var system = ModContent.GetInstance<UpgradeSystem>();
            UpgradeSystem.SetState(null);
            // Give item in slot the player
            if (system.UpgradeUIState.Panel.ParentSlot.ItemInSlot != null && system.UpgradeUIState.Panel.ParentSlot.ItemInSlot.type != 0)
            {
                Player.QuickSpawnItem(system.UpgradeUIState.Panel.ParentSlot.ItemInSlot.GetSource_ReleaseEntity(), system.UpgradeUIState.Panel.ParentSlot.ItemInSlot);
            }
            system.UpgradeUIState.Panel.ParentSlot.ItemInSlot = new(0);
            system.UpgradeUIState.Panel.UpgradeContainer.SetUpgrades(null);
        }
        public override void ResetEffects() {
            if (UpgradeStationLocation != new Point(-1, -1) && (!Main.LocalPlayer.IsInTileInteractionRange(UpgradeStationLocation.X, UpgradeStationLocation.Y, TileReachCheckSettings.Simple) || Player.chest != -1 || !Main.playerInventory || Player.talkNPC != -1)) {
                CloseUI();
                UpgradeStationLocation = new Point(-1, -1);
            }
        }
        public override void OnEnterWorld() {
            // Give item to player
            if (ItemToSpawnOnWorldLoad != null && ItemToSpawnOnWorldLoad.type != ItemID.None)
            {
                Player.QuickSpawnItem(ItemToSpawnOnWorldLoad.GetSource_ReleaseEntity(), ItemToSpawnOnWorldLoad);
            }
        }
        /*public override void SaveData(TagCompound tag) {
            tag.Add("UpgradeSlotItem", ItemInSlot);
            ItemInSlot = new Item(0);
            _upgradeSystem.UpgradeUIState.Panel.UpgradeContainer.SetUpgrades(null);
        }
        public override void LoadData(TagCompound tag) {
            if (tag.ContainsKey("UpgradeSlotItem") && tag.Get<Item>("UpgradeSlotItem") != null)
            {
                var itemInSlot = tag.Get<Item>("UpgradeSlotItem");
                // Prepare to spawn
                ItemToSpawnOnWorldLoad = itemInSlot;
                // Remove key
                tag.Remove("UpgradeSlotItem");
            }
        }*/
        private static UpgradeSystem _upgradeSystem;
        public Item ItemToSpawnOnWorldLoad = new(0);
        public Item ItemInSlot { get => _upgradeSystem.UpgradeUIState.Panel.ParentSlot.ItemInSlot; set => _upgradeSystem.UpgradeUIState.Panel.ParentSlot.ItemInSlot = value; }
    }
}
