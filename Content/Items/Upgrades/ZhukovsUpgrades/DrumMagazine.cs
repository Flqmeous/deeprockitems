using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using Microsoft.Xna.Framework;
using System;

namespace deeprockitems.Content.Items.Upgrades.ZhukovsUpgrades
{
    [ValidWeapons(typeof(Zhukovs))]
    public class DrumMagazine : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<DrumMagazine>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.AdamantiteBar), 10)
            .AddIngredient(ItemID.ExplosivePowder, 10)
            .AddIngredient(ItemID.MusketBall, 99)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
        public override void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem)
        {
            //modItem.Item.useAnimation = (int)Math.Ceiling(modItem.Item.useAnimation);
            modItem.Item.useTime = (int)Math.Ceiling(modItem.Item.useTime * 0.45f);
        }
    }
}
