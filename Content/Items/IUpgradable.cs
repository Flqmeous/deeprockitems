using deeprockitems.Content.Upgrades;
using deeprockitems.Types;
using System;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace deeprockitems.Content.Items
{
    public interface IUpgradable
    {
        public UpgradeList UpgradeMasterList { get; set; }
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
        public void ResetStats() { }
        /// <summary>
        /// Initializes the master list of upgrades this weapon will use.
        /// </summary>
        /// <returns></returns>
        public UpgradeList InitializeUpgrades();
    }
}
