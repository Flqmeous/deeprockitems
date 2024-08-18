using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using deeprockitems.Content.Buffs;
using System.Linq;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades
{
    [ValidWeapons(typeof(PlasmaPistol))]
    public class ColdPlasmaOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<ColdPlasmaOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddRecipeGroup(nameof(ItemID.CobaltBar), 10)
            .AddIngredient(ItemID.FrostCore, 1)
            .AddIngredient(ItemID.FallenStar, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
        public class ColdPlasmaProjectile : UpgradeGlobalProjectile<ColdPlasmaOC>
        {
            public override void UpgradeOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (projectile.type == ModContent.ProjectileType<BigPlasma>())
                {
                    // Add large amount of freeze amount
                    target.ChangeTemperature(-50, projectile.owner);
                }
                if (projectile.type == ModContent.ProjectileType<PlasmaBullet>())
                {
                    // Add low amount of freeze
                    target.ChangeTemperature(-25, projectile.owner);
                }
            }
        }
    }
}
