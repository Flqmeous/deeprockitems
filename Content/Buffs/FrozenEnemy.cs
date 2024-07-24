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
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<FrozenGlobalNPC>().IsFrozen = true;
        }
    }
    public class FrozenGlobalNPC : GlobalNPC
    {
        public bool IsFrozen { get; set; } = false;
        public override bool InstancePerEntity => true;
        public override void ResetEffects(NPC npc)
        {
            IsFrozen = false;
        }
        public override void SetDefaults(NPC entity)
        {
            if (entity.boss || entity.aiStyle == NPCAIStyleID.Worm)
            {
                entity.buffImmune[ModContent.BuffType<FrozenEnemy>()] = true;
            }
        }
        public override bool PreAI(NPC npc)
        {
            // If enemy has frozen effect and is not boss
            if (IsFrozen && !npc.boss)
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
            if (IsFrozen && !npc.boss)
            {
                return false;
            }
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (IsFrozen && !npc.boss)
            {
                return false;
            }
            return base.CanHitNPC(npc, target);
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (IsFrozen && !npc.boss)
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
