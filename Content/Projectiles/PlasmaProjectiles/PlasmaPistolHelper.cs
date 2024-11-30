using Terraria.ID;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaPistolHelper : HeldProjectileBase
    {
        public override double Spread => 0;
        public override int ProjectileToSpawn { get; set; } = ModContent.ProjectileType<PlasmaBullet>();
        public override SoundStyle? ChargeSound { get; set; } = SoundID.Item117;
        public override SoundStyle? FireSound { get; set; } = SoundID.Item114;
        public override float ChargeTime { get; set; } = 45f;
        public override void WhenReachedFullCharge()
        {
            ProjectileToSpawn = ModContent.ProjectileType<BigPlasma>();
            Projectile.velocity *= .4f;
            Projectile.damage *= 3;
            Spread = 0;
            FireSound = SoundID.Item105;
            Cooldown = 4;
        }
        public override void ModifyProjectileAfterSpawning(Projectile projectile) {
            if (ProjectileToSpawn == ModContent.ProjectileType<BigPlasma>())
            {
                Main.player[Projectile.owner].CheckMana(7, true, false);
            }
        }
        public override void WhileHeldAtCharge() {
            // Drain mana to encourage the player to fire a projectile
            if (this.HasReachedFullCharge && Main.player[Projectile.owner].statMana > 0)
            {
                Main.player[Projectile.owner].statMana -= 1;
            }

            if (Main.player[Projectile.owner].statMana < 7)
            {
                ProjectileToSpawn = ModContent.ProjectileType<PlasmaBullet>();
            }
        }
    }
}
