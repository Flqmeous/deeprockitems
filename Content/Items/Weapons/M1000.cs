using deeprockitems.Assets.Textures;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles;
using deeprockitems.Content.Projectiles.M1000Projectile;
using deeprockitems.Content.Upgrades;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class M1000 : UpgradeableItemTemplate
    {
        private int original_projectile;
        public override void NewSetDefaults()
        {
            Item.damage = 45;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.knockBack = 7.75f;
            Item.crit = 17;
            Item.width = 60;
            Item.height = 12;
            Item.useAmmo = AmmoID.Bullet;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10f;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 9, 25, 0);
            Item.consumable = false;
        }
        public float AmmoChance { get; set; } = 1f;
        public override bool CanConsumeAmmo(Item ammo, Player player) {
            return Main.rand.NextBool((int)(100 * AmmoChance), 100);
        }
        public override UpgradeList InitializeUpgrades()
        {
            return new UpgradeList("M1000",
                new UpgradeTier(1,
                    new Upgrade("DamageUpgrade", DRGTextures.DamageIcon) {
                        Item_ModifyStats = () => {
                            Item.damage = (int)(Item.OriginalDamage * 1.15f);
                        }
                    },
                    new Upgrade("BumpFire", DRGTextures.DamageIcon) {
                        Item_ModifyStats = () => {
                            Item.useTime = Item.useAnimation = (int)(Item.useTime * 0.83f);
                        }
                    }
                ),
                new UpgradeTier(2,
                    new Upgrade("QuickCharge", DRGTextures.DamageIcon) {
                        Projectile_OnSpawnHook = (projectile, source) => {
                            if (projectile.ModProjectile is not HeldProjectileBase modProj) return;

                            modProj.ChargeTime = (int)(modProj.ChargeTime * 0.75f);
                        }
                    },
                    new Upgrade("BiggerClip", DRGTextures.DamageIcon) {
                        Item_ModifyStats = () => {
                            AmmoChance = 0.5f;
                        }
                    }
                ),
                new UpgradeTier(3,
                    new Upgrade("FocusDamage", DRGTextures.DamageIcon) {
                        Projectile_OnSpawnHook = (projectile, source) => {
                            if (source is not EntitySource_FromHeldProjectile newSource) return;

                            if (newSource.SourceProjectile.Projectile.timeLeft >= newSource.SourceProjectile.ProjectileTime) return;

                            projectile.damage = (int)(projectile.damage * 1.5f);
                        } 
                    },
                    new Upgrade("DamageUpgrade", DRGTextures.DamageIcon) {
                        Item_ModifyStats = () => {
                            Item.damage = (int)(Item.damage * 1.15f);
                        }
                    }
                ),
                new UpgradeTier(4,
                    new Upgrade("Blowthrough", DRGTextures.DamageIcon) {
                        Projectile_OnSpawnHook = (projectile, source) => {
                            projectile.penetrate = projectile.maxPenetrate = 5;
                        }
                    },
                    new Upgrade("DiggingRounds", DRGTextures.DamageIcon) {
                        Projectile_OnSpawnHook = (projectile, source) => {
                            projectile.tileCollide = false;
                        }
                    }
                ),
                new UpgradeTier(5,
                    new Upgrade("IncendiaryRounds", DRGTextures.DamageIcon) {
                        Projectile_OnHitNPCHook = (projectile, target, hitInfo, damageDone) => {
                            target.AddBuff(BuffID.OnFire3, 120);    
                        }
                    },
                    new Upgrade("HollowPointRounds", DRGTextures.DamageIcon) {
                        Projectile_OnHitNPCHook = (projectile, target, hitInfo, damageDone) => {
                            target.AddBuff(ModContent.BuffType<StunnedEnemy>(), 120);
                        }
                    }
                )
            );
        }
        public override void ResetStats()
        {
            Item.damage = Item.OriginalDamage;
            AmmoChance = 1f;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Store the projectile that would've been shot.
            original_projectile = type;

            // Set type to be the "helper" projectile.
            type = ModContent.ProjectileType<M1000Helper>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback);
            if (proj.ModProjectile is HeldProjectileBase modProj)
            {
                // Make the helper spawn the original projectile when it despawns/dies, to make it look like the original projectile was shot.
                modProj.ProjectileToSpawn = original_projectile;
                // Replace musket balls with high-velocity bullet
                if (original_projectile == ProjectileID.Bullet)
                {
                    modProj.ProjectileToSpawn = ProjectileID.BulletHighVelocity;
                }

                // Sorry, until this weird gravity issue gets fixed: No modded bullets!
                if (!ModInformation.IsProjectileVanilla(original_projectile) && !ModInformation.IsProjectileMyMod(original_projectile))
                {
                    modProj.ProjectileToSpawn = ProjectileID.BulletHighVelocity;
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<M1000>())
            .AddIngredient(ItemID.Musket, 1)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddRecipeGroup(nameof(ItemID.CobaltBar), 20)
            .AddIngredient(ItemID.SoulofNight, 15)
            .Register();

            Recipe.Create(ModContent.ItemType<M1000>())
            .AddIngredient(ItemID.TheUndertaker, 1)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddRecipeGroup(nameof(ItemID.CobaltBar), 20)
            .AddIngredient(ItemID.SoulofNight, 15)
            .Register();
        }
    }
}