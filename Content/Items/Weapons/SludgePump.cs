using deeprockitems.Common.PlayerLayers;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class SludgePump : UpgradableWeapon
    {
        public override void NewSetDefaults()
        {
            Item.damage = 34;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.crit = 4;
            Item.width = 70;
            Item.height = 36;
            Item.mana = 10;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<SludgeHelper>();
            Item.shootSpeed = 22f;
            Item.rare = ItemRarityID.Orange;

            Item.value = Item.sellPrice(0, 5, 30, 0);

        }
        public override void ResetStats() {
            Item.damage = Item.OriginalDamage;
            CooldownTime = 110f;
            ShotsUntilCooldown = 24f;
        }
        public override UpgradeList InitializeUpgrades() {
            return new UpgradeList("SludgePump",
                new UpgradeTier(1,
                    new Upgrade("VisualCalculus", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Item_HoldItemHook = (item, player) => {
                                player.GetModPlayer<TracerRoundPlayer>().IsLayerAllowedToDraw = true;
                            }
                        }
                    },
                    new Upgrade("Glowstick", Assets.Upgrades.Heat.Value) {
                        Behavior = {
                            Projectile_PreDrawHook = (projectile, lightColor) => {
                                if (projectile.ModProjectile is not SludgeBall) return true;

                                Main.EntitySpriteDraw(new DrawData(TextureAssets.Projectile[projectile.type].Value, projectile.getRect(), Color.White));
                                return true;
                            },
                            Projectile_AIHook = (projectile) => {
                                if (projectile.ModProjectile is not SludgeBall) return;

                                Lighting.AddLight(projectile.position, new Vector3(0.05f, 0.9f, 0.05f));
                            }
                        }
                    }
                ),
                new UpgradeTier(2,
                    new Upgrade("MoreFragments", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is SludgeBall ball)
                                {
                                    ball.NumProjectilesToSpawn += 4;
                                }
                            }
                        }
                    }
                ),
                new UpgradeTier(3, 
                    new Upgrade("EfficientCharge", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is SludgeHelper helper)
                                {
                                    helper.ChargeShotCooldownMultiplier = 2f;
                                }
                            }
                        }
                    }
                ),
                new UpgradeTier(4,
                    new Upgrade("StrongerPoison", Assets.Upgrades.Heat.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (proj, target, info, damage) => {
                                
                            }
                        }
                    }
                ),
                new UpgradeTier(5,
                    new Upgrade("WasteOrdnance", Assets.Upgrades.Penetrate.Value) {
                        Behavior = {
                            Projectile_PreKillHook = (projectile, timeLeft) => {
                                if (projectile.ModProjectile is not SludgeBall ball) return true;
                                // Spawn the waste ordance if the projectile is fully charged
                                if (!ball.ShouldSplatter) return true;

                                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<SludgeExplosion>(), (int)(projectile.damage * 1.5f), 0f, Owner: projectile.owner);
                                return false;
                            }
                        }
                    }
                )
            );
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<SludgePump>())
            .AddIngredient(ItemID.HellstoneBar, 15)
            .AddIngredient(ItemID.Gel, 50)
            .AddIngredient(ItemID.Bone, 15)
            .AddTile(TileID.Solidifier)
            .Register();
        }
    }
}