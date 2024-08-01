/*using Terraria.ModLoader;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using deeprockitems.Utilities;
using System.Linq;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades
{
    public class UpgradeProjectile : GlobalProjectile
    {
#nullable enable
        public int[]? Upgrades;
#nullable disable
        #region Projectile lifecycle hooks.
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse { Item.ModItem: UpgradeableItemTemplate { Upgrades: int[] upgrades } })
            {
                Upgrades = upgrades;
                foreach (var hook in ProjectileSpawned.GetInvocationList().Cast<HandleOnSpawn>())
                {
                    if (hook.Target is UpgradeTemplate upgrade)
                    {
                        // THIS SHOULD ONLY MATCH ONE PROJECTILE. ALWAYS
                        if (Upgrades.Contains(upgrade.Type))
                        {
                            hook.Invoke(projectile, source);
                        }
                    }
                }
            }
        }
        public override void AI(Projectile projectile)
        {
            if (Upgrades is not null)
            {
                foreach (var hook in ProjectileAI.GetInvocationList().Cast<HandleAI>())
                {
                    if (hook.Target is UpgradeTemplate upgrade)
                    {
                        // THIS SHOULD ONLY MATCH ONE PROJECTILE. ALWAYS
                        if (Upgrades.Contains(upgrade.Type))
                        {
                            hook.Invoke(projectile);
                        }
                    }
                }
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Upgrades is not null)
            {
                foreach (var hook in ProjectileHitNPC.GetInvocationList().Cast<HandleOnHitNPC>())
                {
                    if (hook.Target is UpgradeTemplate upgrade)
                    {
                        // THIS SHOULD ONLY MATCH ONE PROJECTILE. ALWAYS
                        if (Upgrades.Contains(upgrade.Type))
                        {
                            hook.Invoke(projectile, target, hit, damageDone);
                        }
                    }
                }
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Upgrades is not null)
            {
                foreach (var hook in ProjectileModifyNPC.GetInvocationList().Cast<HandleModifyHitNPC>())
                {
                    if (hook.Target is UpgradeTemplate upgrade)
                    {
                        // THIS SHOULD ONLY MATCH ONE PROJECTILE. ALWAYS
                        if (Upgrades.Contains(upgrade.Type))
                        {
                            hook.Invoke(projectile, target, ref modifiers);
                        }
                    }
                }
            }
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (Upgrades is not null)
            {
                foreach (var hook in ProjectileHitTile.GetInvocationList().Cast<HandleOnTileCollide>())
                {
                    if (hook.Target is UpgradeTemplate upgrade)
                    {
                        // THIS SHOULD ONLY MATCH ONE PROJECTILE. ALWAYS
                        if (Upgrades.Contains(upgrade.Type))
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
        public override bool PreAI(Projectile projectile)
        {
            if (Upgrades is not null)
            {

            }
            return base.PreAI(projectile);
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            if (Upgrades is not null)
            {
                foreach (HandlePreKill hook in ProjectileKilled.GetInvocationList().Cast<HandlePreKill>())
                {
                    if (hook.Target is UpgradeTemplate upgrade)
                    {
                        // THIS SHOULD ONLY MATCH ONE PROJECTILE. ALWAYS
                        if (Upgrades.Contains(upgrade.Type))
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
        public delegate void HandleOnSpawn(Projectile sender, IEntitySource source);
        public delegate void HandleAI(Projectile sender);
        public delegate void HandleOnHitNPC(Projectile sender, NPC target, NPC.HitInfo hit, int damageDone);
        public delegate void HandleModifyHitNPC(Projectile sender, NPC target, ref NPC.HitModifiers modifiers);
        public delegate bool? HandleOnTileCollide(Projectile sender, Vector2 oldVelocity, out bool callBase);
        public delegate bool? HandlePreKill(Projectile sender, int timeLeft, out bool callBase);
        public delegate void HandleSendingData(Projectile sender, Dictionary<string, object> data);
        #endregion
        #region Static events
        public static event HandleOnSpawn ProjectileSpawned;
        public static event HandleAI ProjectileAI;
        public static event HandleOnHitNPC ProjectileHitNPC;
        public static event HandleModifyHitNPC ProjectileModifyNPC;
        public static event HandleOnTileCollide ProjectileHitTile;
        public static event HandlePreKill ProjectileKilled;
        public static event HandleSendingData ProjectileSendData;
        #endregion
    }
}
*/