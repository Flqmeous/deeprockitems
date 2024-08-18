using deeprockitems.Content.Buffs;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles.CryoCannonProjectiles
{
    public class IceTrail : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = 300;
        }
        private int _frame = 0;
        private const int frameCount = 4;
        public float Scale { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.damage = 0;
            Scale = 0;
            _frame = Main.rand.Next(0, 4);
        }
        public override void AI()
        {
            if (Scale <= 1f)
            {
                Scale += 0.05f;
            }
            foreach (var npc in Main.ActiveNPCs)
            {
                if (!npc.friendly && npc.Hitbox.Contains(Projectile.Hitbox) && Projectile.localNPCImmunity[npc.whoAmI] == 0)
                {
                    npc.ChangeTemperature(-8, Projectile.owner);
                    Projectile.localNPCImmunity[npc.whoAmI] = Projectile.localNPCHitCooldown;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // Draw based on frame
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 origin = new(Scale * texture.Width * 0.65f, texture.Height / frameCount / 2f);
            Main.EntitySpriteDraw(new DrawData(texture, new Vector2(Projectile.position.X - Main.screenPosition.X, Projectile.position.Y - Main.screenPosition.Y), new Rectangle(0, _frame * texture.Height / frameCount, texture.Width, texture.Height / frameCount), Color.White, Projectile.rotation, origin, Scale, SpriteEffects.None));
            return false;
        }
    }
}
