using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Buffs;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Misc;
using System.Linq;

namespace deeprockitems.Content.Items.Upgrades
{
    [ValidWeapons(
        typeof(PlasmaPistol),
        typeof(SludgePump))]
    public class OvertunedNozzle: UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<OvertunedNozzle>())
                .AddIngredient<UpgradeToken>()
                .AddTile(TileID.Anvils)
                .AddRecipeGroup(nameof(ItemID.DemoniteBar), 10)
                .AddRecipeGroup(RecipeGroupID.IronBar, 10)
                .AddIngredient(ItemID.Bone, 15)
                .Register();
        }
        public class OvertunedNozzleProjectile : UpgradeGlobalProjectile<OvertunedNozzle>
        {
            
        }
        public override void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem)
        {
            modItem.Item.shootSpeed *= 1.5f;
        }
    }
}
