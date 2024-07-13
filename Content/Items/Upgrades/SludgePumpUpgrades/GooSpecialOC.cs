using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria.DataStructures;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using Microsoft.Xna.Framework;
using System.Linq;

namespace deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades
{
    public class GooSpecialOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<GooSpecialOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.MythrilBar, 10)
            .AddIngredient(ItemID.Gel, 20)
            .AddIngredient(ItemID.Ale, 5)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<GooSpecialOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.OrichalcumBar, 10)
            .AddIngredient(ItemID.Gel, 20)
            .AddIngredient(ItemID.Ale, 5)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();
        }
        public class GooSpecialProjectile : UpgradeGlobalProjectile<GooSpecialOC>
        {
            public override void UpgradeOnSpawn(Projectile projectile, IEntitySource source)
            {
                if (projectile.ModProjectile is SludgeBall ball)
                {
                    ball.CancelBaseKill = true;
                }
            }
            private int _timer;
            public override void UpgradeAI(Projectile projectile)
            {
                // Set AI
                if (projectile.ModProjectile is SludgeBall && projectile.ai[1] <= 900 && projectile.ai[1] > 0)
                {
                    _timer++;

                    if (_timer % 8 == 0)
                    {
                        // Spawn another sludgeball
                        SludgeBall ball = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<SludgeBall>(), (int)(projectile.damage * 0.75f), 0f, projectile.owner).ModProjectile as SludgeBall;
                        ball.CancelBaseKill = false;
                        ball.Projectile.ai[1] = 50;
                    }
                }
            }
        }
    }
}
