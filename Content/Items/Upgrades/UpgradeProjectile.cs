using Terraria.ModLoader;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Items.Upgrades
{
    public class UpgradeProjectile : GlobalProjectile
    {
#nullable enable
        int[]? _upgrades;
#nullable disable
        #region Projectile lifecycle hooks.
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse { Item.ModItem: UpgradeableItemTemplate { Upgrades: int[] upgrades } })
            {
                _upgrades = upgrades;
                ProjectileSpawned?.Invoke(projectile, source, upgrades);
            }
        }
        public override void AI(Projectile projectile)
        {
            if (_upgrades is not null)
            {
                ProjectileAI?.Invoke(projectile, _upgrades);
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (_upgrades is not null)
            {
                ProjectileHitNPC?.Invoke(projectile, target, hit, damageDone, _upgrades);
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (_upgrades is not null)
            {
                ProjectileModifyNPC?.Invoke(projectile, target, ref modifiers, _upgrades);
            }
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (_upgrades is not null)
            {
                ProjectileHitTile?.Invoke(projectile, oldVelocity, _upgrades);
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (_upgrades is not null)
            {
                ProjectileKilled?.Invoke(projectile, timeLeft, _upgrades);
            }
        }
        #endregion
        #region Delegates for events
        public delegate void HandleOnSpawn(Projectile sender, IEntitySource source, int[] upgrades);
        public delegate void HandleAI(Projectile sender, int[] upgrades);
        public delegate void HandleOnHitNPC(Projectile sender, NPC target, NPC.HitInfo hit, int damageDone, int[] upgrades);
        public delegate void HandleModifyHitNPC(Projectile sender, NPC target, ref NPC.HitModifiers modifiers, int[] upgrades);
        public delegate void HandleOnTileCollide(Projectile sender, Vector2 oldVelocity, int[] upgrades);
        public delegate void HandleOnKill(Projectile sender, int timeLeft, int[] upgrades);
        #endregion
        #region Static events
        public static event HandleOnSpawn ProjectileSpawned;
        public static event HandleAI ProjectileAI;
        public static event HandleOnHitNPC ProjectileHitNPC;
        public static event HandleModifyHitNPC ProjectileModifyNPC;
        public static event HandleOnTileCollide ProjectileHitTile;
        public static event HandleOnKill ProjectileKilled;
        #endregion
    }
}
