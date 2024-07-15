using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace deeprockitems.Common.Quests
{
    public class Quest : TagSerializable
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
        public bool Completed { get => Progress >= Data.AmountRequired; }
        /// <summary>
        /// Whether the quest has been rewarded to the player this quest refers to.
        /// </summary>
        public bool HasQuestBeenRewarded { get; set; }
        /// <summary>
        /// The ID of this quest.
        /// </summary>
        public QuestID Type { get; set; }
        public QuestData Data { get; set; }
        public Quest(QuestID id, QuestData data, int progress = 0, bool questRewarded = false)
        {
            Type = id;
            Data = data;
            Progress = progress;
            HasQuestBeenRewarded = questRewarded;
            SetCornerItem(id);
        }
        public Quest(QuestID id, int typeToQuestFor, int amountRequired, bool predicate, int progress = 0, bool questRewarded = false)
        {
            Type = id;
            Data = new QuestData(id, typeToQuestFor, amountRequired, predicate);
            Progress = progress;
            HasQuestBeenRewarded = questRewarded;
            SetCornerItem(id);
        }
        private void SetCornerItem(QuestID id)
        {
            ItemIcon = id switch
            {
                QuestID.Mining => ItemID.IronPickaxe,
                QuestID.Gathering => ItemID.StaffofRegrowth,
                QuestID.Fighting => ItemID.CopperShortsword,
                _ => ItemID.None,
            };
        }

        public TagCompound SerializeData()
        {
            return new TagCompound()
            {
                ["Progress"] = Progress,
                ["QuestID"] = (int)Type,
                ["QuestData"] = Data,
                ["QuestWasRewarded"] = HasQuestBeenRewarded,
            };
        }
        public static Quest Load(TagCompound tag)
        {
            int progress = (int)tag["Progress"];
            QuestID id = (QuestID)tag["QuestID"];
            QuestData data = tag.Get<QuestData>("QuestData");
            bool rewarded = tag.GetBool("QuestWasRewarded");
            return new(id, data, progress, rewarded);
        }
        public static readonly Func<TagCompound, Quest> DESERIALIZER = Load;
    }
    public enum QuestID
    {
        None = 0,
        Mining = 1,
        Gathering = 2,
        Fighting = 3,
    }
}
