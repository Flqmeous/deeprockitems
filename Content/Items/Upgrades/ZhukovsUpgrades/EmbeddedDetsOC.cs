using deeprockitems.Common.NPCs;
using deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades;
using deeprockitems.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Upgrades.ZhukovsUpgrades
{
    public class EmbeddedDetsOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            /*Recipe upgrade = Recipe.Create(ModContent.ItemType<CryoMineletsOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.FrostCore, 3)
            .AddIngredient(ItemID.Grenade, 15)
            .AddTile(TileID.Anvils);
            upgrade.Register();*/
        }
        public override void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem)
        {
            // Always true
            if (modItem is Zhukovs zhukovs)
            {
                zhukovs.CanAltUse = true;
                zhukovs.DamageScale *= 0.75f;
                zhukovs.UseTimeScale *= 1.25f;
            }
        }
        public override void ProjectileOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.TryGetGlobalNPC(out EmbeddedDetsNPC modNPC))
            {
                modNPC.EmbeddedCount++;
                return;
            }
        }
        public override void ItemShootAltUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.TryGetGlobalNPC(out EmbeddedDetsNPC modNPC) && modNPC.EmbeddedCount > 0)
                {
                    modNPC.ExplodeThisNPC(npc, player);
                }
            }
        }
    }
}
