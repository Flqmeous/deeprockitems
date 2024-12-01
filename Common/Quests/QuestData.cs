using System;
using Terraria.ModLoader.IO;

namespace deeprockitems.Common.Quests
{
    public class QuestData : TagSerializable
    {
        public QuestID QuestType { get; set; }
        public int TypeRequired { get; set; }
        public int AmountRequired { get; set; }
        public bool Predicate { get; set; }
        public QuestData(QuestID questType, int typeRequired, int amountRequired, bool predicate)
        {
            QuestType = questType;
            TypeRequired = typeRequired;
            AmountRequired = amountRequired;
            Predicate = predicate;
        }

        public Quest CreateQuestFromThis()
        {
            return new Quest(QuestType, this);
        }

        public TagCompound SerializeData()
        {
            return new TagCompound()
            {
                ["QuestID"] = (int)QuestType,
                ["TypeRequired"] = TypeRequired,
                ["AmountRequired"] = AmountRequired,
                ["QuestPredicate"] = Predicate,
            };
        }
        public static QuestData Load(TagCompound tag)
        {
            // load
            QuestID id = (QuestID)tag["QuestID"];
            int type = (int)tag["TypeRequired"];
            int amount = (int)tag["AmountRequired"];
            bool hardmode = tag.GetBool("QuestPredicate");
            return new(id, type, amount, hardmode);
        }
        public static Func<TagCompound, QuestData> DESERIALIZER = Load;
    }
}
