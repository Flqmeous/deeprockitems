using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using deeprockitems.Content.Items.Weapons;
using System.IO;
using MonoMod.Utils;
using System.Linq;

namespace deeprockitems
{
    public class DRGTextures : ModSystem
    {
        private static readonly string PATH = "deeprockitems/Assets/";
        private static Texture2D RequestTexture(string fileName)
        {
            return ModContent.Request<Texture2D>(PATH + fileName, AssetRequestMode.ImmediateLoad).Value;
        }
        public static Dictionary<string, Asset<Texture2D>> UpgradeIcons = new();
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
            StunTwinkle = RequestTexture("StunTwinkle");

            // Load item icons
            WeaponIconography = new();
            WeaponIconography.Add<Zhukovs>("ZhukovsIcon");
            WeaponIconography.Add<SludgePump>("SludgePumpIcon");
            WeaponIconography.Add<M1000>("M1000Icon");
            WeaponIconography.Add<JuryShotgun>("JuryShotgunIcon");
            WeaponIconography.Add<PlasmaPistol>("PlasmaPistolIcon");

            // Set upgrade icons

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
        public static Texture2D StunTwinkle { get; private set; }
        public static Dictionary<int, Asset<Texture2D>> WeaponIconography { get; private set; }
    }
    public static class Extensions
    {
        private static readonly string PATH = "deeprockitems/Assets/";
        public static void Add<T>(this Dictionary<int, Asset<Texture2D>> dictionary, string fileName) where T : ModItem
        {
            int itemType = ModContent.ItemType<T>();
            dictionary.Add(itemType, ModContent.Request<Texture2D>(PATH + fileName, AssetRequestMode.ImmediateLoad));
        }
    }
}
