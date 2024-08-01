using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.JuryShotgunUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using Microsoft.Xna.Framework;
using System.Linq;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades
{
    [ValidWeapons(typeof(PlasmaPistol))]
    public class PersistantPlasmaOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<PersistantPlasmaOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddRecipeGroup(nameof(ItemID.DemoniteBar), 10)
            .AddIngredient(ItemID.FallenStar, 10)
            .AddIngredient(ItemID.PinkGel, 15)
            .AddTile(TileID.Anvils)
            .Register();
        }
        public class PersistantPlasmaProjectile : UpgradeGlobalProjectile<PersistantPlasmaOC>
        {
            public override void UpgradeOnKill(Projectile projectile, int timeLeft)
            {
                if (timeLeft != 0 && projectile.type == ModContent.ProjectileType<BigPlasma>())
                {
                    Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaExplosion>(), (int)(projectile.damage * 0.85f), 0f, owner: projectile.owner);
                    proj.timeLeft = 60 * 5;
                }
            }
        }
    }
}
