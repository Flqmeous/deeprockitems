using deeprockitems.UI.UpgradeUI;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace deeprockitems.Content.Tiles
{
    public class UpgradeStation : ModTile
    {
        const int HEIGHT = 3;
        const int WIDTH = 3;
        public override void SetStaticDefaults()
        {
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.addTile(Type);
        }
        public override bool RightClick(int i, int j)
        {
            // Open UI
            UpgradeSystem system = ModContent.GetInstance<UpgradeSystem>();
            if (system.Interface.CurrentState == null)
            {
                Main.playerInventory = true;
                system.Interface.SetState(system.UpgradeUIState);
            }
            else // Set state
            {
                system.Interface.SetState(null);
            }
            return true;
        }
    }
}
