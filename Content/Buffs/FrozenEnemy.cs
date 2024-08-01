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
        private bool _oldFrozen = false;
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
        private int _fallingTime = 0;
        private bool _isFalling = false;
        public override bool PreAI(NPC npc)
        {
            // If enemy was just frozen:
            if (IsFrozen && !_oldFrozen)
            {
                npc.velocity = Vector2.Zero;
                npc.velocity.Y += npc.gravity;

                // Find if there is a floor below this npc
                for (int i = 0; i < 8; i++)
                {
                    // If npc is grounded
                    if (Collision.SolidTiles(new Vector2(npc.position.X, npc.position.Y + 2*i), npc.width, npc.height))
                    {
                        // Grounded
                        _isFalling = false;
                        break;
                    }
                    _isFalling = true; // Set npc to falling
                }
            }
            _oldFrozen = IsFrozen;

            if (IsFrozen)
            {
                if (_isFalling)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        // Damage npc proportional to falling time
                        if (_fallingTime > 30)
                        {
                            var info = npc.CalculateHitInfo((int)(_fallingTime * 3f), 1);
                            npc.StrikeNPC(info);
                            _fallingTime = 0;
                            _isFalling = false;
                        }
                    }
                    else
                    {
                        // Increase falling time (height)
                        _fallingTime++;
                    }
                }
                // Set x velocity to zero
                npc.velocity.X = 0;
                // Apply gravity
                npc.velocity.Y += npc.gravity;

                // Stop vanilla code
                return false;
            }

            return base.PreAI(npc);
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (IsFrozen)
            {
                return false;
            }
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (IsFrozen)
            {
                return false;
            }
            return base.CanHitNPC(npc, target);
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (IsFrozen)
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
