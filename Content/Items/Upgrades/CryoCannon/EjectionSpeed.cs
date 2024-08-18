using deeprockitems.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Upgrades.CryoCannon
{
    [ValidWeapons([
        typeof(Weapons.CryoCannon)
        ])]
    public class EjectionSpeed : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem)
        {
            modItem.Item.shootSpeed += 9f;
        }
        
    }
}
