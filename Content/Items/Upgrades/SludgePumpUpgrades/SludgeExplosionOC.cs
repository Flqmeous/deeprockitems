using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using Microsoft.Xna.Framework;
using System;

namespace deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades
{
    [ValidWeapons(typeof(SludgePump))]
    public class SludgeExplosionOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<SludgeExplosionOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddRecipeGroup(nameof(ItemID.CobaltBar), 10)
            .AddIngredient(ItemID.Gel, 15)
            .AddIngredient(ItemID.Bomb, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
        public class SludgeExplosionProjectile : UpgradeGlobalProjectile<SludgeExplosionOC>
        {
            public override bool UpgradePreKill(Projectile projectile, int timeLeft)
            {
                if (projectile.ModProjectile is SludgeBall ball)
                {
                    ball.CancelBaseKill = true;
                }
                return base.UpgradePreKill(projectile, timeLeft);
            }
            public override void UpgradeOnKill(Projectile projectile, int timeLeft)
            {
                if (projectile.ModProjectile is SludgeBall)
                {
                    Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, projectile.velocity, ModContent.ProjectileType<SludgeExplosion>(), (int)Math.Floor(projectile.damage * 1.5f), projectile.knockBack, projectile.owner);
                }
            }
        }
    }
}
