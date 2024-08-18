using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using deeprockitems.Assets.Textures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using deeprockitems.Content.Buffs;

namespace deeprockitems.Content.Projectiles.ZhukovProjectiles
{
    public class ElectricityArc : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 16;
            Projectile.width = 16;
            DynamicScale = new(1, 1);
            Projectile.timeLeft = 120;
            Projectile.friendly = true;
            Projectile.damage = 24;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.tileCollide = false;
        }
        public override void SetStaticDefaults()
        {
            frameHeight = 16;
            frameWidth = 48;
        }
        private int _timer = 0;
        public override void AI()
        {
            // Add time
            _timer++;

            // Random frame
            if (Projectile.timeLeft % 3 == 0)
            {
                Projectile.frame = Main.rand.Next(0, 3);
                // Spawn dust
                Dust.NewDust(Projectile.Center, 8, 8, DustID.Electric, Scale: 0.5f);
            }
            // Make X scale inversly proportional to timer
            DynamicScale.X = _timer / 22.5f;
        }
        private static int frameHeight;
        private static int frameWidth;
        public Vector2 DynamicScale;
        private Rectangle GetFrame => new Rectangle(0, Projectile.frame * frameHeight, frameWidth, frameHeight);
        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(DRGTextures.ElectricityArc.Value, Projectile.position - Main.screenPosition, GetFrame, Color.White, Projectile.rotation, new Vector2(frameWidth / 2f, frameHeight / 2f), DynamicScale, SpriteEffects.None);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 300);
        }
    }
}
