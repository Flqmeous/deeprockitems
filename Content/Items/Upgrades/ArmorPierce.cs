using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades
{
    public class ArmorPierce : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<ArmorPierce>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.CobaltBar, 15)
            .AddIngredient(ItemID.SharkToothNecklace)
            .AddTile(TileID.Anvils);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<ArmorPierce>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.PalladiumBar, 15)
            .AddIngredient(ItemID.SharkToothNecklace)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
        public class ArmorPierceProjectile : UpgradeGlobalProjectile<ArmorPierce>
        {
            public override void UpgradeModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
            {
                modifiers.ArmorPenetration += 10;
            }
        }
    }
}
