using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Items;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles
{
    /// <summary>
    /// This class facilitates two purposes:
    /// 1. Allow upgrades to affect upgradable projectiles
    /// 2. Allow upgrade information to pass between parent and child projectiles.
    /// </summary>
    public class UpgradeGlobalProjectile : GlobalProjectile
    {
        Upgrade[] _equippedUpgrades = [];
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // Pass entitySource
            if (source is EntitySource_Parent { Entity: Projectile newProj })
            {
                var global = newProj.GetGlobalProjectile<UpgradeGlobalProjectile>();
                _equippedUpgrades = global._equippedUpgrades;
                return;
            }
            if (source is not EntitySource_FromUpgradableWeapon newSource) return;

            // Get list of equipped upgrades
            _equippedUpgrades = GetEquippedUpgrades(newSource.Item.UpgradeMasterList);

            foreach (var upgrade in _equippedUpgrades)
            {
                upgrade.Behavior.Projectile_OnSpawnHook?.Invoke(projectile, source);
            }
        }
        public override void AI(Projectile projectile)
        {
            foreach (var upgrade in _equippedUpgrades)
            {
                upgrade.Behavior.Projectile_AIHook?.Invoke(projectile);
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (var upgrade in _equippedUpgrades)
            {
                upgrade.Behavior.Projectile_OnHitNPCHook?.Invoke(projectile, target, hit, damageDone);
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
            if (_equippedUpgrades != null)
            {
                // First of all, disable damage variance. Evil!
                modifiers.DamageVariationScale *= 0f;
                modifiers.DisableCrit();
            }

            foreach (var upgrade in _equippedUpgrades)
            {
                if (upgrade.Behavior.Projectile_ModifyHitNPCHook == null) continue;
                
                modifiers = upgrade.Behavior.Projectile_ModifyHitNPCHook.Invoke(projectile, target, modifiers);
            }
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor) {
            bool callBase = true;
            foreach (var upgrade in _equippedUpgrades)
            {
                if (upgrade.Behavior.Projectile_PreDrawHook == null) continue;

                // If the hook was found, predraw and cache the return value. 
                bool result = upgrade.Behavior.Projectile_PreDrawHook.Invoke(projectile, lightColor);
                if (!result)
                {
                    callBase = false;
                }
            }
            if (!callBase) return false;
            return base.PreDraw(projectile, ref lightColor);
        }
        public override bool PreKill(Projectile projectile, int timeLeft) {
            bool callBase = true;
            foreach (var upgrade in _equippedUpgrades)
            {
                if (upgrade.Behavior.Projectile_PreKillHook == null) continue;

                // If the hook was found, prekill and return base 
                bool result = upgrade.Behavior.Projectile_PreKillHook.Invoke(projectile, timeLeft);
                if (!result)
                {
                    callBase = false;
                }
            }
            if (!callBase) return false;
            return base.PreKill(projectile, timeLeft);
        }
        static Upgrade[] GetEquippedUpgrades(UpgradeList upgrades)
        {
            List<Upgrade> equippedUpgrades = new();
            foreach (var tiers in upgrades)
            {
                foreach (var upgrade in tiers)
                {
                    if (!upgrade.UpgradeState.IsEquipped) continue;

                    equippedUpgrades.Add(upgrade);
                }
            }
            return [.. equippedUpgrades];
        }
    }
}
