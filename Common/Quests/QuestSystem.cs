using Terraria;
using Terraria.ModLoader;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Terraria.WorldBuilding;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace deeprockitems.Common.Quests
{
    public class QuestSystem : ModSystem
    {
        public QuestCollection Quests { get; set; }
        public QuestData CurrentQuest { get; set; }
        public override void OnWorldLoad()
        {
            if (WorldGen.SavedOreTiers.Copper != -1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Quests = new QuestCollection()
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Copper, 75, false)
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Iron, 75, false)
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Silver, 50, false)
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Gold, 50, false)
                    .ChainAdd(QuestID.Mining, TileID.Hellstone, 50, false)
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Cobalt, 50, true)
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Mythril, 35, true)
                    .ChainAdd(QuestID.Mining, WorldGen.SavedOreTiers.Adamantite, 25, true);
            }
        }
        public override void SaveWorldData(TagCompound tag)
        {
            // Save the world's current quest data
            tag["questType"] = CurrentQuest.QuestType;
            tag["questFind"] = CurrentQuest.TypeRequired;
            tag["questAmount"] = CurrentQuest.AmountRequired;
            tag["questHardmode"] = CurrentQuest.Hardmode;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            // load the current quest data
            if (tag.ContainsKey("questType") && tag.ContainsKey("questFind") && tag.ContainsKey("questAmount") && tag.ContainsKey("questHardmode"))
            {
                CurrentQuest = new((QuestID)(int)tag["questType"], (int)tag["questFind"], (int)tag["questAmount"], (bool)tag["questHardmode"]);
            }
            else
            {
                // If failed to load (or is new world), generate a new quest specific to this player
                int index = Main.rand.Next(0, Quests.Length);
                CurrentQuest = Quests[index];
            }
        }
        public override void PostUpdateTime()
        {
            // If it's early morning and this is on the host
            if (Main.time == 0d && Main.dayTime && Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Create a new quest! :D
                int index = Main.rand.Next(0, Quests.Length);
                CurrentQuest = Quests[index];
            }
        }
    }
}
