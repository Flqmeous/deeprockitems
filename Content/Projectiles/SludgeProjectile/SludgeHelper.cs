using deeprockitems.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static System.Math;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
{
    public class SludgeHelper : HeldProjectileBase
    {
        public override int ProjectileToSpawn { get; set; } = ModContent.ProjectileType<SludgeBall>();
        public override float ChargeTime { get; set; } = 50f;
        public override SoundStyle? ChargeSound => DRGSoundIDs.SludgePumpFocus with { Volume = .8f, PitchVariance = 1f};
        public override SoundStyle? FireSound => DRGSoundIDs.SludgePumpFire with { Volume = .5f, PitchVariance = .75f};
        public override void WhenReachedFullCharge()
        {
            Projectile.damage = (int)Floor(Projectile.damage * 1.75f);
        }
    }
}