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
    public abstract class UpgradeableItemTemplate : ModItem
    {
        public float UseTimeScale { get; set; }
        public UpgradeList UpgradeMasterList { get; private set; }
        public virtual void NewSetDefaults() { }
        public sealed override void SetDefaults()
        {
            NewSetDefaults();
            base.SetDefaults();
            // Load upgrades and apply parent name to upgrade
            UpgradeMasterList = InitializeUpgrades();
        }
        public virtual void VerifyUpgrades()
        {
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
        public override void Load()
        {
            _ = InitializeUpgrades();
        }
        public virtual void ResetStats() { }
        /// <summary>
        /// Initializes the master list of upgrades this weapon will use.
        /// </summary>
        /// <returns></returns>
        public virtual UpgradeList InitializeUpgrades() => new("notSet");
        public override sealed void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            NewSetStaticDefaults();
        }
        public virtual void NewSetStaticDefaults() { }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return false;
        }
    }
}
