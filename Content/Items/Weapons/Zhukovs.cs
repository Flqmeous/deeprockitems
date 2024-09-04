using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class Zhukovs : UpgradeableItemTemplate
    {
        public override void NewSetDefaults()
        {
            Item.width = 52;
            Item.height = 46;
            Item.rare = ItemRarityID.Cyan;

            Item.damage = 28;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 12;
            Item.knockBack = 1f;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.consumeAmmoOnLastShotOnly = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(0, 6, 50, 0);
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<Zhukovs>())
                .AddIngredient(ItemID.PhoenixBlaster)
                .AddIngredient(ItemID.IllegalGunParts, 2)
                .AddIngredient(ItemID.SoulofNight, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item41, player.Center);
            // Shoot with spread. Doing this here to preserve overclocks.
            Vector2 spreadVelocity = velocity.RotatedByRandom(MathHelper.Pi / 32);
            Projectile.NewProjectile(source, position, spreadVelocity, type, damage, knockback, Owner: player.whoAmI);
            return false;
        }
    }
}
