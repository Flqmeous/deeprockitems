using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace deeprockitems.Content.Items.Upgrades.M1000Upgrades
{
    public class SupercoolOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<SupercoolOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.MusketBall, 75)
            .AddIngredient(ItemID.FrostCore)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();

        }
        public override void ProjectileOnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.ai[1] <= 900 && projectile.ai[1] > 0)
            {
                projectile.damage *= 3;
            }
        }
        public override void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem)
        {
            modItem.UseTimeScale = 1.50f;
        }
    }
}
