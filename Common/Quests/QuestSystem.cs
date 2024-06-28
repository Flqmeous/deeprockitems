using Terraria;
using Terraria.ModLoader;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Terraria.WorldBuilding;
using Terraria.ID;
using Terraria.ModLoader.IO;
using System.IO;

namespace deeprockitems.Common.Quests
{
    public class QuestSystem : ModSystem
    {
        public QuestCollection Quests { get; set; }
        public QuestData CurrentQuest { get; set; }
        public override void NetSend(BinaryWriter writer)
        {
            // Write current quest
            writer.Write((int)CurrentQuest.QuestType); // Quest ID
            writer.Write(CurrentQuest.TypeRequired); // Quest type
            writer.Write(CurrentQuest.AmountRequired); // Amount required
            writer.Write(CurrentQuest.Hardmode); // Is quest hardmode
        }
        public override void NetReceive(BinaryReader reader)
        {
            // Net is FIFO. This is in the exact same order as above
            CurrentQuest = new((QuestID)reader.ReadInt32(),
                               reader.ReadInt32(),
                               reader.ReadInt32(),
                               reader.ReadBoolean());
        }
        public override void OnWorldLoad()
        {
            if (WorldGen.SavedOreTiers.Copper != -1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Quests = new QuestCollection()
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Copper, 75, false)
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Iron, 75, false)
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Silver, 50, false)
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Gold, 50, false)
                    .ChainAdd(QuestID.Mining, TileID.Hellstone, 50, false);
                if (WorldGen.SavedOreTiers.Cobalt != -1)
                {
                    Quests.Add(QuestID.Mining, WorldGen.SavedOreTiers.Cobalt, 50, true);
                    if (WorldGen.SavedOreTiers.Mythril != -1)
                    {
                        Quests.Add(QuestID.Mining, WorldGen.SavedOreTiers.Mythril, 35, true);
                        if (WorldGen.SavedOreTiers.Adamantite != -1)
                        {
                            Quests.Add(QuestID.Mining, WorldGen.SavedOreTiers.Adamantite, 25, true);
                        }
                    }
                }
            }
        }
        public override void SaveWorldData(TagCompound tag)
        {
            // EMERGENCY ONLY: Generate quest if all is null:
            if (CurrentQuest is null)
            {
                GenerateQuest();
            }
            // Save the world's current quest data
            tag["questType"] = (int)CurrentQuest.QuestType;
            tag["questFind"] = CurrentQuest.TypeRequired;
            tag["questAmount"] = CurrentQuest.AmountRequired;
            tag["questHardmode"] = CurrentQuest.Hardmode;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            // load the current quest data
            if (tag.ContainsKey("questType") && tag.ContainsKey("questFind") && tag.ContainsKey("questAmount") && tag.ContainsKey("questHardmode"))
            {
                CurrentQuest = new((QuestID)tag.GetAsInt("questType"), tag.GetAsInt("questFind"), tag.GetAsInt("questAmount"), tag.GetBool("questHardmode"));
            }
            else
            {
                // If failed to load (or is new world), generate a new quest to the world
                GenerateQuest();
            }
        }
        public override void PostUpdateTime()
        {
            // If it's early morning and this is on the host, OR the CurrentQuest is null
            if ((CurrentQuest is null) || (Main.time == 0d && Main.dayTime && Main.netMode != NetmodeID.MultiplayerClient))
            {
                // Create quest
                GenerateQuest();
            }

        }
        public void GenerateQuest()
        {
            QuestCollection questsToChooseFrom = Main.hardMode ? Quests : (QuestCollection)Quests.Where((q) => q.Hardmode);
            CurrentQuest = questsToChooseFrom.TakeRandom();
        }
    }
}
