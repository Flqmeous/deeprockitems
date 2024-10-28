using deeprockitems.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class Sludged : ModBuff
    {
        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex) {
            npc.GetGlobalNPC<SludgedEnemy>().IsSludged = true;
            if (npc.boss || npc.buffImmune[BuffID.Chilled]) return;
            npc.GetGlobalNPC<SludgedEnemy>().IsSlowed = true;
        }
        public override void Update(Player player, ref int buffIndex) {
            player.GetModPlayer<SludgedPlayer>().IsSludged = true;
        }
    }
    public class SludgedPlayer : ModPlayer {
        public bool IsSludged { get; set; } = false;
        public override void ResetEffects() {
            IsSludged = false;
        }
        public override void UpdateBadLifeRegen() {
            if (!IsSludged) return;

            Player.lifeRegen -= 20;
            Player.moveSpeed *= 0.66f;
        }
    }
    public class SludgedEnemy : GlobalNPC {
        public override bool InstancePerEntity => true;
        public bool IsSludged { get; set; } = false;
        public bool IsSlowed { get; set; } = false;
        public override void ResetEffects(NPC npc) {
            IsSludged = false;
        }
        float SLOW_MULTIPLIER => 0.75f;
        public override void DrawEffects(NPC npc, ref Color drawColor) {
            if (!IsSludged) return;
            drawColor = new(0.45f, 1f, 0.35f);
        }
        public override void PostAI(NPC npc) {
            // Undo position if sludged
            if (!IsSlowed) return;
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
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            if (!IsSludged) return;

            npc.lifeRegen -= 20;
            //// Slow down all velocity for flying enemies
            //if (npc.noGravity)
            //{
            //    npc.velocity *= SLOW_MULTIPLIER;
            //    return;
            //}
            //// Always slow down x velocity
            //npc.velocity.X *= SLOW_MULTIPLIER;
            //// If NPC's vertical velocity is less than 0 (they've jumped), decay it
            //if (npc.velocity.Y >= 0) return;
            //npc.velocity.Y *= SLOW_MULTIPLIER;
        }
    }
}
