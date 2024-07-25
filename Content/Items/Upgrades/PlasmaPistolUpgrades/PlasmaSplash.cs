using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades
{
    public class PlasmaSplash : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<PlasmaSplash>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.GoldBar), 10)
            .AddIngredient(ItemID.FallenStar, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
        public class PlasmaSplashProjectile : UpgradeGlobalProjectile<PlasmaSplash>
        {
            public override void UpgradeOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (projectile.owner == Main.myPlayer)
                {
                    target.immune[projectile.owner] = 15;
                }

                PlasmaSplash(projectile);
            }
            public override bool UpgradeOnTileCollide(Projectile projectile, Vector2 oldVelocity)
            {
                PlasmaSplash(projectile);
                return base.UpgradeOnTileCollide(projectile, oldVelocity);
            }
            private static void PlasmaSplash(Projectile projectile)
            {
                if (projectile.type == ModContent.ProjectileType<PlasmaBullet>() && Main.myPlayer == projectile.owner)
                {
                    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, Vector2.Zero, ModContent.ProjectileType<PlasmaSplashProj>(), projectile.damage, projectile.knockBack, projectile.owner);
                }
            }
        }
    }
}
