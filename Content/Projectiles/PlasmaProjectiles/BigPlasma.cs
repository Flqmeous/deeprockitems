﻿using Microsoft.Xna.Framework;
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
        private int[] upgrades = new int[4];
        private UpgradeableItemTemplate parentItem = null;
        private bool piercingPlasma = false;
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

            if (piercingPlasma)
            {
                return;
            }
            Projectile.rotation = 0;
            Projectile collidingProjectile = Projectile.IsCollidingWithProjectile(ModContent.ProjectileType<PlasmaBullet>());

            if (collidingProjectile is not null)
            {
                if (Projectile.owner == collidingProjectile.owner)
                {
                    collidingProjectile.Kill();
                    SoundEngine.PlaySound(SoundID.Item14 with { Volume = .5f, Pitch = -.8f }); // Sound of the projectile 
                    Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_ItemUse(parentItem.Item), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaExplosion>(), Projectile.damage, .1f);
                    Projectile.Kill();
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse { Item.ModItem: UpgradeableItemTemplate parentItem })
            {
                upgrades = parentItem.Upgrades;
                this.parentItem = parentItem;
            }
            if (upgrades.Contains(ModContent.ItemType<PiercingPlasmaOC>()))
            {
                Projectile.tileCollide = false;
                Projectile.penetrate = 5;
                Projectile.maxPenetrate = 5;
                piercingPlasma = true;

            }
            if (upgrades.Contains(ModContent.ItemType<VelocitySpeedup>()))
            {
                Projectile.velocity *= 1.5f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                if (upgrades.Contains(ModContent.ItemType<EzBoomOC>()))
                {
                    SoundEngine.PlaySound(SoundID.Item14 with { Volume = .5f, Pitch = -.8f });
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaExplosion>(), Projectile.damage, .1f);
                }
            }
        }

    }
}
