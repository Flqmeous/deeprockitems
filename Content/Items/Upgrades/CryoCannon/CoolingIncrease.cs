using deeprockitems.Content.Buffs;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Projectiles.CryoCannonProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Upgrades.CryoCannon
{
    [ValidWeapons([
        typeof(Weapons.CryoCannon)
        ])]
    public class CoolingIncrease : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public class CoolingIncreaseProjectile : UpgradeGlobalProjectile<CoolingIncrease>
        {
            public override void UpgradeOnSpawn(Projectile projectile, IEntitySource source)
            {
                if (projectile.ModProjectile is CryoProjectile proj)
                {
                    proj.CoolingAmount *= 1.5f;
                }
            }
        }
    }
}
