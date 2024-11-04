﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using deeprockitems.Common.Quests;
using Terraria.GameContent.Personalities;
using deeprockitems.Content.Projectiles.MissionControlAttack;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;

namespace deeprockitems.Content.NPCs.MissionControl
{
    [AutoloadHead]
    public class MissionControl : ModNPC
    {
        readonly static string location = "Mods.deeprockitems.Dialogue.MissionControl.";
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 23;

            NPCID.Sets.ExtraFramesCount[Type] = 8;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 1600; // The amount of pixels away from the center of the npc that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 1;
            NPCID.Sets.PrettySafe[Type] = 400;
            NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Direction = 1
            };


            NPC.Happiness
                .SetBiomeAffection(new Common.Interfaces.SpaceBiome(), AffectionLevel.Love) // Loves space!
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like) // Likes the underground
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Hate); // Hates snow
            for (int id = 0; id < NPCID.Count; id++)
            {
                NPC npc;
                if (!ContentSamples.NpcsByNetId.TryGetValue(id, out npc))
                {
                    continue;
                }
                if (npc.townNPC)
                {
                    AffectionLevel level = AffectionLevel.Dislike;
                    switch (id)
                    {
                        case NPCID.GoblinTinkerer:
                            level = AffectionLevel.Love;
                            break;
                        case NPCID.TaxCollector:
                            level = AffectionLevel.Love;
                            break;
                        case NPCID.Steampunker:
                            level = AffectionLevel.Like;
                            break;
                        case NPCID.Princess:
                            level = AffectionLevel.Like;
                            break;
                        case NPCID.Stylist:
                            level = AffectionLevel.Hate;
                            break;
                        default:
                            break;
                    }
                    NPC.Happiness.SetNPCAffection(id, level);
                }
            }

        }
        public override bool CanGoToStatue(bool toKingStatue)
        {
            return toKingStatue;
        }
        public override void SetDefaults()
        {
            
            NPC.width = 18;
            NPC.height = 40;
            NPC.townNPC = NPC.friendly = true;
            NPC.aiStyle = 7;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.Dryad;
            DrawOffsetY = 2;


            NPC.knockBackResist = 0.5f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

                new FlavorTextBestiaryInfoElement("Mission Control may be short, but he's also bald. He gives quests to enhance the unique weapons found around the time of his arrival."),
            });
        }
        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            if (numTownNPCs > 5 && NPC.downedSlimeKing)
            {
                return true;
            }
            return false;
        }
        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return true;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 1f;
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 1;
            knockback = 0f;
        }
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 480;
            randExtraCooldown = 120;
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<ResupplyPodMarker>();
            attackDelay = 1;
        }
        public override string GetChat()
        {
            // Try getting modplayer
            if (!Main.LocalPlayer.TryGetModPlayer(out QuestModPlayer modPlayer)) return "Quest ModPlayer unable to instantiate. Create a bug report on https://scottysimply.github.com/deeprockitems";

            // Create RNG to pull quest
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            // Add general dialogue
            AddChat(dialogue, "StandardDialogue1", 1, Main.LocalPlayer.name);
            AddChat(dialogue, "StandardDialogue2");
            AddChat(dialogue, "StandardDialogue3");

            // If quest is ongoing, add quest-specific dialogue
            if (modPlayer.ActiveQuest is not null && !modPlayer.ActiveQuest.Completed)
            {
                AddChat(dialogue, "QuestOngoing1", 3);
                AddChat(dialogue, "QuestOngoing2", 3);
                AddChat(dialogue, "QuestOngoing3", 3);
            }

            return dialogue;
        }
        private static void AddChat(WeightedRandom<string> dialogue, string key, double weight = 1, params object[] format)
        {
            dialogue.Add(Language.GetTextValue(location + key, format), weight);
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.64");
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            // If the quest button was clicked
            if (firstButton)
            {
                // Get modplayer
                if (!Main.LocalPlayer.TryGetModPlayer(out QuestModPlayer modPlayer)) return;

                // Give player a quest if they don't have one.
                if (modPlayer.ActiveQuest is null)
                {
                    // Get system
                    QuestSystem system = ModContent.GetInstance<QuestSystem>();
                    // Give quest from system
                    modPlayer.ActiveQuest = system.CurrentQuest.CreateQuestFromThis();
                }

                // If player is owed quest rewards, give them rewards
                if (modPlayer.ActiveQuest.Completed && !modPlayer.ActiveQuest.HasQuestBeenRewarded)
                {
                    // Give rewards
                    modPlayer.GiveDeepRockReward();

                    // Set chat
                    WeightedRandom<string> dialogue = new();
                    AddChat(dialogue, "QuestCompleted1");
                    AddChat(dialogue, "QuestCompleted2");
                    AddChat(dialogue, "QuestCompleted3");
                    Main.npcChatText = dialogue;

                    // Disable rewards
                    modPlayer.ActiveQuest.HasQuestBeenRewarded = true;
                }
                // Else if quest has been completed
                else if (modPlayer.ActiveQuest.Completed)
                {
                    // Set chat
                    WeightedRandom<string> dialogue = new();
                    AddChat(dialogue, "QuestInactive1");
                    AddChat(dialogue, "QuestInactive2");
                    AddChat(dialogue, "QuestInactive3");
                    Main.npcChatText = dialogue;
                }
                // Else, give player quest and display the correct chat message
                else
                {
                    // Set dialogue variation
                    int chatVariation = Main.rand.Next(1, 3);

                    // Get types and amounts
                    int type = modPlayer.ActiveQuest.Data.TypeRequired;
                    int amount = modPlayer.ActiveQuest.Data.AmountRequired;

                    // Change dialogue based on variation and what quest type
                    switch (modPlayer.ActiveQuest.Type)
                    {
                        case QuestID.Mining:
                            // Find if the map object name has a name--if else, use block name
                            Main.npcChatText = Language.GetTextValue(location + $"QuestStartMining{chatVariation}", Lang.GetItemNameValue(type).Pluralizer(amount), amount);
                            break;
                        case QuestID.Gathering:
                            Main.npcChatText = Language.GetTextValue(location + $"QuestStartGather{chatVariation}", Lang.GetItemNameValue(type).Pluralizer(amount), amount);
                                break;
                        case QuestID.Fighting:
                            Main.npcChatText = Language.GetTextValue(location + $"QuestStartSlay{chatVariation}", Lang.GetNPCNameValue(type).Pluralizer(amount), amount);
                                break;
                    }
                    Main.npcChatCornerItem = modPlayer.ActiveQuest.ItemIcon;
                }
            }
        }
        /*private void QuestButtonClicked()
        {
            // This is the modplayer of the player who talked to the NPC.
            DRGQuestsModPlayer modPlayer = Main.LocalPlayer.GetModPlayer<DRGQuestsModPlayer>();
            if (modPlayer is null) return; // Return if null.

            // If quest is inactive (completed or otherwise) and player is owed rewards
            if (!modPlayer.PlayerHasClaimedRewards && modPlayer.CurrentQuestInformation[0] == -1)
            {
                QuestsRewards.IssueRewards(modPlayer); // Give the player the rewards they're owed
                modPlayer.PlayerHasClaimedRewards = true; // Don't let the player claim any more rewards
                modPlayer.CurrentQuestInformation[3] = 0; // Quest progress reset to 0, in case it got lowered below zero by mistake
                int chat = Main.rand.Next(3);
                Main.npcChatText = chat switch // Congratulatory messages
                {
                    0 => Language.GetTextValue(location + "QuestCompleted1"),
                    1 => Language.GetTextValue(location + "QuestCompleted2"),
                    _ => Language.GetTextValue(location + "QuestCompleted3")
                };
            }
            // If a quest is completed
            else if (modPlayer.CurrentQuestInformation[0] == -1)
            {
                int chat = Main.rand.Next(3);
                Main.npcChatText = chat switch // Messages telling the player that no quests are available
                {
                    0 => Language.GetTextValue(location + "QuestInactive1"),
                    1 => Language.GetTextValue(location + "QuestInactive2"),
                    _ => Language.GetTextValue(location + "QuestInactive3")
                };
            }
            // A quest is ongoing or needs to be created
            else
            {
                // If a quest needs to be created, create the quest
                if (modPlayer.CurrentQuestInformation[0] == 0)
                {
                    QuestsBase.Talk_CreateQuest(modPlayer);
                }
                bool chat = !Main.rand.NextBool(2);
                // This is messy, but I wanted two available messages for quests. Since they rely on templates, I wanted the messages to be less same-y
                switch (modPlayer.CurrentQuestInformation[0])
                {   // LOOOONG lines. this is just further randomizing between two options to add flavor.
                    case 1:
                        Main.npcChatText = chat ? Language.GetTextValue(location + "QuestStartMining1", Lang.GetMapObjectName(MapHelper.tileLookup[modPlayer.CurrentQuestInformation[1]]).Pluralizer(modPlayer.CurrentQuestInformation[3]), modPlayer.CurrentQuestInformation[3]) : Language.GetTextValue(location + "QuestStartMining2", Lang.GetMapObjectName(MapHelper.tileLookup[modPlayer.CurrentQuestInformation[1]]).Pluralizer(modPlayer.CurrentQuestInformation[3]), modPlayer.CurrentQuestInformation[3]);
                        Main.npcChatCornerItem = ItemID.IronPickaxe;
                        break;
                    case 2:
                        Main.npcChatText = chat ? Language.GetTextValue(location + "QuestStartGather1", Lang.GetItemName(modPlayer.CurrentQuestInformation[1]).ToString().Pluralizer(modPlayer.CurrentQuestInformation[3]), modPlayer.CurrentQuestInformation[3]) : Language.GetTextValue(location + "QuestStartGather2", Lang.GetItemName(modPlayer.CurrentQuestInformation[1]).ToString().Pluralizer(modPlayer.CurrentQuestInformation[3]), modPlayer.CurrentQuestInformation[3]);
                        Main.npcChatCornerItem = ItemID.StaffofRegrowth;
                        break;
                    default:
                        Main.npcChatText = chat ? Language.GetTextValue(location + "QuestStartSlay1", Lang.GetNPCName(modPlayer.CurrentQuestInformation[1]).ToString().Pluralizer(modPlayer.CurrentQuestInformation[3]), modPlayer.CurrentQuestInformation[3]) : Language.GetTextValue(location + "QuestStartSlay2", Lang.GetNPCName(modPlayer.CurrentQuestInformation[1]).ToString().Pluralizer(modPlayer.CurrentQuestInformation[3]), modPlayer.CurrentQuestInformation[3]);
                        Main.npcChatCornerItem = ItemID.CopperShortsword;
                        break;
                }
            }
        }*/
    }
    public static class Extensions
    {
        private static List<string> ores = new List<string>()
        {
            "copper",
            "iron",
            "silver",
            "gold",
            "tin",
            "lead",
            "tungsten",
            "platinum",
            "demonite",
            "crimtane",
            "cobalt",
            "mythril",
            "adamantite",
            "palladium",
            "orichalcum",
            "titanium",
            "chlorophyte",
        };
        public static string Pluralizer(this string str, int count)
        {
            if (ores.Contains(str.ToLower()))
            {
                return str + " Ore";
            }
            if (count == 1)
            {
                return str;
            }
            return str + "s";
            /*for (int i = 0; i < words.Length; i++)
            {
                if (int.TryParse(words[i], out int n))
                {
                    if (n == 1)
                    {
                        return str;
                    }
                    else
                    {
                        i += 1;
                        if (words[i].Contains(",") || words[i].Contains("."))
                        {
                            words[i] = words[i][..^1] + "s" + words[i][^1];
                        }
                    }
                }
            }
            for (int x = 0; x < words.Length; x++)
            {
                sentence += words[x] + " ";
            }
            return sentence.TrimEnd();*/
        }
    }
}