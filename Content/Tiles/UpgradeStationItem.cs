using Terraria.ModLoader;

namespace deeprockitems.Content.Tiles
{
    public class UpgradeStationItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<UpgradeStation>());
        }
    }
}
