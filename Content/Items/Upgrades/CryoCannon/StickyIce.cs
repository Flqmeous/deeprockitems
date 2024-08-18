using deeprockitems.Content.Projectiles.CryoCannonProjectiles;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Items.Upgrades.CryoCannon
{
    [ValidWeapons(typeof(Weapons.CryoCannon))]
    public class StickyIce : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public class StickyIceProjectile : UpgradeGlobalProjectile<StickyIce>
        {
            public override bool UpgradeOnTileCollide(Projectile projectile, Vector2 oldVelocity)
            {
                Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<IceTrail>(), projectile.damage, 0f, owner: projectile.owner);
                // Get rotation from old velocity
                proj.rotation = oldVelocity.ToRotation() + MathHelper.Pi + Main.rand.NextFloat(-MathHelper.Pi / 8, MathHelper.Pi / 8);
                
                return base.UpgradeOnTileCollide(projectile, oldVelocity);
            }
        }
    }
}
