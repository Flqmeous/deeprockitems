using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deeprockitems.Common.Quests
{
    public class QuestData
    {
        public QuestID QuestType { get; set; }
        public int TypeRequired { get; set; }
        public int AmountRequired { get; set; }
        public bool Hardmode { get; set; }
        public QuestData(QuestID questType, int typeRequired, int amountRequired, bool hardmode)
        {
            QuestType = questType;
            TypeRequired = typeRequired;
            AmountRequired = amountRequired;
            Hardmode = hardmode;
        }

        public Quest CreateQuestFromThis()
        {
            return new Quest(QuestType, this);
        }
    }
}
