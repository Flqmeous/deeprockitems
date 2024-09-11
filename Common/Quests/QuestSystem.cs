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
            writer.Write(CurrentQuest.Predicate); // Predicate required to pull the quest
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
            RecalculateQuests();
        }
        public void RecalculateQuests()
        {
            Quests = [new QuestData(QuestID.Gathering, ItemID.Daybloom, 10, true)];
            RecalculateMiningQuests();
            RecalculateGatheringQuests();
            RecalculateFightingQuests();
        }
        private void RecalculateFightingQuests()
        {
            Quests.ChainAdd(QuestID.Fighting, NPCID.BlueSlime, 30, true)
                .ChainAdd(QuestID.Fighting, NPCID.DemonEye, 20, !Main.hardMode)
                .ChainAdd(QuestID.Fighting, NPCID.WanderingEye, 10, Main.hardMode)
                .ChainAdd(QuestID.Fighting, NPCID.Zombie, 20, true)
                .ChainAdd(QuestID.Fighting, NPCID.Skeleton, 20, true)
                .ChainAdd(QuestID.Fighting, NPCID.GreekSkeleton, 15, true)
                .ChainAdd(QuestID.Fighting, NPCID.GraniteFlyer, 15, true)
                .ChainAdd(QuestID.Fighting, NPCID.GraniteGolem, 15, true)
                .ChainAdd(QuestID.Fighting, NPCID.Medusa, 5, Main.hardMode)
                .ChainAdd(QuestID.Fighting, NPCID.Demon, 15, NPC.downedBoss2)
                .ChainAdd(QuestID.Fighting, NPCID.FireImp, 10, NPC.downedBoss2)
                .ChainAdd(QuestID.Fighting, NPCID.RedDevil, 10, NPC.downedMechBossAny)
                .ChainAdd(QuestID.Fighting, NPCID.CaveBat, 15, !Main.hardMode)
                .ChainAdd(QuestID.Fighting, NPCID.GiantBat, 10, Main.hardMode)
                .ChainAdd(QuestID.Fighting, NPCID.ArmoredSkeleton, 15, Main.hardMode)
                .ChainAdd(QuestID.Fighting, NPCID.SkeletonArcher, 15, Main.hardMode)
                .ChainAdd(QuestID.Fighting, NPCID.ChaosElemental, 5, Main.hardMode)
                .ChainAdd(QuestID.Fighting, NPCID.AngryBones, 15, NPC.downedBoss3)
                ;
        }
        private void RecalculateGatheringQuests()
        {
            // All 11 herbs and spices
            Quests.ChainAdd(QuestID.Gathering, ItemID.Daybloom, 10, true)
                .ChainAdd(QuestID.Gathering, ItemID.Moonglow, 10, true)
                .ChainAdd(QuestID.Gathering, ItemID.Waterleaf, 10, true)
                .ChainAdd(QuestID.Gathering, ItemID.Fireblossom, 10, true)
                .ChainAdd(QuestID.Gathering, ItemID.Deathweed, 10, true)
                .ChainAdd(QuestID.Gathering, ItemID.Blinkroot, 10, true)
                .ChainAdd(QuestID.Gathering, ItemID.Shiverthorn, 10, true)
                // Dye plants
                .ChainAdd(QuestID.Gathering, ItemID.YellowMarigold, 3, true)
                .ChainAdd(QuestID.Gathering, ItemID.OrangeBloodroot, 3, true)
                .ChainAdd(QuestID.Gathering, ItemID.TealMushroom, 3, true)
                .ChainAdd(QuestID.Gathering, ItemID.GreenMushroom, 3, true)
                .ChainAdd(QuestID.Gathering, ItemID.LimeKelp, 3, true)
                .ChainAdd(QuestID.Gathering, ItemID.SkyBlueFlower, 3, true)
                .ChainAdd(QuestID.Gathering, ItemID.BlueBerries, 3, true)
                .ChainAdd(QuestID.Gathering, ItemID.PinkPricklyPear, 3, true)
                // Shrooms
                .ChainAdd(QuestID.Gathering, ItemID.VileMushroom, 10, !WorldGen.crimson)
                .ChainAdd(QuestID.Gathering, ItemID.ViciousMushroom, 10, WorldGen.crimson)
                .ChainAdd(QuestID.Gathering, ItemID.GlowingMushroom, 25, true)
                .ChainAdd(QuestID.Gathering, ItemID.Mushroom, 20, true)
                ;
        }
        private void RecalculateMiningQuests()
        {
            if (WorldGen.SavedOreTiers.Copper != -1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Prehardmode tiered ores
                Quests.ChainAdd(QuestID.Mining, ItemID.CopperOre, 100, WorldGen.SavedOreTiers.Copper == TileID.Copper)
                    .ChainAdd(QuestID.Mining, ItemID.TinOre, 100, WorldGen.SavedOreTiers.Copper == TileID.Tin)
                    .ChainAdd(QuestID.Mining, ItemID.IronOre, 75, WorldGen.SavedOreTiers.Iron == TileID.Iron)
                    .ChainAdd(QuestID.Mining, ItemID.LeadOre, 75, WorldGen.SavedOreTiers.Iron == TileID.Lead)
                    .ChainAdd(QuestID.Mining, ItemID.SilverOre, 60, WorldGen.SavedOreTiers.Silver == TileID.Silver)
                    .ChainAdd(QuestID.Mining, ItemID.TungstenOre, 60, WorldGen.SavedOreTiers.Silver == TileID.Tungsten)
                    .ChainAdd(QuestID.Mining, ItemID.GoldOre, 60, WorldGen.SavedOreTiers.Gold == TileID.Gold)
                    .ChainAdd(QuestID.Mining, ItemID.PlatinumOre, 60, WorldGen.SavedOreTiers.Gold == TileID.Platinum)
                    // Evil ores
                    .ChainAdd(QuestID.Mining, ItemID.DemoniteOre, 75, NPC.downedBoss2 && !WorldGen.crimson)
                    .ChainAdd(QuestID.Mining, ItemID.CrimtaneOre, 75, NPC.downedBoss2 && WorldGen.crimson)
                    // Gems and hellstone
                    .ChainAdd(QuestID.Mining, ItemID.Hellstone, 50, NPC.downedBoss2)
                    .ChainAdd(QuestID.Mining, ItemID.Amethyst, 25, true)
                    .ChainAdd(QuestID.Mining, ItemID.Topaz, 25, true)
                    .ChainAdd(QuestID.Mining, ItemID.Sapphire, 20, true)
                    .ChainAdd(QuestID.Mining, ItemID.Emerald, 15, true)
                    .ChainAdd(QuestID.Mining, ItemID.Ruby, 10, true)
                    .ChainAdd(QuestID.Mining, ItemID.Diamond, 5, true)
                    .ChainAdd(QuestID.Mining, ItemID.Amber, 10, true)
                    // Hardmode tiered ores
                    .ChainAdd(QuestID.Mining, ItemID.CobaltOre, 50, WorldGen.SavedOreTiers.Cobalt == TileID.Cobalt)
                    .ChainAdd(QuestID.Mining, ItemID.MythrilOre, 35, WorldGen.SavedOreTiers.Mythril == TileID.Mythril)
                    .ChainAdd(QuestID.Mining, ItemID.AdamantiteOre, 25, WorldGen.SavedOreTiers.Adamantite == TileID.Adamantite)
                    .ChainAdd(QuestID.Mining, ItemID.PalladiumOre, 50, WorldGen.SavedOreTiers.Cobalt == TileID.Palladium)
                    .ChainAdd(QuestID.Mining, ItemID.OrichalcumOre, 35, WorldGen.SavedOreTiers.Mythril == TileID.Orichalcum)
                    .ChainAdd(QuestID.Mining, ItemID.TitaniumOre, 25, WorldGen.SavedOreTiers.Adamantite == TileID.Titanium)
                    // Chlorophyte
                    .ChainAdd(QuestID.Mining, ItemID.ChlorophyteOre, 40, NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);
            }
        }
        public override void SaveWorldData(TagCompound tag)
        {
            // EMERGENCY ONLY: Generate quest if all is null:
            if (CurrentQuest is null)
            {
                RecalculateQuests();
                GenerateQuest();
            }
            // Save the world's current quest data
            tag["questType"] = (int)CurrentQuest.QuestType;
            tag["questFind"] = CurrentQuest.TypeRequired;
            tag["questAmount"] = CurrentQuest.AmountRequired;
            tag["questHardmode"] = CurrentQuest.Predicate;
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
            if ((CurrentQuest is null) || (Main.time == 1d && Main.dayTime && Main.netMode != NetmodeID.MultiplayerClient))
            {
                // Review all possible quests:
                RecalculateQuests();
                // Create quest
                GenerateQuest();
            }

        }
        public void GenerateQuest()
        {
            // Reset quests for all
            foreach (Player player in Main.player)
            {
                if (!player.TryGetModPlayer(out QuestModPlayer modPlayer))
                {
                    continue;
                }
                modPlayer.ActiveQuest = null;
            }
            // Post launch bug fix!!!
            if (Quests is null) return;

            // Generate quest
            int questType = Main.rand.Next(1, 4);
            QuestCollection questsToChooseFrom = Quests.Where(q => ((int)q.QuestType == questType) && q.Predicate);
            CurrentQuest = questsToChooseFrom.TakeRandom();
        }
    }
}
