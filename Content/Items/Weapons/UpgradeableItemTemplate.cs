using deeprockitems.Content.Items.Upgrades;
using deeprockitems.UI.UpgradeItem;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using SmartFormat.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;

namespace deeprockitems.Content.Items.Weapons
{
    public abstract class UpgradeableItemTemplate : ModItem
    {
        public int OriginalDamage { get; set; }
        public float DamageScale { get; set; } = 1f;
        public float UseTimeScale { get; set; }
        /// <summary>
        /// The item useTime before being affected by useTimeModifier.
        /// </summary>
        protected int oldUseTime;
        /// <summary>
        /// The item useAnimation before being affected by useTimeModifier.
        /// </summary>
        protected int oldUseAnimation;
        private int[] saved_upgrades;
        public int[] Upgrades { get; set; }
        public int Overclock { get => Upgrades[^1]; }
        public virtual string CurrentOverclock { get; set; } = "";
        public virtual string OverclockPositives { get; set; } = "";
        public virtual string OverclockNegatives { get; set; } = "";
        public List<int> ValidUpgrades { get; set; }
        // private bool load_flag = false;
        public virtual void SafeDefaults()
        {
            
        }
        public override void SetDefaults()
        {
            saved_upgrades = new int[4];
            Upgrades = new int[4];
            UseTimeScale = 1f;
            ValidUpgrades = new()
            {
                ModContent.ItemType<DamageUpgrade>(),
                ModContent.ItemType<ArmorPierce>(),
                ModContent.ItemType<Blowthrough>()
            };
            SafeDefaults();
            oldUseTime = Item.useTime;
            oldUseAnimation = Item.useAnimation;
            OriginalDamage = Item.damage;
            base.SetDefaults();
        }
        public override void UpdateInventory(Player player)
        {
            if (!Main.playerInventory)
            {
                Close_UI();
            }
            RegisterStatUpgrades();
        }

