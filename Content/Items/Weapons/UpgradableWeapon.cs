using deeprockitems.Common.DamageType;
using deeprockitems.Content.Upgrades;
using deeprockitems.Types;
using System;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
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
        /// The time it takes for the weapon to be able to fire again after being disabled.
        /// </summary>
        public float COOLDOWN_THRESHOLD { get; set; } = 140f;
        /// <summary>
        /// The initial cooldown used to determine whether the weapon will go on cooldown
        /// </summary>
        public float Cooldown { get; set; } = 0;
        /// <summary>
        /// The timer used to passively cool down the weapon if it is not being used.
        /// </summary>
        protected float _cooldownTimer = 0;
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
                    Cooldown -= 0.4f;
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
                Cooldown += 12;
                // Add to cooldown use timer
                _cooldownTimer = 90;
            }

        }
        public override bool CanUseItem(Player player) {
            return IsWeaponEnabled;
        }
        public float UseTimeScale { get; set; }
        public UpgradeList UpgradeMasterList { get; set; }
        public virtual void NewSetDefaults() { }
        public sealed override void SetDefaults() {
            NewSetDefaults();
            Item.DamageType = ModContent.GetInstance<DRGDamageClass>();
            base.SetDefaults();
            // Load upgrades and apply parent name to upgrade
            UpgradeMasterList = InitializeUpgrades();
        }
        public virtual void VerifyUpgrades() {
            ResetStats();
            foreach (var upgradeTier in UpgradeMasterList)
            {
                foreach (var upgrade in upgradeTier)
                {
                    if (upgrade.IsEquipped)
                    {
                        upgrade.Item_ModifyStats?.Invoke();
                    }
                }
            }
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
        public override bool? PrefixChance(int pre, UnifiedRandom rand) {
            return false;
        }
    }
}
