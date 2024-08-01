using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using deeprockitems.Common.Items;

namespace deeprockitems.Common.Quests
{
    public class QuestKillTracker : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void OnKill(NPC npc) // Used for fighting quests
        {
            // Convert our player to a ModPlayer
            QuestModPlayer modPlayer = Main.LocalPlayer.GetModPlayer<QuestModPlayer>();
            if (modPlayer is null) return; // Return if null.
            if (modPlayer.ActiveQuest is null) return;

            int bannerID = Item.NPCtoBanner(BannerID(npc.type, npc.netID)); // Fingers crossed, this converts the NPC id correctly...
            // Check if quest is fighting, if quest pertains to this NPC, and if the client got the kill
            if (modPlayer.ActiveQuest.Type == QuestID.Fighting && Item.NPCtoBanner(modPlayer.ActiveQuest.Data.TypeRequired) == bannerID && npc.lastInteraction == Main.myPlayer)
            {
                // Decrease progress
                modPlayer.UpdateQuestProgress(modPlayer.ActiveQuest.Progress + 1);
            }
        }
        private static int BannerID(int type, int netID)
        {
            if (netID >= -10)
            {
                return netID;
            }
            return type;
        }
    }
}