        public override void RightClick(Player player)
        {
            Item.stack += 1;
            if (UpgradeUISystem.UpgradeUIPanel.ParentSlot.ItemToDisplay != Item)
            {
                Open_UI();
            }
            else
            {
                Close_UI();
            }
            RegisterStatUpgrades();
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void SaveData(TagCompound tag)
        {
            for (int upgrade_index = 0; upgrade_index < Upgrades.Length; upgrade_index++)
            {
                if (Upgrades[upgrade_index] == 0)
                {
                    saved_upgrades[upgrade_index] = -1;
                    continue;
                }
                for (int valid_index = 0; valid_index < ValidUpgrades.Count; valid_index++)
                {
                    if (ValidUpgrades[valid_index] == Upgrades[upgrade_index])
                    {
                        saved_upgrades[upgrade_index] = valid_index;
                    }
                }
            }
            tag["Upgrades"] = saved_upgrades;
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("Upgrades"))
            {
                saved_upgrades = (int[])tag["Upgrades"];
                for (int upgrade_index = 0; upgrade_index < saved_upgrades.Length; upgrade_index++)
                {
                    if (saved_upgrades[upgrade_index] == -1)
                    {
                        Upgrades[upgrade_index] = 0;
                        continue;
                    }
                    for (int valid_index = 0; valid_index < ValidUpgrades.Count; valid_index++)
                    {
                        if (saved_upgrades[upgrade_index] == valid_index)
                        {
                            Upgrades[upgrade_index] = ValidUpgrades[valid_index];
                        }
                    }
                }
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (ItemSlot.ShiftInUse)
            {
                string upgrades_text = "";
                for (int u = 0; u < Upgrades.Length; u++)
                {
                    UpgradeTemplate upgrade_item = new Item(Upgrades[u]).ModItem as UpgradeTemplate;
                    if (upgrade_item is null)
                    {
                        continue;
                    }
                    if (Upgrades[u] == Overclock && Overclock != 0) // If this is the overclock && overclock exists
                    {
                        upgrades_text = upgrades_text + Language.GetTextValue("Mods.deeprockitems.Misc.DefaultOverclockText", upgrade_item.DisplayName) + "\n";
                        upgrades_text = upgrades_text + upgrade_item.GetLocalizedValue("Positives") + "\n";
                        upgrades_text = upgrades_text + upgrade_item.GetLocalizedValue("Negatives") + "\n";
                        break;
                    }
                    if (Upgrades[u] != 0)
                    {
                        upgrades_text = upgrades_text + string.Format("[c/23DF24:▲{0}]", upgrade_item.Tooltip) + "\n";
                    }
                }
                upgrades_text = upgrades_text.TrimEnd('\n');
                TooltipLine line = new(Mod, "Upgrades", upgrades_text);
                TooltipHelpers.InsertTooltip(tooltips, line, "OneDropLogo");
            }
            else if (Upgrades.Count(0) < 4)
            {
                TooltipLine line = new(Mod, "ShiftToView", Language.GetTextValue("Mods.deeprockitems.Misc.UpgradeShiftTooltip"))
                {
                    OverrideColor = new(74, 177, 211),
                };
                TooltipHelpers.InsertTooltip(tooltips, line, "OneDropLogo");
            }
            if (UpgradeUISystem.Interface.CurrentState is null)
            {
                // How to open the upgrade menu?
                TooltipLine line = new(Mod, "HowToOpenMenu", Language.GetTextValue("Mods.deeprockitems.Misc.HowToOpenMenu"))
                {
                    OverrideColor = new(74, 177, 211),
                };
                TooltipHelpers.InsertTooltip(tooltips, line, "OneDropLogo");
            }
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return false;
        }
        public void RegisterStatUpgrades()
        {
            // Reset stats
            DamageScale = 1f;
            UseTimeScale = 1f;
            Item.damage = OriginalDamage;
            ResetStats();

            // Hook injection
            ItemStatChange?.Invoke(this, Upgrades);

            // Set damage
            Item.damage = (int)Math.Floor(OriginalDamage * DamageScale);

            // Set usetime
            Item.useTime = (int)Math.Ceiling(oldUseTime * UseTimeScale);
            Item.useAnimation = (int)Math.Ceiling(oldUseAnimation * UseTimeScale);
        }
        public virtual void ResetStats() { }
        public delegate void HandleItemStatChange(UpgradeableItemTemplate sender, int[] upgrades);
        public static event HandleItemStatChange ItemStatChange;


        public sealed override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // When the weapon is fired with right click (presumably unused)
            if (player.altFunctionUse == 2)
            {
                // Call upgrade hook, then alt shoot
                //ItemModifyShootAltUse?.Invoke(this, player, ref position, ref velocity, ref type, ref damage, ref knockback, Upgrades);
                ModifyShootAltUse(player, ref position, ref velocity, ref type, ref damage, ref knockback);
                return;
            }
            // Call upgrade hook, then primary shoot
            //ItemModifyShootPrimaryUse?.Invoke(this, player, ref position, ref velocity, ref type, ref damage, ref knockback, Upgrades);
            ModifyShootPrimaryUse(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        /// <summary>
        /// Functions identically to <see cref="ModItem.ModifyShootStats"/> and should replace it.
        /// </summary>
        public virtual void ModifyShootPrimaryUse(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
        /// <summary>
        /// Functions identically to <see cref="ModItem.ModifyShootStats"/>, however this only activates when the weapon is fired with alternate use (right click).
        /// </summary>
        public virtual void ModifyShootAltUse(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }


/*        public delegate void HandleItemModifyShootPrimaryUse(UpgradeableItemTemplate sender, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, int[] upgrades);
        public delegate void HandleItemModifyShootAltUse(UpgradeableItemTemplate sender, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, int[] upgrades);

        public static event HandleItemModifyShootPrimaryUse ItemModifyShootPrimaryUse;
        public static event HandleItemModifyShootAltUse ItemModifyShootAltUse;
*/



        public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // When the weapon is fired with right click (presumably unused)
            if (player.altFunctionUse == 2)
            {
                // Call upgrade hook, then alt shoot
                ItemShootAltUse?.Invoke(this, player, source, position, velocity, type, damage, knockback, Upgrades);
                return ShootAltUse(player, source, position, velocity, type, damage, knockback);
            }
            // Call upgrade hook, then primary shoot
            ItemShootPrimaryUse?.Invoke(this, player, source, position, velocity, type, damage, knockback, Upgrades);
            return ShootPrimaryUse(player, source, position, velocity, type, damage, knockback);
        }
        /// <summary>
        /// Functions identically to <see cref="ModItem.Shoot"/> and should replace it.
        /// </summary>
        public virtual bool ShootPrimaryUse(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => false;
        /// <summary>
        /// Functions identically to <see cref="ModItem.Shoot"/>, however this only activates when the weapon is fired with alternate use (right click).
        /// </summary>
        public virtual bool ShootAltUse(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => false;


        public delegate void HandleItemShootPrimaryUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int[] upgrades);
        public delegate void HandleItemShootAltUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int[] upgrades);

        public static event HandleItemShootPrimaryUse ItemShootPrimaryUse;
        public static event HandleItemShootAltUse ItemShootAltUse;


        public virtual void UniqueUpgrades()
        {
            
        }
        private void Open_UI()
        {
            UpgradeUISystem.UpgradeUIPanel.ClearItems();
            UpgradeUISystem.Interface.SetState(UpgradeUISystem.UpgradeUIPanel);
            UpgradeUISystem.UpgradeUIPanel.ParentSlot.ItemToDisplay = Item;
            UpgradeUISystem.UpgradeUIPanel.ShowItems();
        }
        private void Close_UI()
        {
            UpgradeUISystem.UpgradeUIPanel.ParentSlot.ItemToDisplay = null;
            UpgradeUISystem.UpgradeUIPanel.ClearItems();
            UpgradeUISystem.Interface.SetState(null);
        }
    }
}
