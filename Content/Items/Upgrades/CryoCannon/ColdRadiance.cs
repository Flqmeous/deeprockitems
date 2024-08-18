using deeprockitems.Content.Buffs;
using deeprockitems.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Upgrades.CryoCannon
{
    [ValidWeapons([
        typeof(Weapons.CryoCannon)
        ])]
    public class ColdRadiance : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override bool UpgradeItem_ShootPrimaryUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool callBase)
        {
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly) { continue; }
                if (Vector2.DistanceSquared(player.Center, npc.Center) <= 10000)
                {
                    npc.ChangeTemperature(-4, player.whoAmI);
                }
            }
            return base.UpgradeItem_ShootPrimaryUse(sender, player, source, position, velocity, type, damage, knockback, out callBase);
        }

    }
}
