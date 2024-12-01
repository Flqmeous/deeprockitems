using deeprockitems.Content.Projectiles;
using Terraria;
using deeprockitems.Content.Items.Weapons;

namespace deeprockitems.Common.EntitySources
{
    public class EntitySource_FromHeldProjectile(Player player, UpgradableWeapon item, int ammoItemID, HeldProjectileBase projectile, string context = null) : EntitySource_FromUpgradableWeapon(player, item, ammoItemID, context)
    {
        public HeldProjectileBase SourceProjectile { get => projectile; }
    }
}
