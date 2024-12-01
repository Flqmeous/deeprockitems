/*using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Misc;
using System.Linq;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using MonoMod.Utils;
using System;
using deeprockitems.Content.Pets.Molly;
using deeprockitems.Common.Items;

namespace deeprockitems.Common.Quests
{
    public class QuestsRewards : ModSystem
    {
        public static int RewardsTier { get; set; }
        public static List<int> UniqueRewards { get; set; }
        public static Dictionary<int, int> CommonPreHardmodeRewards { get; set; }
        public static Dictionary<int, int> CommonHardmodeRewards { get; set; }
        public static Dictionary<int, int> RarePreHardmodeRewards { get; set; }
        public static Dictionary<int, int> RareHardmodeRewards { get; set; }

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("DRGRewardsTier", RewardsTier);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey("DRGRewardsTier"))
            {
                RewardsTier = (int)tag["DRGRewardsTier"];
            }
        }
        public override void OnWorldLoad()
        {
            // Rarest tier of rewards, the odds of pulling one increases with the amount of quests completed per session. Only 1 item will be given of each since the majority of them are unstackable
            UniqueRewards = new List<int>()
            {
                ItemID.GoldenCrate,
                ItemID.LavaFishingHook,
                ItemID.ExtendoGrip,
                ItemID.DiggingMoleMinecart,
                ItemID.PaintSprayer,
                ItemID.BrickLayer,
                ItemID.PortableCementMixer,

            };
            // Common rewards are *always* given.
            CommonPreHardmodeRewards = new Dictionary<int, int>()
            {
                [ItemID.SilverCoin] = 130,
                [ItemID.IronOre] = 25,
                [ItemID.LeadOre] = 25,
                [ItemID.SilverOre] = 30,
                [ItemID.TungstenOre] = 30,
                [ItemID.GoldOre] = 30,
                [ItemID.PlatinumOre] = 30,
                [ItemID.IronBar] = 5,
                [ItemID.LeadBar] = 5,
                [ItemID.SilverBar] = 7,
                [ItemID.TungstenBar] = 7,
                [ItemID.Bone] = 10,
                [ItemID.Bomb] = 7,
                [ItemID.Dynamite] = 2,
                [ItemID.Ale] = 10,


            };
            CommonHardmodeRewards = new Dictionary<int, int>
            {
                [ItemID.CobaltOre] = 25,
                [ItemID.PalladiumOre] = 25,
                [ItemID.MythrilOre] = 20,
                [ItemID.OrichalcumOre] = 20,
                [ItemID.CobaltBar] = 5,
                [ItemID.PalladiumBar] = 5,
                [ItemID.MythrilOre] = 4,
                [ItemID.OrichalcumBar] = 4,


            };
            // Adding pre-existing items
            CommonHardmodeRewards.AddRange(CommonPreHardmodeRewards);
            // Rare rewards are given less often, but they're not super rare. One every few quests at lower rates is fair.
            RarePreHardmodeRewards = new Dictionary<int, int>()
            {
                [ItemID.EnchantedNightcrawler] = 3,
                [ItemID.GoldBar] = 8,
                [ItemID.PlatinumBar] = 8,
                [ItemID.BattlePotion] = 3,
                [ItemID.EndurancePotion] = 3,
                [ItemID.RagePotion] = 3,
                [ItemID.RagePotion] = 3,
                [ItemID.CalmingPotion] = 3,
                [ItemID.HeartreachPotion] = 3,
                [ItemID.LifeforcePotion] = 1,
                [ItemID.IronskinPotion] = 5,
                [ItemID.RegenerationPotion] = 5,
                [ItemID.SwiftnessPotion] = 5,
                [ItemID.ScarabBomb] = 5,
            };
            RareHardmodeRewards = new Dictionary<int, int>()
            {
                [ItemID.AdamantiteOre] = 15,
                [ItemID.TitaniumOre] = 15,
                [ItemID.AdamantiteBar] = 3,
                [ItemID.TitaniumBar] = 3,
                [ItemID.TeleportationPotion] = 3,
            };
            // Adding pre-existing items
            RareHardmodeRewards.AddRange(RarePreHardmodeRewards);
        }
        public static void IssueRewards(DRGQuestsModPlayer modPlayer)
        {
            RewardsTier = SetRewardTiers();
            modPlayer.QuestsCompleted += 1;
            modPlayer.QuestsCompletedThisSession += 1;

            // If quests are threeven
            if (modPlayer.QuestsCompleted % 3 == 0)
            {
                // We're giving matrix cores every 3 quests
                modPlayer.Player.QuickSpawnItem(NPC.GetSource_NaturalSpawn(), ModContent.ItemType<MatrixCore>());
            }
            else
            {
                // Otherwise, give an upgrade token
                modPlayer.Player.QuickSpawnItem(NPC.GetSource_NaturalSpawn(), ModContent.ItemType<UpgradeToken>());
            }

            if (modPlayer.QuestsCompleted > 15)
            {
                UniqueRewards.Add(ModContent.ItemType<ChunkOfNitra>());
            }
            else if (modPlayer.QuestsCompleted == 15)
            {
                modPlayer.Player.QuickSpawnItem(NPC.GetSource_NaturalSpawn(), ModContent.ItemType<ChunkOfNitra>());
            }

            // Generate items
            double weird_number = Math.Log(RewardsTier * 2 / 3 + 1.7); // This is just an overly complex variable
            int numCommonRewards = (int)Math.Floor(Math.Log(weird_number)) + Main.rand.Next(1, 3); // between 1 and 5 common rewards. I felt like using a complex generator :>
            int rareRewardChance = (int)Math.Ceiling(Math.Pow(weird_number - 1.3, 2) / 10 + .2) * 10; // Odds of getting a rare reward

            int uniqueRewardChance = ((modPlayer.QuestsCompletedThisSession + 1) ^ 2) / 3;

            Dictionary<int, int> commonRewardSet; // This is the pool of common rewards that the generator will pull from.
            Dictionary<int, int> rareRewardSet; // Pool of rare rewards to take from.
            Dictionary<int, int> rewardsAndAmounts = new(); // This is the rewards that will be given to the player after this.

            // If in hardmode, use hardmode reward set.
            if (Main.hardMode)
            {
                commonRewardSet = CommonHardmodeRewards;
                rareRewardSet = RareHardmodeRewards;
            }
            else
            {
                commonRewardSet = CommonPreHardmodeRewards;
                rareRewardSet = RarePreHardmodeRewards;
            }

            // Repeat given number of times
            for (int item = 0; item < numCommonRewards; item++)
            {
                int valueToTake = Main.rand.Next(commonRewardSet.Keys.ToList()); // which key to pull from
                if (rewardsAndAmounts.ContainsKey(valueToTake))
                {
                    rewardsAndAmounts[valueToTake] = (int)Math.Floor(rewardsAndAmounts[valueToTake] * 1.5); // If same quests pulled multiple times, "reward' the user for being lucky
                    continue; // Don't add key and value
                }
                rewardsAndAmounts.Add(valueToTake, commonRewardSet[valueToTake]); // Add corresponding key and value
            }
            // Check if should take rare reward
            if (Main.rand.NextBool(rareRewardChance, 10))
            {
                int valueToTake = Main.rand.Next(rareRewardSet.Keys.ToList()); // which key to pull from
                rewardsAndAmounts.Add(valueToTake, rareRewardSet[valueToTake]); // Add corresponding key and value
            }
            // Check if should take unique reward
            if (Main.rand.NextBool(uniqueRewardChance, 100))
            {
                int valueToTake = Main.rand.Next(UniqueRewards);
                modPlayer.Player.QuickSpawnItem(NPC.GetSource_NaturalSpawn(), valueToTake);
            }

            // Convert rewards dictionary to items:
            foreach (KeyValuePair<int, int> values in rewardsAndAmounts)
            {
                float rewardsMultipler = (float)Main.rand.NextFloat(.85f, 1.15f);
                int amount = (int)Math.Ceiling(rewardsMultipler * values.Value);
                modPlayer.Player.QuickSpawnItem(NPC.GetSource_NaturalSpawn(), values.Key, amount);
            }




        }
        public static int SetRewardTiers()
        {
            if (NPC.downedMoonlord)
            {
                return 7;
            }
            if (NPC.downedGolemBoss)
            {
                return 6;
            }
            if (NPC.downedPlantBoss)
            {
                return 5;
            }
            // If all 3 mechs downed
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                return 4;
            }

            // If any of 2 mechs are downed
            if ((NPC.downedMechBoss1 && NPC.downedMechBoss2) || (NPC.downedMechBoss2 && NPC.downedMechBoss3) || (NPC.downedMechBoss1 && NPC.downedMechBoss3))
            {
                return 3;
            }
            // If any of 1 mechs are downed
            if (NPC.downedMechBossAny)
            {
                return 2;
            }
            if (Main.hardMode)
            {
                return 1;
            }
            return 0;
        }
    }
}
*/
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Pets.Molly;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Common.Quests
{
    public class QuestRewardSystem : ModSystem
    {
        /// <summary>
        /// Common rewards make up the majority of quest rewards.
        /// Number of rewards scales with each vanilla boss downed.
        /// </summary>
        public static List<QuestReward> CommonRewards { get; set; }
        /// <summary>
        /// Rare rewards serve as a boost to the gameplay loop.
        /// Chance scales with total quests completed.
        /// </summary>
        [Obsolete("Rare rewards are not currently implemented.")]
        public static List<QuestReward> RareRewards { get; set; }
        /// <summary>
        /// These rewards are seldom rewarded, but they act as alternative ways to obtain rare items, or are outright new items altogether.
        /// Doing multiple quests in a session will greatly increase the chances of getting these rewards.
        /// </summary>
        public static List<QuestReward> UniqueRewards { get; private set; }
        public override void OnWorldLoad()
        {
            RecalculateQuests();
        }
        private void RecalculateQuests()
        {
            // Create list of rewards
            CommonRewards = new List<QuestReward>()
            {
                // Note for those contributing: 10 is the default weight to keep everything equally likely.
                new(ItemID.SilverCoin, 75, true, 15),
                new(ItemID.GoldCoin, 1, true, 15),
                new(ItemID.EndurancePotion, 5, true, 1),
                new(ItemID.MiningPotion, 5, true, 1),
                new(ItemID.BuilderPotion, 5, true, 1),
                new(ItemID.PotionOfReturn, 5, true, 1),
                new(ItemID.SpelunkerPotion, 5, true, 1),
                new(ItemID.RagePotion, 5, WorldGen.crimson, 1),
                new(ItemID.WrathPotion, 5, !WorldGen.crimson, 1),
                new(ItemID.LifeforcePotion, 5, true, 1),
                new(ItemID.SummoningPotion, 5, true, 1),
                new(ItemID.HallowedBar, 15, NPC.downedMechBossAny, 10),
                new(ItemID.LifeFruit, 3, NPC.downedMechBossAny, 10), 
                new(ItemID.SoulofMight, 10, NPC.downedMechBoss1, 10),
                new(ItemID.SoulofSight, 10, NPC.downedMechBoss2),
                new(ItemID.SoulofFright, 10, NPC.downedMechBoss3),
            };
            UniqueRewards = new()
            {
                new(ModContent.ItemType<ChunkOfNitra>(), 1, NPC.downedBoss3, 1),
                // Able to get weapons through quests as a treat.
                new(ModContent.ItemType<M1000>(), 1, NPC.downedBoss3, 1),
                new(ModContent.ItemType<SludgePump>(), 1, NPC.downedBoss3, 1),
                new(ModContent.ItemType<Zhukovs>(), 1, Main.hardMode, 1),
                new(ModContent.ItemType<PlasmaPistol>(), 1, true, 1),
                new(ModContent.ItemType<JuryShotgun>(), 1, Main.hardMode, 1),
            };
        }
    }
    public class QuestReward
    {
        /// <summary>
        /// What ItemID will be rewarded for this quest
        /// </summary>
        public int RewardType { get; set; }
        /// <summary>
        /// Number of rewards to give
        /// </summary>
        public int RewardAmount { get; set; }
        /// <summary>
        /// The condition required to make this reward available. Supplying true results in the quest always being available. <br/>
        /// Use this to make certain rewards available at different stages in the game.
        /// </summary>
        public bool Condition { get; set; }
        /// <summary>
        /// The cumulative weight of this item in the RNG pool
        /// </summary>
        public int Weight { get; set; }
        public QuestReward(int rewardType, int rewardAmount, bool condition, int weight = 10)
        {
            RewardType = rewardType;
            RewardAmount = rewardAmount;
            Condition = condition;
            Weight = weight;
        }
    }
    public enum QuestRewardType
    {
        None = 0,
        Common, // Common rewards are the bulk of a givens quest reward.
        Rare, // Rare rewards get more common with the total amount of quests completed.
        Unique, // Unique rewards are items like accessories or upgrades, scaling with the amount of quests completed per session.

    }
}