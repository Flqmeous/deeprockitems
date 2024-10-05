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
using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Buffs;

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
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage = (int)(item.damage * 1.2f);
                            }
                        }
                    },
                    new Upgrade("FireRate", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.useTime = item.useAnimation = (int)(item.useAnimation * 0.75f);
                            }
                        }
                    }
                ),
                new UpgradeTier(2,
                    new Upgrade("Sniper", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                (item.ModItem as JuryShotgun).SpreadMultiplier *= 0.75f;
                            }
                        }
                    },
                    new Upgrade("ReloadSpeed", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                (item.ModItem as JuryShotgun).CooldownTime *= 0.5f;
                            }
                        }
                    }
                ),
                new UpgradeTier(3,
                    new Upgrade("Birdshot", Assets.Upgrades.Penetrate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage -= 2;
                                (item.ModItem as JuryShotgun).PelletCount += 3;
                            }
                        }   
                                        },
                    new Upgrade("Buckshot", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage += 12;
                                (item.ModItem as JuryShotgun).PelletCount -= 1;
                            }
                        }
                    }
                ),
                new UpgradeTier(4,
                    new Upgrade("WhitePhosphorusShells", Assets.Upgrades.Heat.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (projectile, npc, hit, damage) => {
                                npc.ChangeTemperature(125 / PelletCount, projectile.owner);
                            }
                        }
                    },
                    new Upgrade("Shockwave", Assets.Upgrades.Heat.Value) {
                        Behavior = {
                            Item_OnShoot = (item, player, source, projectile) => {
                                // Find enemies around the player
                                foreach (var npc in Main.ActiveNPCs)
                                {
                                    if (player.Center.DistanceSQ(npc.Center) > 25000) continue;
                                    
                                    var hitinfo = npc.CalculateHitInfo(20, -1);
                                    player.StrikeNPCDirect(npc, hitinfo);
                                    
                                }
                            }
                        }
                    },
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage = (int)(item.damage * 1.2f);
                            }
                        }
                    }
                ),
                new UpgradeTier(5,
                    new Upgrade("QuadrupleBarrel", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                (item.ModItem as JuryShotgun).ShotsUntilCooldown = 4f;
                            }
                        }
                    },
                    new Upgrade("QuickFire", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.useTime = item.useAnimation = 6;
                            }
                        }
                    }
                )
            );
        }
        public override void ResetStats() {
            PelletCount = 3;
            Item.damage = Item.OriginalDamage;
            CooldownTime = 75f;
            ShotsUntilCooldown = 2f;
            SpreadMultiplier = 1f;
        }
        public override bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
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
            int numberProjectiles = PelletCount + Main.rand.Next(0, 1);
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