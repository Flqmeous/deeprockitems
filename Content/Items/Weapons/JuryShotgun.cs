using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using deeprockitems.UI;
using deeprockitems.Content.Items.Upgrades;
using deeprockitems.Utilities;
using deeprockitems.Content.Items.Upgrades.JuryShotgunUpgrades;
using System;

namespace deeprockitems.Content.Items.Weapons
{
    public class JuryShotgun : UpgradeableItemTemplate
    {
        public int oldFireRate = 0;
        public int newFireRate = 0;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeDefaults()
        {
            Item.CloneDefaults(ItemID.Boomstick);
            Item.damage = 16;
            Item.width = 40;
            Item.height = 16;
            Item.useTime = 39;
            Item.useAnimation = 39;
            Item.value = Item.sellPrice(0, 3, 0, 0);

            oldFireRate = Item.useTime;

            ValidUpgrades.Add(ModContent.ItemType<PelletAlignmentOC>());
            ValidUpgrades.Add(ModContent.ItemType<SpecialPowderOC>());
            ValidUpgrades.Add(ModContent.ItemType<StuffedShellsOC>());

            ValidUpgrades.Add(ModContent.ItemType<WhitePhosphorus>());
            ValidUpgrades.Add(ModContent.ItemType<BumpFire>());
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
        public override void ResetStats()
        {
            ProjectileMultiplier = 1f;
            SpreadMultiplier = 1f;
            VelocityLowerBound = 0.8f;
        }
        public override bool ShootPrimaryUse(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
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
            int numberProjectiles = 4 + Main.rand.Next(1, 3);
            double spread = Math.PI / 13;
            /*if (Upgrades.Contains(ModContent.ItemType<PelletAlignmentOC>())) // Reduced spread
            {
                spread *= .5;
            }
            else if (Upgrades.Contains(ModContent.ItemType<StuffedShellsOC>())) // twice amount of pellets, much more spread and lower firerate
            {
                numberProjectiles *= 2;
                spread *= 2;
            }*/
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
            Recipe JuryShotgun = Recipe.Create(ModContent.ItemType<JuryShotgun>());
            JuryShotgun.AddIngredient(ItemID.Boomstick, 1);
            JuryShotgun.AddIngredient(ItemID.IllegalGunParts);
            JuryShotgun.AddRecipeGroup(nameof(ItemID.DemoniteBar), 8);
            JuryShotgun.AddRecipeGroup(nameof(ItemID.VilePowder), 10);
            JuryShotgun.AddTile(TileID.Anvils);
            JuryShotgun.Register();
        }

        public override void UniqueUpgrades()
        {
            if (Overclock == ModContent.ItemType<PelletAlignmentOC>())
            {
                DamageScale = 1f;
                newFireRate = 39;
            }
            else if (Overclock == ModContent.ItemType<SpecialPowderOC>())
            {
                DamageScale = .75f;
                newFireRate = 39;
            }
            else if (Overclock == ModContent.ItemType<StuffedShellsOC>())
            {
                DamageScale = 1f;
                newFireRate = 50;
            }
            else
            {
                DamageScale = 1f;
                newFireRate = 39;
            }
            foreach (int i in Upgrades)
            {
                if (i == ModContent.ItemType<BumpFire>())
                {
                    Item.useAnimation = (int)Math.Ceiling(newFireRate * .83f);
                    Item.useTime = (int)Math.Ceiling(newFireRate * .83f);
                }
                else
                {
                    Item.useAnimation = oldFireRate;
                    Item.useTime = oldFireRate;
                }
            }


        }
    }
}