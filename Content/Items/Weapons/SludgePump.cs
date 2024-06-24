﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using deeprockitems.Content.Items.Upgrades;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using deeprockitems.Utilities;
using deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades;

namespace deeprockitems.Content.Items.Weapons
{
    public class SludgePump : UpgradeableItemTemplate
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeDefaults()
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
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 22f;
            Item.rare = ItemRarityID.Orange;

            Item.value = Item.sellPrice(0, 5, 30, 0);

            ValidUpgrades.Add(ModContent.ItemType<AntiGravOC>());
            ValidUpgrades.Add(ModContent.ItemType<SludgeExplosionOC>());
            ValidUpgrades.Add(ModContent.ItemType<GooSpecialOC>());

            ValidUpgrades.Add(ModContent.ItemType<QuickCharge>());
            ValidUpgrades.Add(ModContent.ItemType<TracerRounds>());

        }
        public override void ModifyShootPrimaryUse(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<SludgeHelper>();
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