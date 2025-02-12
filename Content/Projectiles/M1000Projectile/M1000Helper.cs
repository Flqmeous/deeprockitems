﻿using Terraria;
using Terraria.ID;
using Terraria.Audio;
using deeprockitems.Audio;

namespace deeprockitems.Content.Projectiles.M1000Projectile
{
    public class M1000Helper : HeldProjectileBase
    {
        public override float ChargeTime { get; set; } = 30f;
        public override int ProjectileToSpawn { get; set; } = ProjectileID.BulletHighVelocity;
        public override SoundStyle? ChargeSound => DRGSoundIDs.M1000Focus;
        public override SoundStyle? FireSound => DRGSoundIDs.M1000Fire;
        public override void NewSetDefaults() {
            ChargeShotCooldownMultiplier = 2f;
        }
        public override void ModifyProjectileAfterSpawning(Projectile projectile) {
            if ((ammoUsed != ItemID.MusketBall || ammoUsed != ItemID.EndlessMusketPouch) && projectile.maxPenetrate == 5) return;

            projectile.penetrate = 1;
        }
        public override void WhenReachedFullCharge()
        {
            Projectile.damage *= 2;
        }
    }
}