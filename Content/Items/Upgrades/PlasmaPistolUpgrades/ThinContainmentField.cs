﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using deeprockitems.Utilities;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades
{
    [ValidWeapons(typeof(PlasmaPistol))]
    public class ThinContainmentField : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<ThinContainmentField>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.HellstoneBar, 15)
            .AddIngredient(ItemID.Fireblossom, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
        public class ThinContainmentFieldProjectile : UpgradeGlobalProjectile<ThinContainmentField>
        {
            public override void UpgradeAI(Projectile projectile)
            {
                if (projectile.type == ModContent.ProjectileType<BigPlasma>() && projectile.owner == Main.myPlayer)
                {
                    // Find whoAmI of a plasma bullet colliding with this projectile
                    int whoAmI = projectile.IsCollidingWithProjectile(ModContent.ProjectileType<PlasmaBullet>());
                    if (whoAmI != -1)
                    {
                        // Spawn projectile, play sound
                        SoundEngine.PlaySound(SoundID.Item14 with { Volume = .5f, Pitch = -.8f }); // Sound of the projectile 
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaExplosion>(), projectile.damage, 0f, Owner: whoAmI);
                        // Kill both older projectiles
                        projectile.Kill();
                        Main.projectile[whoAmI].Kill();
                    }
                }
            }
        }
    }
}
