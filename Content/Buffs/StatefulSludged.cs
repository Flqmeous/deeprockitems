using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class StatefulSludged : StatefulBuff
    {
        public bool StrongSludge { get; set; } = false;
        public bool SlowingSludge { get; set; } = false;
        public override void SetStaticDefaults() {
            
        }
        public override void OnCreate() {

        }
        public override void Update(Player player) {
            if (StrongSludge)
            {
                player.lifeRegen -= 30;
            }
            else
            {
                player.lifeRegen -= 15;
            }
        }
        public override void Update(NPC npc, ref int damage) {
            int damagePerSecond = 15;
            if (StrongSludge)
            {
                damagePerSecond = 30;
            }
            npc.lifeRegen -= damagePerSecond * 2;
            // Display dps
            damage = damagePerSecond;
        }
        float SLOW_MULTIPLIER => 0.5f;
        public override void PostUpdate(NPC npc) {
            if (!SlowingSludge) return;
            // update velocity, oldvelocity, and position
            npc.position.X -= npc.velocity.X * (1f - SLOW_MULTIPLIER);
            if (npc.noGravity)
            {
                // nograv NPCs always get Y velocity slow
                npc.position.Y -= npc.velocity.Y * (1f - SLOW_MULTIPLIER);
                return;
            }
            // gravity NPCs get y velocity while they are traveling upward
            if (npc.velocity.Y >= 0) return;
            npc.velocity.Y -= npc.velocity.Y * 0.25f * (1f - SLOW_MULTIPLIER);
        }
    }
}
