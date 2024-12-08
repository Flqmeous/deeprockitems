using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Projectiles.NishankaBoltsharkProjectiles;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons {
    public class NishankaBoltshark : UpgradableWeapon {
        public int shotCounterMaximum = 5;
        private int shotCounter = 0;
        public int shotCounterCooldown = 90;
        private int shotCounterTimer = 0;
        private float _critChance = 0.0f;

        public override void NewSetDefaults() {
            Item.width = 52;
            Item.height = 20;
            Item.damage = 15;
            Item.knockBack = 1.5f;
            Item.noMelee = true;
            Item.useAnimation = Item.useTime = 30;
            Item.shootSpeed = 9f;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false; // set to false and have true as an upgrade, this is just set as it is for now because it's nicer to my finger

            this.ShotsUntilCooldown = 14.9f; // 15 gives 16 shots so this is bad solution
            this.TimeToEndCooldown = 90;
        }

        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            Vector2 offset = Vector2.Normalize(velocity) * 10f;
            if (Collision.CanHit(position, 0, 0, position + offset, 0, 0)) {
                position += offset;
            }
        }

        public override bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            shotCounter = (shotCounter % shotCounterMaximum) + 1;
            shotCounterTimer = 0;
            if (shotCounter == shotCounterMaximum) {
                velocity *= 2f;
                damage += (int)(2 * _critChance);
                knockback += 2.0f;
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SpecialBolt>(), damage, knockback); // Fire custom bolt
                SoundEngine.PlaySound(SoundID.Item11 with { Pitch = -0.6f }, position);
                return false;
            }
            SoundEngine.PlaySound(SoundID.Item5, position);
            return true;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit) {
            base.ModifyWeaponCrit(player, ref crit);
            _critChance = crit;
        }

        public override void UpdateInventory(Player player) {
            base.UpdateInventory(player);

            if (shotCounter != 0) {
                shotCounterTimer = (shotCounterTimer % shotCounterCooldown) + 1;
                if (shotCounterTimer == shotCounterCooldown) {
                    shotCounter = 0;
                }
            } else { shotCounterTimer = 0; }
        }

        public override Vector2? HoldoutOffset() {
            return new Vector2(-3f, 0f);
        }

        public override void AddRecipes() {
            Recipe.Create(ModContent.ItemType<NishankaBoltshark>())
                .AddRecipeGroup(RecipeGroupID.IronBar, 10)
                .AddIngredient(ItemID.AshBlock, 20)
                .AddIngredient(ItemID.WoodenBow, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public override UpgradeList InitializeUpgrades() {
            // upgrade ideas oops you didn't see this: alt shot but higher cooldown / 3rd shot but higher heat-up / no cooldown / auto-reuse
            return new UpgradeList("NishankaBoltshark",
                new UpgradeTier(1,
                    new Upgrade("Automatic", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.autoReuse = true;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                               .AddCandidateIngredient([ItemID.AaronsBreastplate, ItemID.ZombieMermanBanner], 1)
                    },
                    new Upgrade("T1", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage += 1000000; // a little silly, a tiny bit goofy, perhaps somewhat whimsical
                            }
                        },
                        Recipe = new UpgradeRecipe()
                               .AddCandidateIngredient([ItemID.AaronsBreastplate, ItemID.ZombieMermanBanner], 1)
                    }
                ),
                new UpgradeTier(2,
                    new Upgrade("T2", Assets.Upgrades.TemperatureDecrease.Value) {
                        Behavior = {
                            /**/
                        },
                        Recipe = new UpgradeRecipe()
                                .AddCandidateIngredient([ItemID.AaronsBreastplate, ItemID.ZombieMermanBanner], 1)
                    }
                ),
                new UpgradeTier(3,
                    new Upgrade("T3", Assets.Upgrades.TemperatureDecrease.Value) {
                        Behavior = {
                            /**/
                        },
                        Recipe = new UpgradeRecipe()
                                .AddCandidateIngredient([ItemID.AaronsBreastplate, ItemID.ZombieMermanBanner], 1)
                    }
                ),
                new UpgradeTier(4,
                    new Upgrade("T4", Assets.Upgrades.TemperatureDecrease.Value) {
                        Behavior = {
                            /**/
                        },
                        Recipe = new UpgradeRecipe()
                                .AddCandidateIngredient([ItemID.AaronsBreastplate, ItemID.ZombieMermanBanner], 1)
                    }
                ),
                new UpgradeTier(5,
                    new Upgrade("T5", Assets.Upgrades.TemperatureDecrease.Value) {
                        Behavior = {
                            /**/
                        },
                        Recipe = new UpgradeRecipe()
                                .AddCandidateIngredient([ItemID.AaronsBreastplate, ItemID.ZombieMermanBanner], 1)
                    }
                )
            );

        }

    }
}
