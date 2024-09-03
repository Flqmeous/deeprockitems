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
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is not EntitySource_ItemUse newSource) return;

            if (newSource.Item.ModItem is not UpgradeableItemTemplate modItem) return;

            // Get list of equipped upgrades
            _equippedUpgrades = GetEquippedUpgrades(modItem.UpgradeMasterList);

            foreach (var upgrade in _equippedUpgrades)
            {
                upgrade.Projectile_OnSpawnHook?.Invoke(projectile, source);
            }
        }
        public override void AI(Projectile projectile)
        {
            foreach (var upgrade in _equippedUpgrades)
            {
                upgrade.Projectile_AIHook?.Invoke(projectile);
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (var upgrade in _equippedUpgrades)
            {
                upgrade.Projectile_OnHitNPCHook?.Invoke(projectile, target, hit, damageDone);
            }
        }
        static Upgrade[] GetEquippedUpgrades(UpgradeList upgrades)
        {
            List<Upgrade> equippedUpgrades = new();
            foreach (var tiers in upgrades)
            {
                foreach (var upgrade in tiers)
                {
                    if (!upgrade.IsEquipped) continue;

                    equippedUpgrades.Add(upgrade);
                }
            }
            return [.. equippedUpgrades];
        }
    }
}
