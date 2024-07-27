using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using deeprockitems.Content.Buffs;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades
{
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
            Recipe upgrade = Recipe.Create(ModContent.ItemType<ColdPlasmaOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddIngredient(ItemID.FallenStar, 10)
            .AddIngredient(ItemID.ScarabBomb, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
        public class ColdPlasmaProjectile : UpgradeGlobalProjectile<ColdPlasmaOC>
        {
            public override void UpgradeOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (projectile.type == ModContent.ProjectileType<BigPlasma>())
                {
                    target.AddBuff(ModContent.BuffType<FrozenEnemy>(), 120);
                }
                if (projectile.type == ModContent.ProjectileType<PlasmaBullet>())
                {
                    target.AddBuff(BuffID.Frostburn, 60);
                }
                if (projectile.type == ModContent.ProjectileType<PlasmaExplosion>())
                {
                    target.AddBuff(ModContent.BuffType<FrozenEnemy>(), 120);
                    target.AddBuff(BuffID.Frostburn, 120);
                }
            }
        }
    }
}
