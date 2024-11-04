using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Common.Quests
{
    public class QuestItemPickupTracker : GlobalItem
    {
        private int old_stack = 0;
        public override bool InstancePerEntity => true;
        public override void UpdateInventory(Item item, Player player)
        {
            // Convert our player to a ModPlayer
            QuestModPlayer modPlayer = player.GetModPlayer<QuestModPlayer>();
            if (modPlayer is null) return; // Return if null.


            if (old_stack < item.stack)
            {
                old_stack = item.stack;
                if (modPlayer.ActiveQuest != null && (modPlayer.ActiveQuest.Type == QuestID.Gathering || modPlayer.ActiveQuest.Type == QuestID.Mining) && modPlayer.ActiveQuest.Data.TypeRequired == item.type)
                {
                    modPlayer.UpdateQuestProgress(item.stack);
                }
            }

        }
    }
}
