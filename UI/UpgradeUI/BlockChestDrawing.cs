using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.UI;
using System.Linq;
using System;
using MonoMod.Utils;

namespace deeprockitems.UI.UpgradeUI
{
    public class BlockChestDrawing : ModSystem
    {
        public override void Load() {
            // Detours
            //On_Main.DrawBestiaryIcon += On_Main_DrawBestiaryIcon;
            //On_Main.DrawEmoteBubblesButton += On_Main_DrawEmoteBubblesButton;
            On_Main.DrawTrashItemSlot += On_Main_DrawTrashItemSlot;
            // IL
            //IL_Main.DrawBestiaryIcon += IL_Main_DrawBestiaryIcon;
            //IL_Main.DrawEmoteBubblesButton += IL_Main_DrawEmoteBubblesButton;
        }

        private void IL_Main_DrawEmoteBubblesButton(ILContext il) {
            try
            {
                ILCursor cursor = new(il);

                // This edit will remove lines 2-4, and allow an offset to be provided into the function
                // Move after inventory was stored to
                cursor.GotoNext(i => i.MatchLdcI4(450));

                // Get amount of lines to remove
                cursor.FindNext(out ILCursor[] cursors, i => i.MatchLdcI4(244));
                cursor.RemoveRange(cursors.First().Index - cursor.Index);

                // Next, allow an offset to be provided by loading the offset and adding.
                // X offset
                cursor.GotoNext(i => i.MatchLdcI4(534));
                cursor.Index++;
                cursor.EmitLdarg0();
                cursor.EmitAdd();
                // Y offset
                cursor.GotoNext(i => i.Match(OpCodes.Ldc_I4_4));
                cursor.Index++;
                cursor.EmitLdarg1();
                cursor.EmitAdd();
            }
            catch
            {
                MonoModHooks.DumpIL(Mod, il);
                Mod.Logger.Info("Exception found when modifying Main::DrawBestiaryIcon. Dumping IL.");
            }
        }

        private void IL_Main_DrawBestiaryIcon(ILContext il) {
            try
            {
                ILCursor cursor = new(il);

                // This edit will remove lines 2-4, and allow an offset to be provided into the function
                // Move after inventory was stored to
                cursor.GotoNext(i => i.MatchLdcI4(450));

                // Get amount of lines to remove
                cursor.FindNext(out ILCursor[] cursors, i => i.MatchLdcI4(244));
                cursor.RemoveRange(cursors.First().Index - cursor.Index);

                // Next, allow an offset to be provided by loading the offset and adding.
                // X offset
                cursor.GotoNext(i => i.MatchLdcI4(498));
                cursor.Index++;
                cursor.EmitLdarg0();
                cursor.EmitAdd();
                // Y offset
                cursor.GotoNext(i => i.Match(OpCodes.Ldc_I4_4));
                cursor.Index++;
                cursor.EmitLdarg1();
                cursor.EmitAdd();
            }
            catch
            {
                MonoModHooks.DumpIL(Mod, il);
                Mod.Logger.Info("Exception found when modifying Main::DrawBestiaryIcon. Dumping IL.");
            }
        }
        private void On_ShiftIcon(Action<int, int> orig) {
            int offX = 0;
            int offY = 0;
            // If my ui is open, move icon
            if (UpgradeSystem.IsUIOpen)
            {
                offY = 200;
            }
            //pivotTopLeftX = (int)((455 + pivotTopLeftX) - 56f * Main.inventoryScale * 2f);
            orig(offX, offY);
        }
        private void On_Main_DrawTrashItemSlot(On_Main.orig_DrawTrashItemSlot orig, int pivotTopLeftX, int pivotTopLeftY) {
            On_ShiftIcon(orig.CastDelegate<Action<int, int>>());
        }
        private void On_Main_DrawEmoteBubblesButton(On_Main.orig_DrawEmoteBubblesButton orig, int pivotTopLeftX, int pivotTopLeftY) {
            On_ShiftIcon(orig.CastDelegate<Action<int, int>>());
        }
        private void On_Main_DrawBestiaryIcon(On_Main.orig_DrawBestiaryIcon orig, int pivotTopLeftX, int pivotTopLeftY) {
            On_ShiftIcon(orig.CastDelegate<Action<int, int>>());
        }
    }
}
