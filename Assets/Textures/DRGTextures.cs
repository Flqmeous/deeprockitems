using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace deeprockitems.Assets.Textures
{
    public class DRGTextures : ModSystem
    {
        private static readonly string PATH = "deeprockitems/Assets/Textures/";
        private static Texture2D RequestTexture(string fileName)
        {
            return ModContent.Request<Texture2D>(PATH + fileName, AssetRequestMode.ImmediateLoad).Value;
        }
        public override void SetStaticDefaults()
        {
            InventorySlot = RequestTexture("InventorySlot");
            SlotOutline = RequestTexture("SlotOutline");
            Zhukovs = RequestTexture("ZhukovsHeld");
            WhitePixel = RequestTexture("WhitePixel");
            TracerHit = RequestTexture("TracerHit");
            ElectricityArc = RequestTexture("ElectricityArc");
            UpgradeSlot = RequestTexture("UpgradeSlot");
            UpgradeIcon = RequestTexture("UpgradeIcon");
            OverclockSlot = RequestTexture("OverclockSlot");

        }
        public static Texture2D InventorySlot { get; private set; }
        public static Texture2D SlotOutline { get; private set; }
        public static Texture2D Zhukovs { get; private set; }
        public static Texture2D WhitePixel { get; private set; }
        public static Texture2D TracerHit { get; private set; }
        public static Texture2D ElectricityArc { get; private set; }
        public static Texture2D UpgradeSlot { get; private set; }
        public static Texture2D UpgradeIcon { get; private set; }
        public static Texture2D OverclockSlot { get; private set; }
    }
}
