using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.M1000Upgrades;
using deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades.JuryShotgunUpgrades
{
    [ValidWeapons(typeof(JuryShotgun))]
    public class PelletAlignmentOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<PelletAlignmentOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.MeteoriteBar, 10)
            .AddRecipeGroup(nameof(ItemID.VilePowder), 15)
            .AddIngredient(ItemID.CelestialMagnet)
            .AddTile(TileID.Anvils)
            .Register();
        }
        public override void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem)
        {
            // Always true
            if (modItem is JuryShotgun shotgun)
            {
                shotgun.SpreadMultiplier = 0.5f;
                shotgun.VelocityLowerBound = 1f;
            }
        }
    }
}
