using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using Microsoft.Xna.Framework;
using System;

namespace deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades
{
    public class SludgeExplosionOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<SludgeExplosionOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.CobaltBar, 10)
            .AddIngredient(ItemID.Gel, 15)
            .AddIngredient(ItemID.Bomb, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<SludgeExplosionOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.PalladiumBar, 10)
            .AddIngredient(ItemID.Gel, 15)
            .AddIngredient(ItemID.Bomb, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
        public override bool? UpgradeProjectile_PreKill(Projectile sender, int timeLeft, out bool callBase)
        {
            callBase = true;
            if (sender.ModProjectile is SludgeBall)
            {
                Projectile.NewProjectile(sender.GetSource_FromThis(), sender.Center, sender.velocity, ModContent.ProjectileType<SludgeExplosion>(), (int)Math.Floor(sender.damage * 1.5f), sender.knockBack, sender.owner);
                return false;
            }
            return base.UpgradeProjectile_PreKill(sender, timeLeft, out callBase);
        }
    }
}
