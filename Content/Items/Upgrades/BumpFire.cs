﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades
{
    [ValidWeapons(
        typeof(M1000),
        typeof(JuryShotgun))]
    public class BumpFire : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<BumpFire>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.HellstoneBar, 15)
            .AddIngredient(ItemID.IllegalGunParts)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
