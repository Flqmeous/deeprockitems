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
        public override bool? CanDamage(Projectile projectile)
        {
            if (_upgrades is not null)
            {
                return ProjectileCanDamage?.Invoke(projectile, _upgrades);
            }
            return base.CanDamage(projectile);
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
                bool? killed = ProjectileHitTile?.Invoke(projectile, oldVelocity, _upgrades);
                if (killed is not null)
                {
                    return (bool)killed;
                }
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            if (_upgrades is not null)
            {
                bool? result = ProjectileKilled?.Invoke(projectile, timeLeft, _upgrades);
                if (result is not null)
                {
                    return (bool)result;
                } 
            }
            return base.PreKill(projectile, timeLeft);
        }
        #endregion
        #region Delegates for events
        public delegate void HandleOnSpawn(Projectile sender, IEntitySource source, int[] upgrades);
        public delegate void HandleAI(Projectile sender, int[] upgrades);
        public delegate bool? HandleCanDamage(Projectile sender, int[] upgrades);
        public delegate void HandleOnHitNPC(Projectile sender, NPC target, NPC.HitInfo hit, int damageDone, int[] upgrades);
        public delegate void HandleModifyHitNPC(Projectile sender, NPC target, ref NPC.HitModifiers modifiers, int[] upgrades);
        public delegate bool? HandleOnTileCollide(Projectile sender, Vector2 oldVelocity, int[] upgrades);
        public delegate bool? HandleOnKill(Projectile sender, int timeLeft, int[] upgrades);
        #endregion
        #region Static events
        public static event HandleOnSpawn ProjectileSpawned;
        public static event HandleAI ProjectileAI;
        public static event HandleCanDamage ProjectileCanDamage;
        public static event HandleOnHitNPC ProjectileHitNPC;
        public static event HandleModifyHitNPC ProjectileModifyNPC;
        public static event HandleOnTileCollide ProjectileHitTile;
        public static event HandleOnKill ProjectileKilled;
        #endregion
    }
}
