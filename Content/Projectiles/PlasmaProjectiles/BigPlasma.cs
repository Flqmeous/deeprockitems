using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using deeprockitems.Utilities;
using Terraria.Audio;
using Terraria.ID;
using System;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class BigPlasma : ModProjectile // Darn big plasma.. and their exploding!
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.rotation = 0;
            Projectile.timeLeft = 600;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 16;
            height = 16;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override string GlowTexture => "deeprockitems/Content/Projectiles/PlasmaProjectile/BigPlasma";
        public override bool PreDraw(ref Color lightColor)
        {
            Lighting.AddLight(Projectile.Center, new Vector3(100, 30, 120).RGBToVector3());
            return true;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 3)
            {
                Projectile.frameCounter = 0;
                Projectile.rotation = Main.rand.Next(0, 3) * MathHelper.PiOver2;
                Projectile.frame = Main.rand.Next(0, 3);
            }
        }
    }
}
