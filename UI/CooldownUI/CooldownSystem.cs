using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI.CooldownUI
{
    public class CooldownSystem : ModSystem
    {
        public PlayerCooldownUIState CooldownState;
        public UserInterface Interface;
        public override void Load() {
            if (Main.dedServ)
            {
                return;
            }
            CooldownState = new();
            CooldownState.Activate();
            Interface = new();
            Interface.SetState(CooldownState);
        }
        public override void UpdateUI(GameTime gameTime) {
            Interface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int mouseTextIndex = layers.FindIndex((layer) => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex == -1) return;
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("deeprockitems: WeaponCooldownUI",
                                                                       () => {
                                                                           Interface.Draw(Main.spriteBatch, new GameTime());
                                                                           return true;
                                                                       },
                                                                       InterfaceScaleType.UI));
        }
    }
}
