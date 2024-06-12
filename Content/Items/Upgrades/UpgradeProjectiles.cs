using Terraria.ModLoader;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;

namespace deeprockitems.Content.Items.Upgrades
{
    public class UpgradeProjectiles : GlobalProjectile
    {
        UpgradeableItemTemplate? _weapon;
        int[]? _upgrades;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse { Item.ModItem: UpgradeableItemTemplate { Upgrades: int[] upgrades } })
            {
                _upgrades = upgrades;
                ProjectileSpawned?.Invoke(projectile, source, upgrades);
            }
        }
        public delegate void HandleOnSpawn(Projectile sender, IEntitySource source, int[] upgrades);
        public static event HandleOnSpawn ProjectileSpawned;
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (_weapon is not null && _upgrades is not null)
            {
                ProjectileKilled?.Invoke(projectile, timeLeft, _upgrades);
            }
        }
        public delegate void HandleOnKill(Projectile sender, int timeLeft, int[] upgrades);
        public static event HandleOnKill ProjectileKilled;

    }
}
