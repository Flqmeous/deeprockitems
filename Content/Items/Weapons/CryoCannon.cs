using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Projectiles.CryoCannonProjectiles;
using deeprockitems.Content.Items.Upgrades.CryoCannon;
using deeprockitems.Content.Items.Upgrades;

namespace deeprockitems.Content.Items.Weapons
{
    public class CryoCannon : UpgradeableItemTemplate
    {
        public override void NewSetDefaults()
        {
            Item.width = 30;
            Item.height = 22;
            Item.mana = 8;
            Item.damage = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<CryoProjectile>();
            Item.useAnimation = Item.useTime = 6;
            Item.shootSpeed = 16f;
            Item.DamageType = DamageClass.Magic;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
    }
}
