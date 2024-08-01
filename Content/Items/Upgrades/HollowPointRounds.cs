using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Items.Misc;

namespace deeprockitems.Content.Items.Upgrades
{
    [ValidWeapons(
        typeof(Zhukovs),
        typeof(M1000),
        typeof(JuryShotgun))]
    public class HollowPointRounds : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<HollowPointRounds>())
                .AddTile(TileID.MythrilAnvil)
                .AddIngredient<UpgradeToken>()
                .AddIngredient(ItemID.ShroomiteBar, 10)
                .AddRecipeGroup(RecipeGroupID.IronBar, 15)
                .AddRecipeGroup(nameof(ItemID.CopperBar), 15)
                .Register();
        }
        public class HollowPointProjectile : UpgradeGlobalProjectile<HollowPointRounds>
        {
            public override void UpgradeOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(ModContent.BuffType<StunnedEnemy>(), 120);
            }
        }
    }
}
