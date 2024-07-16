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
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Assets.Textures;
using Terraria.GameContent.Creative;

namespace deeprockitems.Content.Items.Weapons
{
    public abstract class UpgradeableItemTemplate : ModItem
    {
        public int BaseDamage { get; private set; }
        public float DamageScale { get; set; } = 1f;
        public float UseTimeScale { get; set; }
        /// <summary>
        /// The item useTime before being affected by useTimeModifier.
        /// </summary>
        public int BaseUseTime { get; private set; }
        /// <summary>
        /// The item useAnimation before being affected by useTimeModifier.
        /// </summary>
        public int BaseUseAnimation { get; private set; }
        private int[] saved_upgrades;
        public int[] Upgrades { get; set; }
        public int Overclock { get => Upgrades[^1]; }
        public virtual string CurrentOverclock { get; set; } = "";
        public virtual string OverclockPositives { get; set; } = "";
        public virtual string OverclockNegatives { get; set; } = "";
        public List<int> ValidUpgrades { get; set; }
        // private bool load_flag = false;
        public virtual void NewSetDefaults()
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
            NewSetDefaults();
            BaseUseTime = Item.useTime;
            BaseUseAnimation = Item.useAnimation;
            BaseDamage = Item.damage;
            base.SetDefaults();
        }
        public override void SetStaticDefaults()
        {
            UpgradeTemplate.DrawingWeaponIconInBottomLeft += DrawWeaponIconInBottomLeft;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            NewSetStaticDefaults();
        }
        public virtual void NewSetStaticDefaults() { }
        private void DrawWeaponIconInBottomLeft(UpgradeTemplate sender, UpgradeTemplate.DrawArgs args)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            // Get position of bottom of slot:
            float newScale = args.Scale * (texture.Width <= 40f ? 0.65f : 0.5f);
            float yOffset = texture.Height * 0.5f * newScale;
            /*float bottomYValue = args.Position.Y - yOffset + args.Scale * 0.5f * 52;*/
            Vector2 bottomLeftOfSlot = new Vector2(args.Position.X - 0.5f * args.Scale * 52 + 4f, args.Position.Y + args.Scale * 0.5f * 52 - 4f);
            Vector2 drawPos = new Vector2(bottomLeftOfSlot.X, bottomLeftOfSlot.Y - yOffset);
            args.SpriteBatch.Draw(texture, new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)(texture.Width * newScale), (int)(texture.Height * newScale)), Color.White);
            args.SpriteBatch.Draw(DRGTextures.WhitePixel, drawPos, Color.Orange);
            /*if (ValidUpgrades.Contains(sender.Type))
            {
                args.SpriteBatch.Draw(texture, new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)(texture.Width * newScale), (int)(texture.Height * newScale)), Color.White);
            }*/



            /*float newScale = args.Scale * 0.5f; // New scale to fit in the bottom left corner
            float yOff = args.Frame.Height * 0.5f;
            // Move to new location from this item's sprite
            Vector2 newPosition = new Vector2(args.Position.X - 0.5f * newScale * Item.width, args.Position.Y + yOff + newScale * Item.width);
            float scaledWidth = args.Frame.Width * newScale;
            float scaledHeight = args.Frame.Height * newScale;
            if (ValidUpgrades.Contains(sender.Type))
            {
                args.SpriteBatch.Draw(texture, new Rectangle((int)newPosition.X, (int)newPosition.Y, (int)scaledWidth, (int)scaledHeight), Color.White);
            }*/
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
            Item.damage = BaseDamage;
            Item.useTime = BaseUseTime;
            Item.useAnimation = BaseUseAnimation;
            ResetStats();

            // Hook injection
            ItemStatChange?.Invoke(this, Upgrades);

            // Set damage
            Item.damage = (int)Math.Floor(Item.damage * DamageScale);

            // Set usetime
            Item.useTime = (int)Math.Ceiling(Item.useTime * UseTimeScale);
            Item.useAnimation = (int)Math.Ceiling(Item.useAnimation * UseTimeScale);
        }
        public virtual void ResetStats() { }
        public delegate void HandleItemStatChange(UpgradeableItemTemplate sender, int[] upgrades);
        public static event HandleItemStatChange ItemStatChange;

        public sealed override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // When the weapon is fired with right click (presumably unused)
            bool shouldCallBase = true;
            if (player.altFunctionUse == 2)
            {
                // Call upgrade hooks
                foreach (var hook in ItemModifyShootAltUse.GetInvocationList().Cast<HandleItemModifyShootAltUse>())
                {
                    if (hook.Target is UpgradeTemplate upgrade)
                    {
                        if (Upgrades.Contains(upgrade.Type))
                        {
                            hook.Invoke(this, player, ref position, ref velocity, ref type, ref damage, ref knockback, out bool callBase);
                            shouldCallBase = !(shouldCallBase || callBase);
                        }
                    }
                }
                if (shouldCallBase)
                {
                    ModifyShootAltUse(player, ref position, ref velocity, ref type, ref damage, ref knockback);
                }
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


        public delegate void HandleItemModifyShootPrimaryUse(UpgradeableItemTemplate sender, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, out bool callBase);
        public delegate void HandleItemModifyShootAltUse(UpgradeableItemTemplate sender, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, out bool callBase);

        public static event HandleItemModifyShootPrimaryUse ItemModifyShootPrimaryUse;
        public static event HandleItemModifyShootAltUse ItemModifyShootAltUse;




        public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // When the weapon is fired with right click (presumably unused)
            List<bool> callsBase = new();
            List<bool> spawnsProjectile = new();
            if (player.altFunctionUse == 2)
            {
                // Call upgrade hooks
                foreach (var hook in ItemShootAltUse.GetInvocationList().Cast<HandleItemShootAltUse>())
                {
                    if (hook.Target is UpgradeTemplate upgrade)
                    {
                        if (Upgrades.Contains(upgrade.Type))
                        {
                            spawnsProjectile.Add(hook.Invoke(this, player, source, position, velocity, type, damage, knockback, out bool callBase));
                            spawnsProjectile.Add(callBase);
                        }
                    }
                }
                for (int i = 0; i < callsBase.Count; i++)
                {
                    if (!callsBase[i])
                    {
                        return spawnsProjectile[i];
                    }
                }
                return ShootAltUse(player, source, position, velocity, type, damage, knockback);
            }

            foreach (var hook in ItemShootPrimaryUse.GetInvocationList().Cast<HandleItemShootPrimaryUse>())
            {
                if (hook.Target is UpgradeTemplate upgrade)
                {
                    if (Upgrades.Contains(upgrade.Type))
                    {
                        spawnsProjectile.Add(hook.Invoke(this, player, source, position, velocity, type, damage, knockback, out bool callBase));
                        callsBase.Add(callBase);
                    }
                }
            }
            for (int i = 0; i < callsBase.Count; i++)
            {
                if (!callsBase[i])
                {
                    return spawnsProjectile[i];
                }
            }
            return ShootPrimaryUse(player, source, position, velocity, type, damage, knockback);
        }
        /// <summary>
        /// Functions identically to <see cref="ModItem.Shoot"/> and should replace it.
        /// </summary>
        public virtual bool ShootPrimaryUse(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => true;
        /// <summary>
        /// Functions identically to <see cref="ModItem.Shoot"/>, however this only activates when the weapon is fired with alternate use (right click).
        /// </summary>
        public virtual bool ShootAltUse(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => false;


        public delegate bool HandleItemShootPrimaryUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool callBase);
        public delegate bool HandleItemShootAltUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool callBase);

        public static event HandleItemShootPrimaryUse ItemShootPrimaryUse;
        public static event HandleItemShootAltUse ItemShootAltUse;


        public delegate void HandleItemHold(UpgradeableItemTemplate sender, Player player, int[] upgrades);
        public static event HandleItemHold ItemHold;

        public sealed override void HoldItem(Player player)
        {
            ItemHold?.Invoke(this, player, Upgrades);
            ItemOnHold(player);
        }
        public virtual void ItemOnHold(Player player) { }



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
