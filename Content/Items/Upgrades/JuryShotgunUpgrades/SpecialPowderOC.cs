using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.M1000Upgrades;
using deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace deeprockitems.Content.Items.Upgrades.JuryShotgunUpgrades
{
    [ValidWeapons(typeof(JuryShotgun))]
    public class SpecialPowderOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<SpecialPowderOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddRecipeGroup(nameof(ItemID.CobaltBar), 10)
            .AddRecipeGroup(nameof(ItemID.VilePowder), 30)
            .AddIngredient(ItemID.SoulofFlight, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
        const float SPEED_CAP = 15f;
        public override bool UpgradeItem_ShootPrimaryUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool callBase)
        {
            // Get adjusted mouse position
            Vector2 mousePos = Main.MouseWorld - player.Center;
            player.velocity -= Vector2.Normalize(mousePos) * 10; // Actually launch the player
            // Cap horizontal speed
            if (Math.Abs(player.velocity.X) > SPEED_CAP)
            {
                player.velocity.X = SPEED_CAP * Math.Sign(player.velocity.X);
            }
            // Cancel fall damage
            if (player.velocity.Y < 5f)
            {
                player.fallStart = (int)player.position.Y / 16;
            }
            return base.UpgradeItem_ShootPrimaryUse(sender, player, source, position, velocity, type, damage, knockback, out callBase);
        }
    }
}
