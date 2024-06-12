using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using deeprockitems.Content.Items.Weapons;
using Terraria.UI;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.UI.UpgradeItem;
using Terraria.DataStructures;
using SmartFormat.Core.Extensions;

namespace deeprockitems.Content.Items.Upgrades
{
    public abstract class UpgradeTemplate : ModItem
    {
        public virtual bool IsOverclock { get; set; }   
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.width = Item.height = 30;
            Item.value = IsOverclock ? Item.buyPrice(0, 5, 0, 0) : Item.buyPrice(0, 3, 0, 0);
        }
        public override void SetStaticDefaults()
        {
            UpgradeProjectiles.ProjectileSpawned += UpgradeProjectiles_ProjectileSpawned;
            UpgradeProjectiles.ProjectileKilled += UpgradeProjectiles_ProjectileKilled;
        }
        private void UpgradeProjectiles_ProjectileSpawned(Projectile sender, IEntitySource source, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                ProjectileOnSpawnWhenEquipped(sender, source);
            }
        }
        public virtual void ProjectileOnSpawnWhenEquipped(Projectile projectile, IEntitySource source) { }
        private void UpgradeProjectiles_ProjectileKilled(Projectile sender, int timeLeft, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                ProjectileOnKillWhenEquipped(sender, timeLeft);
            }
        }
        public virtual void ProjectileOnKillWhenEquipped(Projectile projectile, int timeLeft) { }

        public override bool CanStack(Item item2)
        {
            return false;
        }
        /// <summary>
        /// Used for drawing the slot to show valid upgrades. Don't touch.
        /// </summary>
        private uint _drawTimer = 0;
        
    }
}