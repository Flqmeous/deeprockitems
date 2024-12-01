using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Tiles
{
    public class UpgradeStationItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<UpgradeStation>());
        }
        public override void AddRecipes() {
            Recipe.Create(Type)
                .AddIngredient(ItemID.SunplateBlock, 20)
                .AddRecipeGroup(nameof(ItemID.GoldBar), 8)
                .AddIngredient(ItemID.Glass, 10)
                .Register();
        }
    }
}
