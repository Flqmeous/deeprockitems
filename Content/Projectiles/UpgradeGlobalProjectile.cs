using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Items;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Upgrades;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles
{
    public class UpgradeGlobalProjectile : GlobalProjectile
    {
        Upgrade[] _equippedUpgrades = [];
        UpgradableWeapon parentItem;
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is not EntitySource_FromUpgradableWeapon newSource) return;

            // Save weapon
            parentItem = newSource.Item;

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
            if (parentItem != null)
            {
                // First of all, disable damage variance. Evil!
                modifiers.DamageVariationScale *= 0f;
                modifiers.DisableCrit();
            }

            foreach (var upgrade in _equippedUpgrades)
            {
                if (upgrade.Behavior.Projectile_ModifyHitNPCHook == null) return;
                
                modifiers = upgrade.Behavior.Projectile_ModifyHitNPCHook.Invoke(projectile, target, modifiers);
            }
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
