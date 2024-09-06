﻿using deeprockitems.UI.UpgradeUI;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI
{
    public class ShiftClickIntoFakeSlot : ModPlayer
    {
        UpgradeSystem upgradeSystem;
        bool canShiftIn = false;
        public override void Initialize()
        {
            upgradeSystem = ModContent.GetInstance<UpgradeSystem>();
        }
        public override void PreUpdate()
        {
            canShiftIn = false;
        }
        public override bool HoverSlot(Item[] inventory, int context, int slot)
        {
            if (inventory[slot].type == 0) return false;
            if (ItemSlot.ShiftInUse && !ItemSlot.ShiftForcedOn && upgradeSystem.Interface.CurrentState != null && upgradeSystem.UpgradeUIState.Panel.ParentSlot.CanItemBePutIn(inventory[slot], upgradeSystem.UpgradeUIState.Panel.ParentSlot.ItemInSlot))
            {
                Main.cursorOverride = 9;
                canShiftIn = true;
                return true;
            }
            return false;
        }
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            if (canShiftIn)
            {
                upgradeSystem.UpgradeUIState.Panel.ParentSlot.SwapItems(ref inventory[slot], ref upgradeSystem.UpgradeUIState.Panel.ParentSlot.ItemInSlot);
                return true;
            }
            return false;
        }
    }
}
