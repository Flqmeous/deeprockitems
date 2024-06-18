using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace deeprockitems.Common.NPCs
{
    public class EmbeddedDetsNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool IsEmbedded { get; set; } = false;
        public void ExplodeThisNPC(NPC npc, Player player)
        {
            IsEmbedded = false;
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, position: npc.Center);
            player.StrikeNPCDirect(npc, new NPC.HitInfo() { Damage = 150, Knockback = 1f });
        }
    }
}
