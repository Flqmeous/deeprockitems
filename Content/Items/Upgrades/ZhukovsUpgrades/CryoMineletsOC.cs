using deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Items.Upgrades.ZhukovsUpgrades
{
    [ValidWeapons(typeof(Zhukovs))]
    public class CryoMineletsOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<CryoMineletsOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddRecipeGroup(nameof(ItemID.AdamantiteBar), 10)
            .AddIngredient(ItemID.FrostCore, 3)
            .AddIngredient(ItemID.Grenade, 15)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();
        }
        /*public override bool? UpgradeProjectile_OnTileCollide(Projectile projectile, Vector2 oldVelocity, out bool callBase)
        {
            callBase = true;
            Point spawnTile = projectile.Center.ToTileCoordinates();
            // Move projectile right
            if (oldVelocity.X > projectile.velocity.X)
            {
                spawnTile.X++;
            }
            // Move projectile left
            if (oldVelocity.X < projectile.velocity.X)
            {
                spawnTile.X--;
            }
            // Move projectile down
            if (oldVelocity.Y > projectile.velocity.Y)
            {
                spawnTile.Y++;
            }
            // Move projectile up
            if (oldVelocity.Y < projectile.velocity.Y)
            {
                spawnTile.Y--;
            }
            // Spawn projectile at this new position.
            Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<Content.Projectiles.ZhukovProjectiles.CryoMineletProjectile>(), projectile.damage, 0f, projectile.owner, ai0: 40f, ai1: spawnTile.X, ai2: spawnTile.Y);
            proj.position += projectile.velocity * 2f;
            return true;
        }*/
        public class CryoMineletProjectile : UpgradeGlobalProjectile<CryoMineletsOC>
        {
            public override bool UpgradeOnTileCollide(Projectile projectile, Vector2 oldVelocity)
            {
                Point spawnTile = projectile.Center.ToTileCoordinates();
                // Move projectile right
                if (oldVelocity.X > projectile.velocity.X)
                {
                    spawnTile.X++;
                }
                // Move projectile left
                if (oldVelocity.X < projectile.velocity.X)
                {
                    spawnTile.X--;
                }
                // Move projectile down
                if (oldVelocity.Y > projectile.velocity.Y)
                {
                    spawnTile.Y++;
                }
                // Move projectile up
                if (oldVelocity.Y < projectile.velocity.Y)
                {
                    spawnTile.Y--;
                }
                // Spawn projectile at this new position.
                Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<Content.Projectiles.ZhukovProjectiles.CryoMineletProjectile>(), projectile.damage, 0f, projectile.owner, ai0: 40f, ai1: spawnTile.X, ai2: spawnTile.Y);
                proj.position += projectile.velocity * 2f;
                return true;
            }
        }
    }
}
