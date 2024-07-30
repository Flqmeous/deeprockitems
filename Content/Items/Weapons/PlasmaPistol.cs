﻿using deeprockitems.Content.Items.Upgrades;
using deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class PlasmaPistol : UpgradeableItemTemplate
    {
        public override void NewSetDefaults()
        {
            Item.damage = 15;
            Item.rare = ItemRarityID.Green;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.knockBack = 4;
            Item.crit = 4;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 18f;
            Item.channel = true;
            Item.noMelee = true;
            Item.height = 28;
            Item.width = 30;

            Item.value = Item.sellPrice(0, 1, 60, 0);

            ValidUpgrades.Add(ModContent.ItemType<QuickCharge>());
            ValidUpgrades.Remove(ModContent.ItemType<Blowthrough>());
            ValidUpgrades.Add(ModContent.ItemType<ColdPlasmaOC>());
            ValidUpgrades.Add(ModContent.ItemType<PersistantPlasmaOC>());
            ValidUpgrades.Add(ModContent.ItemType<HeavyHitterOC>());
            ValidUpgrades.Add(ModContent.ItemType<ThinContainmentField>());
            ValidUpgrades.Add(ModContent.ItemType<PlasmaSplash>());
            ValidUpgrades.Add(ModContent.ItemType<OvertunedNozzle>());
        }
        public override void ModifyShootPrimaryUse(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<Projectiles.PlasmaProjectiles.PlasmaPistolHelper>();
        }
        public override void ResetStats()
        {
            Item.channel = true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<PlasmaPistol>())
                .AddRecipeGroup(nameof(ItemID.GoldBar), 10)
                .AddIngredient(ItemID.Amethyst, 8)
                .AddIngredient(ItemID.FallenStar, 5)
            .Register();
        }
    }
}