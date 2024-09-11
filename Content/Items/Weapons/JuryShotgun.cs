using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using System;
using deeprockitems.Content.Upgrades;

namespace deeprockitems.Content.Items.Weapons
{
    public class JuryShotgun : UpgradableWeapon
    {
        public override void NewSetDefaults()
        {
            ResetStats();
            Item.CloneDefaults(ItemID.Boomstick);
            Item.damage = 15;
            Item.width = 40;
            Item.height = 16;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }
        /// <summary>
        /// The multiplier given to the number of projectiles this shotgun shoots.
        /// </summary>
        public float ProjectileMultiplier { get; set; } = 1f;
        /// <summary>
        /// The multiplier given to the spread of this shotgun.
        /// </summary>
        public float SpreadMultiplier { get; set; } = 1f;
        /// <summary>
        /// The lower bound of the shotgun velocity
        /// </summary>
        public float VelocityLowerBound { get; set; } = 0.8f;
        public int PelletCount { get; set; } = 3;
        public override UpgradeList InitializeUpgrades() {
            return new UpgradeList("JuryShotgun",
                new UpgradeTier(1,
                    new Upgrade("MorePellets", Assets.Upgrades.Penetrate.Value) {
                        Item_ModifyStats = () => {
                            PelletCount += 2;
                        }
                    },
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Item_ModifyStats = () => {
                            Item.damage += 2;
                        }
                    }
                ),
                new UpgradeTier(2, 
                    new Upgrade("BiggerClip", Assets.Upgrades.FireRate.Value) {

                    }
                )
            );
        }
        public override void ResetStats() {
            COOLDOWN_THRESHOLD = 36f;
            PelletCount = 3;
            Item.damage = Item.OriginalDamage;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Change player's direction to face the cursor
            if (Main.MouseWorld.X > player.Center.X)
            {
                player.direction = 1;
            }
            else
            {
                player.direction = -1;
            }

            // Shoot logic
            int numberProjectiles = PelletCount + Main.rand.Next(1, 3);
            double spread = Math.PI / 13;

            // This block is for the projectile spread.
            int projectilesWithMultiplier = (int)Math.Floor(ProjectileMultiplier * numberProjectiles);
            for (int i = 0; i < projectilesWithMultiplier; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(spread * SpreadMultiplier) * Main.rand.NextFloat(VelocityLowerBound, 1.2f); // random velocity effect
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI, numberProjectiles);
            }
            return true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<JuryShotgun>())
                .AddIngredient(ItemID.Boomstick, 1)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddRecipeGroup(nameof(ItemID.DemoniteBar), 8)
                .AddRecipeGroup(nameof(ItemID.VilePowder), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}