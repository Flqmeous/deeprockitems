using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace deeprockitems.Assets.Textures
{
    public class DRGTextures : ModSystem
    {
        public override void SetStaticDefaults()
        {
            InventorySlot = ModContent.Request<Texture2D>("deeprockitems/Assets/Textures/InventorySlot", AssetRequestMode.ImmediateLoad).Value;
            SlotOutline = ModContent.Request<Texture2D>("deeprockitems/Assets/Textures/SlotOutline", AssetRequestMode.ImmediateLoad).Value;
            Zhukovs = ModContent.Request<Texture2D>("deeprockitems/Content/Items/Weapons/ZhukovsHeld", AssetRequestMode.ImmediateLoad).Value;
            WhitePixel = ModContent.Request<Texture2D>("deeprockitems/Assets/Textures/WhitePixel", AssetRequestMode.ImmediateLoad).Value;
        }
        public static Texture2D InventorySlot { get; private set; }
        public static Texture2D SlotOutline { get; private set; }
        public static Texture2D Zhukovs { get; private set; }
        public static Texture2D WhitePixel { get; private set; }
    }
}
