using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System;

namespace deeprockitems.Common.NPCs
{
    public class EmbeddedDetsNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int EmbeddedCount { get; set; } = 0;
        public void ExplodeThisNPC(NPC npc, Player player)
        {
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, position: npc.Center);
            player.StrikeNPCDirect(npc, new NPC.HitInfo() { Damage = (int)Math.Floor(150 + 400*Math.Log10(EmbeddedCount)), Knockback = 1f });
            EmbeddedCount = 0;
        }
    }
}
