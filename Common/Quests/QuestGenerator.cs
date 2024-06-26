using Terraria;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace deeprockitems.Common.Quests
{
    public class QuestGenerator
    {
        public static Quest? GenerateQuest()
        {
            Quest.QuestTypeID questType = (Quest.QuestTypeID)Main.rand.Next(1, Enum.GetValues(typeof(Quest.QuestTypeID)).Length - 1);
            switch (questType)
            {
                case Quest.QuestTypeID.Mining:
                    break;
                case Quest.QuestTypeID.Gathering:
                    break;
                case Quest.QuestTypeID.Fighting:
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
