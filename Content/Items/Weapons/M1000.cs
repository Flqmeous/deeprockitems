using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static System.Math;
using deeprockitems.Content.Items.Upgrades;
using deeprockitems.Content.Projectiles.M1000Projectile;
using deeprockitems.Utilities;
using Terraria.DataStructures;
using deeprockitems.Content.Projectiles;
using System;
using Microsoft.CodeAnalysis;
using deeprockitems.Content.Items.Upgrades.M1000Upgrades;
using deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades;

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



            ValidUpgrades.Add(ModContent.ItemType<HipsterOC>());
            ValidUpgrades.Add(ModContent.ItemType<DiggingRoundsOC>());
            ValidUpgrades.Add(ModContent.ItemType<SupercoolOC>());
            ValidUpgrades.Add(ModContent.ItemType<HollowPointRounds>());
            ValidUpgrades.Add(ModContent.ItemType<QuickCharge>());
            ValidUpgrades.Add(ModContent.ItemType<BumpFire>());


            
        }
        public override void ModifyShootPrimaryUse(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Store the projectile that would've been shot.
            original_projectile = type;

            // Set type to be the "helper" projectile.
            type = ModContent.ProjectileType<M1000Helper>();
        }
        public override bool ShootPrimaryUse(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
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
        public override void ResetStats()
        {
            Item.channel = true;
        }
    }
}