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
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            DrawOffsetX = -8;
            DrawOriginOffsetY = -8;
        }
        public override string GlowTexture => "deeprockitems/Content/Projectiles/PlasmaProjectile/BigPlasma";
        public override bool PreDraw(ref Color lightColor)
        {
            Lighting.AddLight(Projectile.Center, new Vector3(100, 30, 120).RGBToVector3());
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = 0;
        }
    }
}
