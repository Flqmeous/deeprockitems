using Terraria;
using Terraria.ModLoader;
using Mono;
using MonoMod;
using MonoMod.Cil;
using Terraria.UI;
using Mono.Cecil.Cil;
using System.Reflection;
using System.Collections.Generic;

namespace deeprockitems.UI
{
    public class InterfaceBlocker : ModSystem
    {
        public static bool BlockItemSlotLogic { get; set; }
        public override void Load()
        {
            IL_Main.DrawInventory += IL_Main_DrawInventory;
        }

        private void IL_Main_DrawInventory(ILContext il)
        {
            try
            {
                // Create new cursor for each iteration
                var cursor = new ILCursor(il);
                bool reachedEndOfMethod = false;
                // Find every "Player.IgnoreMouseInterface" and chain onto the method
                do
                {
                    // Navigate to ignoreMouseInterface
                    var propReference = typeof(Terraria.GameInput.PlayerInput).GetProperty(nameof(Terraria.GameInput.PlayerInput.IgnoreMouseInterface), BindingFlags.Public | BindingFlags.Static).GetMethod;
                    if (cursor.TryGotoNext(i => i.MatchCall(propReference)))
                    {
                        // Go back 1 index to allow Next.Operand
                        cursor.Index--;
                        // Get the label to the non-branched code
                        if (cursor.Next.Operand is ILLabel oldLabel)
                        {
                            // Create new label
                            var newLabel = cursor.DefineLabel();
                            // Navigate to the end of the previous brtrue
                            cursor.Index += 3;
                            // Emit
                            cursor.EmitCall(typeof(InterfaceBlocker).GetProperty(nameof(BlockItemSlotLogic), BindingFlags.Public | BindingFlags.Static).GetMethod);
                            cursor.Emit(OpCodes.Brtrue_S, newLabel);

                            cursor.GotoNext(i => i.Equals(oldLabel.Target.Previous));
                            cursor.MarkLabel(newLabel);
                            // Continue again
                            continue;
                        }
                    }
                    // Exit the loop
                    reachedEndOfMethod = true;

                } while (!reachedEndOfMethod);
            }
            catch
            {
                MonoModHooks.DumpIL(Mod, il);
            }
        }
        public override void PostUpdateEverything()
        {
            BlockItemSlotLogic = false;
        }
    }
}
