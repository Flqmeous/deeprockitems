using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class Zhukovs : UpgradableWeapon
    {
        public override void NewSetDefaults()
        {
            Item.width = 52;
            Item.height = 46;
            Item.rare = ItemRarityID.Cyan;

            Item.damage = 28;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 12;
            Item.knockBack = 1f;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.consumeAmmoOnLastShotOnly = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(0, 6, 50, 0);
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<Zhukovs>())
                .AddIngredient(ItemID.PhoenixBlaster)
                .AddIngredient(ItemID.IllegalGunParts, 2)
                .AddIngredient(ItemID.SoulofNight, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override UpgradeList InitializeUpgrades() {
            return new UpgradeList("Zhukovs",
                new UpgradeTier(1,
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage = (int)(item.OriginalDamage * 1.10f);
                            }
                        }
                    },
                    new Upgrade("FireRate", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.useTime = (int)(item.useTime * 0.8f);
                                item.useAnimation = (int)(item.useAnimation * 0.8f);
                            }
                        }
                    }
                ),
                new UpgradeTier(4,
                    new Upgrade("FireRounds", Assets.Upgrades.Heat.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (proj, npc, hit, damage) => {
                                npc.ChangeTemperature(10, proj.owner);
                            }
                        }
                    },
                    new Upgrade("CryoRounds", Assets.Upgrades.Cryo.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (proj, npc, hit, damage) => {
                                npc.ChangeTemperature(-10, proj.owner);
                            }
                        }
                    }
                ),
                new UpgradeTier(5,
                    new Upgrade("GetInGetOut", Assets.Upgrades.Haste.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (proj, npc, hit, damage) => {
                                // Is the npc dead? return
                                if (npc.life > 0) return;
                                Main.player[proj.owner].AddBuff(ModContent.BuffType<Haste>(), 180);
                            }
                        }
                    },
                    new Upgrade("Blowthrough", Assets.Upgrades.Penetrate.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.penetrate > 5) return;
                                proj.penetrate = 5;
                            }
                        }
                    }
                )
            );
        }
        public override bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item41, player.Center);
            // Shoot with spread. Doing this here to preserve overclocks.
            Vector2 spreadVelocity = velocity.RotatedByRandom(MathHelper.Pi / 32);
            Projectile.NewProjectile(source, position, spreadVelocity, type, damage, knockback, Owner: player.whoAmI);
            return false;
        }
    }
}
