using deeprockitems.Common.NPCs;
using deeprockitems.Content.Items.Misc;
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
    [ValidWeapons(typeof(Zhukovs))]
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
            Recipe.Create(ModContent.ItemType<EmbeddedDetsOC>())
                .AddIngredient<MatrixCore>()
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

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
        public override void UpgradeProjectile_OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.TryGetGlobalNPC(out EmbeddedDetsNPC modNPC))
            {
                modNPC.EmbeddedCount++;
                return;
            }
        }
        public override bool UpgradeItem_ShootAltUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool callBase)
        {
            // Explode embedded npcs
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.TryGetGlobalNPC(out EmbeddedDetsNPC modNPC) && modNPC.EmbeddedCount > 0)
                {
                    modNPC.ExplodeThisNPC(npc, player);
                }
            }
            return base.UpgradeItem_ShootAltUse(sender, player, source, position, velocity, type, damage, knockback, out callBase);
        }
        public class EmbeddedDetsProjectile : UpgradeGlobalProjectile<EmbeddedDetsOC>
        {
            public override void UpgradeOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (target.TryGetGlobalNPC(out EmbeddedDetsNPC npc))
                {
                    npc.EmbeddedCount++;
                }
            }
        }
    }
}
