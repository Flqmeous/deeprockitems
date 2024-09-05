using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace deeprockitems.Content.Buffs
{
    public class StunnedEnemy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            // If we hit an npc with realLife, remove buff from here and apply to the parent
            if (npc.realLife > -1 && npc.realLife != npc.whoAmI)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Main.npc[npc.realLife].AddBuff(Type, npc.buffTime[buffIndex]);
                    npc.DelBuff(buffIndex);
                }
                Main.npc[npc.realLife].GetGlobalNPC<StunnedEnemyNPC>().IsStunned = true;
                return;
            }
            npc.GetGlobalNPC<StunnedEnemyNPC>().IsStunned = true;
        }
    }
    public class StunnedEnemyNPC : GlobalNPC
    {
        public bool IsStunned { get; set; } = false;
        private bool _oldStun = false;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC entity)
        {
            entity.buffImmune[ModContent.BuffType<StunnedEnemy>()] = entity.boss;
        }
        public override void ResetEffects(NPC npc)
        {
            IsStunned = false;
        }
        public override bool PreAI(NPC npc)
        {
            // If NPC was just stunned:
            if (IsStunned && !_oldStun)
            {
                npc.velocity = new(0, 0);
                // Share stun to the head of the npc.

            }
            _oldStun = IsStunned;

            // If npc is still stunned
            if (IsStunned)
            {
                if (!npc.noGravity)
                {
                    npc.velocity.Y += npc.gravity;
                }
                return false;
            }
            return base.PreAI(npc);
        }
        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (IsStunned) return false;
            return base.CanHitNPC(npc, target);
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (IsStunned) return false;
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        private int _stunFrameTimer = 0;
        private int _stunFrame = 0;
        private const int _frameCount = 3;
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (IsStunned)
            {
                // Increment frame
                _stunFrameTimer++;
                if (_stunFrameTimer > 8)
                {
                    _stunFrameTimer = 0;
                    _stunFrame++;
                    if (_stunFrame >= _frameCount)
                    {
                        _stunFrame = 0;
                    }
                }

                // Drawpos
                Vector2 center = new Vector2(npc.Center.X, npc.position.Y - 10f);
                Vector2 adjustedDrawPos = center - 0.5f * DRGTextures.StunTwinkle.Size();
                int frameHeight = DRGTextures.StunTwinkle.Height / _frameCount;
                Rectangle frame = new Rectangle(0, _stunFrame * frameHeight, DRGTextures.StunTwinkle.Width, frameHeight);
                // Draw
                Main.EntitySpriteDraw(new DrawData(DRGTextures.StunTwinkle, adjustedDrawPos - Main.screenPosition, frame, Color.White));
            }

            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
    }
}
