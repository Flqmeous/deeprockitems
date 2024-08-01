using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using System.Linq;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades
{
    [ValidWeapons(typeof(PlasmaPistol))]
    public class HeavyHitterOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<HeavyHitterOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddIngredient(ItemID.FallenStar, 15)
            .AddIngredient(ItemID.Deathweed, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
        public override void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem)
        {
            modItem.UseTimeScale += 0.5f;
            modItem.Item.channel = false;
        }
        public class HeavyHitterProjectile : UpgradeGlobalProjectile<HeavyHitterOC>
        {
            public override void UpgradeOnSpawn(Projectile projectile, IEntitySource source)
            {
                if (projectile.ModProjectile is PlasmaPistolHelper modProj)
                {
                    modProj.ProjectileToSpawn = ModContent.ProjectileType<BigPlasma>();
                    modProj.Projectile.damage = modProj.Projectile.damage * 3;
                }
            }
        }
    }
}
