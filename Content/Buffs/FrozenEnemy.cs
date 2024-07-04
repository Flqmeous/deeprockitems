using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class FrozenEnemy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
    }
    public class FrozenGlobalNPC : GlobalNPC
    {
        public override void SetDefaults(NPC entity)
        {
            entity.buffImmune[ModContent.BuffType<FrozenEnemy>()] = entity.aiStyle == NPCAIStyleID.Worm;
        }
        public override bool PreAI(NPC npc)
        {
            // If enemy has frozen effect and is not boss
            if (npc.FindBuffIndex(ModContent.BuffType<FrozenEnemy>()) != -1 && !npc.boss)
            {
                // Apply gravity
                npc.velocity.Y += 1f;

                // Set x velocity to zero
                npc.velocity.X = 0f;

                // Stop vanilla code
                return false;
            }
            return base.PreAI(npc);
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (npc.FindBuffIndex(ModContent.BuffType<FrozenEnemy>()) != -1 && !npc.boss)
            {
                return false;
            }
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (npc.FindBuffIndex(ModContent.BuffType<FrozenEnemy>()) != -1 && !npc.boss)
            {
                return false;
            }
            return base.CanHitNPC(npc, target);
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (npc.FindBuffIndex(ModContent.BuffType<FrozenEnemy>()) != -1 && !npc.boss)
            {
                drawColor = Lighting.GetColor(npc.Center.ToTileCoordinates(), new Color(125, 175, 240));
            }
            else
            {
                base.DrawEffects(npc, ref drawColor);
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }
    }
}
