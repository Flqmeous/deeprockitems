using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override string GlowTexture => "deeprockitems/Content/Projectiles/PlasmaProjectile/PlasmaBullet";
        public override bool PreDraw(ref Color lightColor)
        {
            Lighting.AddLight(Projectile.Center, new Vector3(75, 22, 90).RGBToVector3());
            return true;
        }
    }
}
