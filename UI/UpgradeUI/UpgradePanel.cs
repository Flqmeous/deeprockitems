using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using deeprockitems.Content.Items;
using deeprockitems.Content.Items.Weapons;
using Terraria.GameContent.UI.Elements;
using System;
using Terraria.ModLoader.UI;
using deeprockitems.Content.Upgrades;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

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
            RecipeDisplay = new UpgradeRecipeDisplay();


            // Set size and position of parent slot
            ParentSlot.HAlign = 0f;
            ParentSlot.Width.Pixels = ParentSlot.Height.Pixels = 52;

            // Set size and position of button
            ForgeButton.Height = ParentSlot.Height;
            ForgeButton.HAlign = 1f;
            ForgeButton.OnLeftClick += ForgeButton_OnLeftClick;

            // Set size of upgrade panel
            UpgradeContainer = new(Width.Pixels, Height.Pixels - ParentSlot.Height.Pixels);
            UpgradeContainer.Top.Pixels = ParentSlot.Height.Pixels;
            UpgradeContainer.OnLeftClick += UpgradeContainer_OnLeftClick;

            // Append everything
            Append(ParentSlot);
            Append(ForgeButton);
            Append(UpgradeContainer);

            // Set recipe display position
            RecipeDisplay.Top.Pixels = ParentSlot.Top.Pixels;
            RecipeDisplay.Width.Pixels = Width.Pixels - ForgeButton.Width.Pixels - ParentSlot.Width.Pixels - 4 * PADDING;
            RecipeDisplay.Left.Pixels = ParentSlot.Left.Pixels + ParentSlot.Width.Pixels + PADDING;
            RecipeDisplay.Height.Pixels = ParentSlot.Height.Pixels;
            RecipeDisplay.SetState(null);
            Append(RecipeDisplay);
        }
        /// <summary>
        /// Recipe display for whether an upgrade can be "bought" or not.
        /// </summary>
        public UpgradeRecipeDisplay RecipeDisplay { get; set; }
        /// <summary>
        /// Selects the locked upgrade for crafting. Will not attempt to unlock the upgrade; refer to <see cref="UpgradePanel.UnlockUpgrade()"/>
        /// </summary>
        private void SelectThisLockedUpgrade(UpgradeSelectOption option) {
            RecipeDisplay.SetState(option);
        }

        private void UpgradeContainer_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
            // Determine which upgrade was clicked:
            if (evt.Target is not UpgradeSelectOption option) return;

            // DEBUG: allow any upgrade to be equipped
            if (deeprockitems.DebugMode)
            {
                SelectThisLockedUpgrade(option);
                option.SelectThisUpgrade();
                return;
            }

            if (!option.Upgrade.UpgradeState.IsUnlocked)
            {
                SelectThisLockedUpgrade(option);
                return;
            }

            // Select this upgrade
            option.SelectThisUpgrade();
        }
        private void ParentSlot_OnItemSwap(Item itemNowInSlot, Item itemThatLeftSlot)
        {
            // Remove currently selected locked upgrade
            RecipeDisplay.SetState(null);

            // Set upgrade state
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
            // Check if a current recipe is put in
            if (RecipeDisplay.Option is null || RecipeDisplay.Option.Upgrade is null) return;

            // Check if the ingredients of the recipe are in the player's inventory
            if (!TryToCraftItem(Main.LocalPlayer, RecipeDisplay.Option.Upgrade.Recipe)) return;

            // Unlock the upgrade, equip it, and disable the recipe
            RecipeDisplay.Option.Upgrade.UpgradeState.IsUnlocked = true;

            // Select this upgrade through the recipe
            RecipeDisplay.Option.SelectThisUpgrade();
            // Set state to null
            RecipeDisplay.SetState(null);
            // Play sound to let the player know items were taken
            SoundEngine.PlaySound(SoundID.Unlock);

        }
        /// <summary>
        /// Attempt crafting the upgrade. Returns true if the item was successfully crafted.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="recipe"></param>
        /// <returns></returns>
        private bool TryToCraftItem(Player player, UpgradeRecipe recipe) {
            List<Item> matchingItems = [];

            // Iterate the player's inventory and find the max amount of items
            for (int recipeIndex = 0; recipeIndex < recipe.Length; recipeIndex++)
            {
                for (int invIndex = 0; invIndex < 50; invIndex++)
                {
                    // If we didn't find an item, continue
                    if (!recipe.ItemsAndAmounts[recipeIndex].AcceptedTypes.Contains(player.inventory[invIndex].type)) continue;

                    // Put candidate item in the list
                    matchingItems.Add(player.inventory[invIndex]);
                }

                // Sum stacks of the items. If we didn't find every item, then we are unable to craft the recipe.
                int totalStack = matchingItems.Where(item => recipe.ItemsAndAmounts[recipeIndex].AcceptedTypes.Contains(item.type)).Sum(item => item.stack);
                if (totalStack < recipe.ItemsAndAmounts[recipeIndex].Stack) return false;
            }

            // The recipe is craftable. Let's start crafting it.
            for (int recipeIndex = 0; recipeIndex < recipe.Length; recipeIndex++)
            {
                // Take items required
                int itemsRequired = recipe.ItemsAndAmounts[recipeIndex].Stack;
                // Take items sequentially from inventory
                foreach (var item in matchingItems)
                {
                    // Don't take items if the type is not a candidate.
                    if (!recipe.ItemsAndAmounts[recipeIndex].AcceptedTypes.Contains(item.type)) continue;

                    // Begin taking items
                    int currentStack = 0;
                    while (currentStack < itemsRequired)
                    {
                        item.stack--;
                        currentStack++;
                        if (item.stack == 0)
                        {
                            item.TurnToAir();
                            item.maxStack = 0;
                        }
                    }
                }
            }
            return true;
        }
    }
}
