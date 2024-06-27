using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace deeprockitems.Common.Quests
{
    public class Quest
    {
        /// <summary>
        /// The ID of the icon to display in the bottom right of the Quest textbox.
        /// </summary>
        public int ItemIcon { get; set; }
        /// <summary>
        /// The units of this quest completed.
        /// </summary>
        public int Progress { get; set; }
        /// <summary>
        /// Whether the class has been completed. Shorthand for <c><see cref="Progress"/> == <see cref="AmountRequired"/></c>
        /// </summary>
        public bool Completed { get => Progress == Data.AmountRequired; }
        /// <summary>
        /// Whether the quest has been rewarded to the player this quest refers to.
        /// </summary>
        public bool HasQuestBeenRewarded { get; set; }
        /// <summary>
        /// The ID of this quest.
        /// </summary>
        public QuestID Type { get; set; }
        public QuestData Data { get; set; }
        public Quest(QuestID id, QuestData data)
        {
            Type = id;
            Data = data;
        }
        public Quest(QuestID id, int typeToQuestFor, int amountRequired, bool hardmode)
        {
            Type = id;
            Data = new QuestData(id, typeToQuestFor, amountRequired, hardmode);
        }
    }
    public enum QuestID
    {
        None = 0,
        Mining = 1,
        Gathering = 2,
        Fighting = 3,
    }
}
