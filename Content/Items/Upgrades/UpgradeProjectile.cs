using Terraria.ModLoader;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using deeprockitems.Utilities;
using System.Linq;

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
                foreach (var hook in ProjectileHitTile.GetInvocationList().Cast<HandleOnTileCollide>())
                {
                    if (hook.Target is UpgradeTemplate upgrade)
                    {
                        // THIS SHOULD ONLY MATCH ONE PROJECTILE. ALWAYS
                        if (_upgrades.Contains(upgrade.Type))
                        {
                            bool? shouldContinue = hook.Invoke(projectile, oldVelocity, out bool callBase);
                            if (shouldContinue is not null)
                            {
                                if (callBase)
                                {
                                    projectile.ModProjectile?.OnTileCollide(oldVelocity);
                                }
                                return (bool)shouldContinue;
                            }
                        }
                    }
                }
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            if (_upgrades is not null)
            {
                foreach (HandlePreKill hook in ProjectileKilled.GetInvocationList().Cast<HandlePreKill>())
                {
                    if (hook.Target is UpgradeTemplate upgrade)
                    {
                        // THIS SHOULD ONLY MATCH ONE PROJECTILE. ALWAYS
                        if (_upgrades.Contains(upgrade.Type))
                        {
                            bool? shouldContinue = hook.Invoke(projectile, timeLeft, out bool callBase);
                            if (shouldContinue is not null)
                            {
                                if (callBase)
                                {
                                    projectile.ModProjectile?.PreKill(timeLeft);
                                }
                                return (bool)shouldContinue;
                            }
                        }
                    }
                }
            }
            return base.PreKill(projectile, timeLeft);
        }
        #endregion
        #region Delegates for events
        public delegate void HandleOnSpawn(Projectile sender, IEntitySource source, int[] upgrades);
        public delegate void HandleAI(Projectile sender, int[] upgrades);
        public delegate void HandleOnHitNPC(Projectile sender, NPC target, NPC.HitInfo hit, int damageDone, int[] upgrades);
        public delegate void HandleModifyHitNPC(Projectile sender, NPC target, ref NPC.HitModifiers modifiers, int[] upgrades);
        public delegate bool? HandleOnTileCollide(Projectile sender, Vector2 oldVelocity, out bool callBase);
        public delegate bool? HandlePreKill(Projectile sender, int timeLeft, out bool callBase);
        #endregion
        #region Static events
        public static event HandleOnSpawn ProjectileSpawned;
        public static event HandleAI ProjectileAI;
        public static event HandleOnHitNPC ProjectileHitNPC;
        public static event HandleModifyHitNPC ProjectileModifyNPC;
        public static event HandleOnTileCollide ProjectileHitTile;
        public static event HandlePreKill ProjectileKilled;
        #endregion
    }
}
