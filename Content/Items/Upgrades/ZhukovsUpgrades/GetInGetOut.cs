using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using Microsoft.Xna.Framework;
using System;
using deeprockitems.Content.Buffs;

namespace deeprockitems.Content.Items.Upgrades.ZhukovsUpgrades
{
    [ValidWeapons(typeof(Zhukovs))]
    public class GetInGetOut : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<GetInGetOut>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.CobaltBar), 10)
            .AddIngredient(ItemID.SwiftnessPotion, 10)
            .AddIngredient(ItemID.HermesBoots, 99)
            .AddTile(TileID.MythrilAnvil)
            .Register();

            Recipe.Create(ModContent.ItemType<GetInGetOut>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.CobaltBar), 10)
            .AddIngredient(ItemID.SwiftnessPotion, 10)
            .AddIngredient(ItemID.FlurryBoots, 99)
            .AddTile(TileID.MythrilAnvil)
            .Register();

            Recipe.Create(ModContent.ItemType<GetInGetOut>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.CobaltBar), 10)
            .AddIngredient(ItemID.SwiftnessPotion, 10)
            .AddIngredient(ItemID.SailfishBoots, 99)
            .AddTile(TileID.MythrilAnvil)
            .Register();

            Recipe.Create(ModContent.ItemType<GetInGetOut>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.CobaltBar), 10)
            .AddIngredient(ItemID.SwiftnessPotion, 10)
            .AddIngredient(ItemID.SandBoots, 99)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
        public class GetInGetOutProjectile : UpgradeGlobalProjectile<GetInGetOut>
        {
            public override void UpgradeOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (target.life - damageDone <= 0)
                {
                    Main.player[projectile.owner].AddBuff(ModContent.BuffType<Haste>(), 240);
                }
            }
        }
    }
}
