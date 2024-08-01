using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Items.Upgrades.JuryShotgunUpgrades
{
    [ValidWeapons(typeof(JuryShotgun))]
    public class WhitePhosphorus : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<WhitePhosphorus>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.MeteoriteBar, 15)
            .AddIngredient(ItemID.Fireblossom, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
        public override bool UpgradeItem_ShootPrimaryUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool callBase)
        {
            // Ignite all npcs
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly) { continue; }
                if (Vector2.DistanceSquared(Main.player[Main.myPlayer].Center, npc.Center) <= 10000)
                {
                    npc.AddBuff(BuffID.OnFire, 360);
                }
            }

            return base.UpgradeItem_ShootPrimaryUse(sender, player, source, position, velocity, type, damage, knockback, out callBase);
        }
    }
}
