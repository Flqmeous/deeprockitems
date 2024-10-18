using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Upgrades;
using deeprockitems.UI.UpgradeUI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace deeprockitems.Content.Items.Weapons
{
    public abstract class UpgradableWeapon : ModItem, IUpgradable
    {
        /// <summary>
        /// Whether the weapon is disabled or not.
        /// </summary>
        public bool IsWeaponEnabled { get; private set; } = true;
        /// <summary>
        /// The threshold before the weapon is disabled.
        /// </summary>
        public const float COOLDOWN_THRESHOLD = 100f;
        /// <summary>
        /// How much cooldown is gained per shot. Defaults to 12f.
        /// </summary>
        public float ShotsUntilCooldown { get; set; } = 12f;
        /// <summary>
        /// The initial cooldown used to determine whether the weapon will go on cooldown
        /// </summary>
        public float Cooldown { get; set; } = 0;
        /// <summary>
        /// The time in ticks it takes for the weapon to cool down.
        /// </summary>
        public float CooldownTime { get; set; } = 2f;
        /// <summary>
        /// The timer used to passively cool down the weapon if it is not being used.
        /// </summary>
        protected float _cooldownTimer = 0;
        public override void HoldItem(Player player) {
            foreach (var tier in UpgradeMasterList)
            {
                foreach (var upgrade in tier)
                {
                    if (upgrade.UpgradeState.IsEquipped)
                    {
                        upgrade.Behavior.Item_HoldItemHook?.Invoke(Item, player);
                    }
                }
            }
        }
        public override void UpdateInventory(Player player) {
            // PSEUDOCODE FOR COOLDOWN
            /*
             * if cooldown active:
             *      disable weapon
             *      decrement cooldown
             * if cooldown at 0:
             *      re-enable weapon
             *  
             * 
             */
            // If cooldown timer is at 0, start passively cooling the weapon
            if (_cooldownTimer <= 0)
            {
                if (Cooldown > 0)
                {
                    Cooldown -= (COOLDOWN_THRESHOLD / CooldownTime);
                }
                else
                {
                    Cooldown = 0f;
                }
            }
            else // Else decrement time since using this weapon
            {
                _cooldownTimer--;
            }
            if (Cooldown <= 0) // If cooldown is at or below 0, re-enable weapon
            {
                IsWeaponEnabled = true;
            }
            // Disable weapon if cooldown gets too high
            if (Cooldown >= COOLDOWN_THRESHOLD)
            {
                IsWeaponEnabled = false;
                _cooldownTimer = 0;
                Cooldown = COOLDOWN_THRESHOLD;
            }

            // Calculate whether cooldown should begin or be added to
            // Ignore other items
            if (player.HeldItem != Item) return;

            // If player channeling, increment cooldown timer to prevent it from going down
            if (player.channel && _cooldownTimer > 0)
            {
                _cooldownTimer++;
            }

            // Detect release
            if (player.ItemAnimationActive && player.itemAnimation + 1 == Item.useAnimation)
            {
                // Add to cooldown
                Cooldown += COOLDOWN_THRESHOLD / ShotsUntilCooldown;
                // Add to cooldown use timer
                _cooldownTimer = 90;
            }

        }
        #region Saving and loading upgrades
        public override void SaveData(TagCompound tag) {
            // Serialize each tag with the key being the upgrade name, and the data being the UpgradeState
            foreach (var tier in UpgradeMasterList)
            {
                foreach (var upgrade in tier)
                {
                    // Save each upgrade state
                    tag.Add($"{upgrade.InternalName}.{tier.Tier}.State", upgrade.UpgradeState);
                }
            }
        }
        public override void LoadData(TagCompound tag) {
            // Set upgrades' states
            foreach (var tier in UpgradeMasterList)
            {
                foreach (var upgrade in tier)
                {
                    // Early continue ensures that this upgrade must exist.
                    if (!tag.ContainsKey($"{upgrade.InternalName}.{tier.Tier}.State")) continue;
                    // If this upgrade name matches the key, then we can safely set that upgrade's state
                    upgrade.UpgradeState = tag.Get<UpgradeStateBinding>($"{upgrade.InternalName}.{tier.Tier}.State");
                }
            }
            // Apply stat upgrades
            ApplyStatUpgrades();
        }
        #endregion
        public override bool? CanAutoReuseItem(Player player) {
            if (Item.channel && player.autoReuseAllWeapons) return true;
            return null;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            // Remove the crit line
            tooltips.Find(tl => tl.FullName == "Terraria/CritChance")?.Hide();
        }
        public override bool CanUseItem(Player player) {
            return IsWeaponEnabled;
        }
        protected int _oldUseTime;
        protected int _oldUseAnimation;
        public UpgradeList UpgradeMasterList { get; set; }
        public Dictionary<string, UpgradeStateBinding> Upgrades = new();
        public virtual void NewSetDefaults() { }
        public sealed override void SetDefaults() {
            ResetStats();
            NewSetDefaults();
            _oldUseTime = Item.useTime;
            _oldUseAnimation = Item.useAnimation;
            base.SetDefaults();
            // Load upgrades and apply parent name to upgrade
            UpgradeMasterList = InitializeUpgrades();
        }
        public virtual void ApplyStatUpgrades() {
            // Reset global stats
            Item.useTime = _oldUseTime;
            Item.useAnimation = _oldUseAnimation;
            ResetStats();
            foreach (var upgradeTier in UpgradeMasterList)
            {
                foreach (var upgrade in upgradeTier)
                {
                    if (upgrade.UpgradeState.IsEquipped)
                    {
                        upgrade.Behavior.Item_ModifyStats?.Invoke(Item);
                    }
                }
            }
        }
        private Upgrade[] GetEquippedUpgrades() {
            return (from upgradeTier in UpgradeMasterList
                   from upgrade in upgradeTier
                   where upgrade.UpgradeState.IsEquipped
                   select upgrade).ToArray();
        }
        public override void Load() {
            _ = InitializeUpgrades();
        }
        public virtual void ResetStats() { }
        /// <summary>
        /// Initializes the master list of upgrades this weapon will use.
        /// </summary>
        /// <returns></returns>
        public virtual UpgradeList InitializeUpgrades() => new("notSet");
        public override sealed void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            NewSetStaticDefaults();
        }
        public virtual void NewSetStaticDefaults() { }
        public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var newSource = new EntitySource_FromUpgradableWeapon(player, this, source.AmmoItemIdUsed, source.Context);
            if (NewShoot(player, newSource, position, velocity, type, damage, knockback))
            {
                Projectile spawnedProj = Projectile.NewProjectileDirect(newSource, position, velocity, type, damage, knockback, owner: player.whoAmI);
                // Activate upgrades if equipped
                foreach (var upgrade in GetEquippedUpgrades())
                {
                    upgrade.Behavior.Item_OnShoot?.Invoke(Item, player, newSource, spawnedProj);
                }
            }
            return false;
        }
        public virtual bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => true;
        public override bool? PrefixChance(int pre, UnifiedRandom rand) {
            return false;
        }
    }
}
