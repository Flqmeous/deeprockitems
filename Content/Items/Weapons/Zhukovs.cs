using deeprockitems.Common.PlayerLayers;
using deeprockitems.Common.Weapons;
using deeprockitems.Content.Items.Upgrades.ZhukovsUpgrades;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.Player;

namespace deeprockitems.Content.Items.Weapons
{
    public class Zhukovs : UpgradeableItemTemplate
    {
        public override void SafeDefaults()
        {
            Item.width = 52;
            Item.height = 46;
            Item.rare = ItemRarityID.Cyan;

            Item.damage = 21;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 12;
            Item.knockBack = 1f;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(0, 6, 50, 0);
            ValidUpgrades.Add(ModContent.ItemType<CryoMineletsOC>());
            ValidUpgrades.Add(ModContent.ItemType<EmbeddedDetsOC>());
        }
        public override bool ShootPrimaryUse(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item41, player.Center);
            return true;
        }
        public bool CanAltUse { get; set; } = false;
        public override bool AltFunctionUse(Player player)
        {
            return CanAltUse;
        }
        public override void ResetStats()
        {
            CanAltUse = false;
        }
        public override bool ShootAltUse(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
}
