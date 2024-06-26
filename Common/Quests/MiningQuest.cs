using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Common.Quests
{
    public class MiningQuest : Quest
    {
        public override int ItemIcon => ItemID.IronPickaxe;
        public override QuestTypeID QuestType => QuestTypeID.Mining;

        
    }
    /*public class MiningQuestTracker : GlobalTile
    {
        public int WhoLastInteractedWithThisTile { get; set; } = -1;
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            // GOAL: Detect who last interacted with a tile
            // Pseudo code
            // If netmode is a client
            // Try and find out which player this is.
            // Write a packet containing the position of the tile and who interacted with it
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (Main.LocalPlayer.)

                    ModPacket packet = Mod.GetPacket();
                // Write x coordinate, y coordinate, and player.
                packet.Write(i); // X coord
                packet.Write(j); // Y coord
                packet.Write(Main.myPlayer);
            }

            // I can't think of a better way to do this, so i'm just giving the progress to the closest player
            // Use LINQ to get the distance from this tile to each player's center
            if (!fail && !effectOnly && Main.netMode != NetmodeID.MultiplayerClient)
            {
                var players = Main.player.Select((p) => p.Center.DistanceSQ(new Point(i, j).ToWorldCoordinates(8f, 8f))).OrderBy(v => v);
                Main.NewText(players.FirstOrDefault());
            }



            // If quest type is mining and player is in interaction range:
            if (modPlayer.Cur == 1 && modPlayer.Player.IsInTileInteractionRange(i, j, Terraria.DataStructures.TileReachCheckSettings.Pylons))
            {
                // Fixing all gem migraines in this section of code
                int subID = FixGems(i, j, modPlayer.CurrentQuestInformation[1]);
                // Essentially, test for gems
                if (subID > -1 && Main.tile[i, j].TileType == TileID.ExposedGems && Main.tile[i, j].TileFrameX > 16 * subID && Main.tile[i, j].TileFrameX < 16 * (subID + 1) && !effectOnly && !fail)
                {
                    modPlayer.CurrentQuestInformation[3]--;
                    QuestsBase.DecrementProgress(modPlayer);
                    noItem = true;
                    return;
                }
                else if (modPlayer.CurrentQuestInformation[1] == type && !effectOnly && !fail) // If the current quest tile is this tile, and the tile was mined, and the tile was killed.
                {
                    modPlayer.CurrentQuestInformation[3]--;
                    QuestsBase.DecrementProgress(modPlayer);
                    noItem = true;
                    return;
                }
            }
        }
    }*/
}
