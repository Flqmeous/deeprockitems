using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Buffs;
using Terraria.DataStructures;

namespace deeprockitems.Content.Items.Upgrades
{
    [ValidWeapons(
        typeof(PlasmaPistol),
        typeof(SludgePump))]
    public class OvertunedNozzle: UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {

        }
        public class OvertunedNozzleProjectile : UpgradeGlobalProjectile<OvertunedNozzle>
        {
            
        }
        public override void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem)
        {
            modItem.Item.shootSpeed *= 1.5f;
        }
    }
}
