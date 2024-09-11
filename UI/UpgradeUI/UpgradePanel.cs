using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using deeprockitems.Content.Items;
using deeprockitems.Content.Items.Weapons;
using Terraria.GameContent.UI.Elements;
using System;
using Terraria.ModLoader.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradePanel : UIPanel
    {
        // The UI will have 1 item slot.
        public FakeItemSlot ParentSlot;

        // The UI will have 1 upgrade selection container.
        public UpgradeSelectionContainer UpgradeContainer;
        
        // The UI will have a forge button to facilitate crafting the upgrade
        public UIButton<string> ForgeButton;
        public override void OnInitialize()
        {
            // Set sizes of objects and initialize elements
            float MARGIN = 4;
            float PADDING = 10;
            SetPadding(PADDING);

            ParentSlot = new FakeItemSlot((mouseItem, slotItem) =>
            {
                if (mouseItem.ModItem is IUpgradable)
                {
                    return true;
                }
                else if (slotItem.type != 0 && (mouseItem.type == 0 || mouseItem.ModItem is IUpgradable))
                {
                    return true;
                }
                return false;
            });
            ParentSlot.OnItemSwap += ParentSlot_OnItemSwap;
            ForgeButton = new("Forge")
            {
                ScalePanel = true
            };


            // Set size and position of parent slot
            ParentSlot.HAlign = 0f;
            ParentSlot.Width.Pixels = ParentSlot.Height.Pixels = 52;

            // Set size and position of button
            ForgeButton.HAlign = 1f;
            ForgeButton.OnLeftClick += ForgeButton_OnLeftClick;

            // Set size of upgrade panel
            UpgradeContainer = new(Width.Pixels, Height.Pixels - ParentSlot.Height.Pixels);
            UpgradeContainer.Top.Pixels = ParentSlot.Height.Pixels;

            // Append everything
            Append(ParentSlot);
            Append(ForgeButton);
            Append(UpgradeContainer);
        }

        private void ParentSlot_OnItemSwap(Item itemNowInSlot, Item itemThatLeftSlot)
        {
            if (itemNowInSlot.ModItem is IUpgradable modItem)
            {
                // Set upgrades
                UpgradeContainer.SetUpgrades(modItem.UpgradeMasterList);
            }
            else
            {
                UpgradeContainer.SetUpgrades(null);
            }
        }

        private void ForgeButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.NewText("Forging!");
        }
    }
}
