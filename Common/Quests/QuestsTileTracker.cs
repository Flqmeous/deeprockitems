using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Common.Items;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Collections.Generic;

namespace deeprockitems.Common.Quests
{
    public class TileTrackerIL : ModSystem
    {
        public override void Load()
        {
            IL_Player.PickTile += PickTileHook;
        }
        public delegate void PlayerPickedTileHandler(int i, int j, Player player);
        public static event PlayerPickedTileHandler OnPlayerMineTile;
        private void PickTileHook(ILContext il)
        {
            try
            {
                // Create cursor
                var cursor = new ILCursor(il);

                // Navigate cursor. I'm putting this edit just before local 4 is stored
                cursor.GotoNext((i) => i.MatchStloc(4));

                // Emit values for delegate to stack
                cursor.EmitLdarg1(); // Emit tile x coord
                cursor.EmitLdarg2(); // Emit tile y coord   
                cursor.EmitLdarg0(); // Emit player

                // Insert my IL edit
                cursor.EmitDelegate((int i, int j, Player player) => {
                    // Get Modplayer
                    if (!player.TryGetModPlayer(out QuestModPlayer modPlayer)) return;

                    // If quest type is mining and this was requested, count up progress
                    if (modPlayer.ActiveQuest is not null && modPlayer.ActiveQuest.Type == QuestID.Mining && modPlayer.ActiveQuest.Data.TypeRequired == Main.tile[i, j].TileType)
                    {
                        modPlayer.AddProgressToQuest();
                    }
                });
            }
            catch (Exception ex)
            {
                Mod.Logger.Info("An error has occured. Dumping IL");
                MonoModHooks.DumpIL(Mod, il);
            }
        }
    }
}