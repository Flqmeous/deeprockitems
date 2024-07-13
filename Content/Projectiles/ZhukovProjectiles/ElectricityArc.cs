using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using deeprockitems.Assets.Textures;
using Microsoft.Xna.Framework.Graphics;

namespace deeprockitems.Content.Projectiles.ZhukovProjectiles
{
    public class ElectricityArc : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 16;
            Projectile.width = 16;
            DynamicScale = new(1, 1);
            Projectile.timeLeft = 60;
            Projectile.friendly = true;
            Projectile.damage = 24;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }
        public override void SetStaticDefaults()
        {
            frameHeight = 16;
            frameWidth = 48;
        }
        public override void AI()
        {
            // Random frame
            if (Projectile.timeLeft % 3 == 0)
            {
                Projectile.frame = Main.rand.Next(0, 3);
            }
            // Make X scale inversly proportional to timeleft
            int minScale = 0;
        }
        private static int frameHeight;
        private static int frameWidth;
        public Vector2 DynamicScale { get; set; }
        private Rectangle GetFrame => new Rectangle(0, Projectile.frame * frameHeight, frameWidth, frameHeight);
        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(DRGTextures.ElectricityArc, Projectile.position - Main.screenPosition, GetFrame, Color.White, Projectile.rotation, new Vector2(frameWidth / 2, frameHeight / 2), DynamicScale, SpriteEffects.None);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}
