using deeprockitems.Content.Buffs;
using Terraria;

namespace deeprockitems.Content.Buffs
{
    public class Sludged : InstancedBuff
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            int dps = StrongSludge ? 30 : 15;
            npc.lifeRegen -= dps * 2;
            damage = dps;
            // Update movement
            if (npc.noGravity)
            {
                // Flying NPCs get vertical slowing
                npc.position.Y -= npc.velocity.Y * (1 - SlowMultiplier);
            }
            // All NPCs get horizontal slowing
            npc.position.X -= npc.velocity.X * (1 - SlowMultiplier);
        }
        private float SlowMultiplier => 0.75f;
        public bool SlowingSludge { get; set; } = false;
        public bool StrongSludge { get; set; } = false;
    }
}