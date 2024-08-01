using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades.JuryShotgunUpgrades
{
    [ValidWeapons(typeof(JuryShotgun))]
    public class StuffedShellsOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<StuffedShellsOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddRecipeGroup(nameof(ItemID.CobaltBar), 10)
            .AddRecipeGroup(nameof(ItemID.VilePowder), 25)
            .AddIngredient(ItemID.MusketBall, 100)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();
        }
        public override void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem)
        {
            // Will always be true
            if (modItem is JuryShotgun shotgun)
            {
                shotgun.SpreadMultiplier = 2f;
                shotgun.ProjectileMultiplier = 2f;
            }
        }
    }
}
