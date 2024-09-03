using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeSystem : ModSystem
    {
        public UpgradeState UpgradeUIState;
        public UserInterface Interface;
        public override void Load()
        {
            UpgradeUIState = new();
            UpgradeUIState.Activate();
            Interface = new();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            Interface?.Update(gameTime);
            if (!Main.playerInventory)
            {
                Interface.SetState(null);
            }
            if (UpgradeUIState.Panel.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex((layer) => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex == -1) return;
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("deeprockitems: UpgradeStationUI",
                                                                       () =>
                                                                       {
                                                                           Interface.Draw(Main.spriteBatch, new GameTime());
                                                                           return true;
                                                                       },
                                                                       InterfaceScaleType.UI));
        }
    }
}
