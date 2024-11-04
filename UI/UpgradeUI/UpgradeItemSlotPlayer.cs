using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeItemSlotPlayer : ModPlayer
    {
        public override void SetStaticDefaults() {
            _upgradeSystem = ModContent.GetInstance<UpgradeSystem>();
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
