using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles
{
    public class DebugProjectile : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
        }
        public override string Texture => "deeprockitems/Assets/WhitePixel";
        public override bool PreDraw(ref Color lightColor) {
            Main.EntitySpriteDraw(new DrawData(Assets.WhitePixel.Value, new Rectangle((int)(Projectile.position.X - Main.screenPosition.X), (int)(Projectile.position.Y - Main.screenPosition.Y), Projectile.width, Projectile.height), Color.Orange with { A = 0x80}));
            return false;
        }
    }
}
