using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades
{
    [ValidWeapons(typeof(SludgePump))]
    public class TracerRounds : UpgradeTemplate
    {
        public int _sludgeTracerTimer = 0;
        public static int MAX_TIMER = 30;
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<TracerRounds>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.GoldBar), 10)
            .AddIngredient(ItemID.Gel, 10)
            .AddIngredient(ItemID.RottenChunk, 5)
            .AddTile(TileID.Anvils)
            .Register();

            Recipe.Create(ModContent.ItemType<TracerRounds>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.GoldBar), 10)
            .AddIngredient(ItemID.Gel, 10)
            .AddIngredient(ItemID.Vertebrae, 5)
            .AddTile(TileID.Anvils)
            .Register();
        }
        public override void ItemHold(UpgradeableItemTemplate sender, Player player)
        {/*
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Normalize(Main.MouseWorld - player.Center) * Item.shootSpeed, ModContent.ProjectileType<ProjectileTracer>(), 0, 0, ai0: _sludgeTracerTimer, ai1: sender.Upgrades[^1]);
            _sludgeTracerTimer++;
            if (_sludgeTracerTimer > MAX_TIMER)
            {
                _sludgeTracerTimer = 0;
            }*/
        }
    }
}
