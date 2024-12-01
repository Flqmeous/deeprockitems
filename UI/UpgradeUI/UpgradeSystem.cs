using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeSystem : ModSystem
    {
        public UpgradeState UpgradeUIState;
        public UserInterface Interface;
        public static bool IsUIOpen { get => ModContent.GetInstance<UpgradeSystem>().Interface.CurrentState != null; }
        public static void SetState(UIState state) {
            // Set state
            var self = ModContent.GetInstance<UpgradeSystem>();
            self.Interface.SetState(state);
        }   
        public override void Load()
        {
            UpgradeUIState = new();
            UpgradeUIState.Activate();
            Interface = new();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            Interface?.Update(gameTime);
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
