using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;
using Terraria.DataStructures;
using deeprockitems.Audio;
using deeprockitems.Content.Buffs;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
{
    public class SludgeFragment : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 1;
            Projectile.appliesImmunityTimeOnSingleHits = true;

            DrawOffsetX = -4;
            DrawOriginOffsetY = -4;
        }
        public override void AI()
        {
            if (Projectile.velocity.Y <= 30f) // Set gravity cap
            {
                Projectile.velocity.Y += .5f;
            }
            Projectile.rotation += Projectile.velocity.X / 100;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(DRGSoundIDs.SludgeBallHit with { Volume = .2f }, Projectile.position);
            for (int i = 0; i < 2; i++)
            {
                int dust = Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.SludgeDust>(), Scale: Main.rand.NextFloat(.9f, 1.1f));
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var buff = target.AddStatefulBuff<StatefulSludged>(300);
            buff.AddStateChange(() => Projectile.GetGlobalProjectile<UpgradeGlobalProjectile>().IsUpgradeEquipped("StrongerPoison"), () => buff.StrongSludge = true);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
    }
}
