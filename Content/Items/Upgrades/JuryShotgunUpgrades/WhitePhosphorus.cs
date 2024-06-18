using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Items.Upgrades.JuryShotgunUpgrades
{
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
            .AddIngredient(ItemID.HellstoneBar, 15)
            .AddIngredient(ItemID.Fireblossom, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
        public override void ItemShootPrimaryUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.friendly) { continue; }
                if (Vector2.DistanceSquared(Main.player[Main.myPlayer].Center, npc.Center) <= 10000)
                {
                    npc.AddBuff(BuffID.OnFire, 360);
                }
            }
        }
    }
}
