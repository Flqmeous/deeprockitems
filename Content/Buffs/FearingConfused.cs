﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class FearingConfused : ModBuff
    {
        public override void Update(NPC npc, ref int buffIndex) {
            npc.confused = true;
            npc.canDisplayBuffs = false;
        }
    }
}
