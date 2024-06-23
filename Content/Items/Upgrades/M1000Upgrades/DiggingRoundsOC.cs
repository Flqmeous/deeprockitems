﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace deeprockitems.Content.Items.Upgrades.M1000Upgrades
{
    public class DiggingRoundsOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<DiggingRoundsOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.MythrilBar, 10)
            .AddIngredient(ItemID.MusketBall, 30)
            .AddIngredient(ItemID.ScarabBomb, 5)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<DiggingRoundsOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.OrichalcumBar, 10)
            .AddIngredient(ItemID.MusketBall, 30)
            .AddIngredient(ItemID.ScarabBomb, 5)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();
        }
        public override void ProjectileOnSpawn(Projectile projectile, IEntitySource source)
        {
            projectile.tileCollide = false;
        }
    }
}
