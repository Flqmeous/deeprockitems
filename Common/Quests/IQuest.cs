using Terraria;
using Terraria.ID;

namespace deeprockitems.Common.Quests
{
    public interface IQuest
    {
        /// <summary>
        /// The ID of the icon to display in the bottom right of the Quest textbox.
        /// </summary>
        public int ItemIcon { get => ItemID.None; }
        /// <summary>
        /// The units of this quest completed.
        /// </summary>
        public int Progress { get; set; }
        /// <summary>
        /// Total amount of units required to complete this quest.
        /// </summary>
        public int AmountRequired { get; set; }
        /// <summary>
        /// Whether the class has been completed. Shorthand for <c><see cref="Progress"/> == <see cref="AmountRequired"/></c>
        /// </summary>
        public sealed bool Completed { get => Progress == AmountRequired; }
        /// <summary>
        /// Whether the quest has been rewarded to the player this quest refers to.
        /// </summary>
        public bool HasQuestBeenRewarded { get; set; }
        public void OnQuestProgressIncrease(Player player);
        /// <summary>
        /// The ID of this quest.
        /// </summary>
        public QuestTypeID QuestType => QuestTypeID.None;
        public enum QuestTypeID
        {
            None = 0,
            Mining = 1,
            Gathering = 2,
            Fighting = 3,
        }
    }
}
