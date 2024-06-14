﻿using Terraria;
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
        public override bool CanStack(Item item2)
        {
            return false;
        }
        /// <summary>
        /// Used for drawing the slot to show valid upgrades through IL edits. Don't touch.
        /// </summary>
        private uint _drawTimer = 0;
        public override void SetStaticDefaults()
        {
            UpgradeProjectile.ProjectileSpawned += UpgradeProjectile_OnSpawn;
            UpgradeProjectile.ProjectileAI += UpgradeProjectile_AI;
            UpgradeProjectile.ProjectileCanDamage += UpgradeProjectile_CanDamage;
            UpgradeProjectile.ProjectileHitNPC += UpgradeProjectile_OnHitNPC;
            UpgradeProjectile.ProjectileModifyNPC += UpgradeProjectile_ModifyHitNPC;
            UpgradeProjectile.ProjectileHitTile += UpgradeProjectile_OnTileCollide;
            UpgradeProjectile.ProjectileKilled += UpgradeProjectile_OnKill;
            UpgradeableItemTemplate.ItemStatChange += UpgradeableItemTemplate_ItemStatChange;
        }

        #region Public event virtual methods
        public virtual void ProjectileOnSpawn(Projectile projectile, IEntitySource source) { }
        public virtual void ProjectileAI(Projectile projectile) { }
        public virtual bool? ProjectileCanDamage(Projectile projectile) => null;
        public virtual void ProjectileOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) { }
        public virtual void ProjectileModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) { }
        public virtual bool? ProjectileOnTileCollide(Projectile projectile, Vector2 oldVelocity) => null;
        public virtual void ProjectileOnKill(Projectile projectile, int timeLeft) { }
        public virtual void ItemStatChange(UpgradeableItemTemplate modItem) { }
        #endregion
        #region Private event handlers
        private void UpgradeProjectile_OnSpawn(Projectile sender, IEntitySource source, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                ProjectileOnSpawn(sender, source);
            }
        }
        private void UpgradeProjectile_AI(Projectile sender, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                ProjectileAI(sender);
            }
        }
        private bool? UpgradeProjectile_CanDamage(Projectile sender, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                return ProjectileCanDamage(sender);
            }
            return null;
        }
        private void UpgradeProjectile_OnHitNPC(Projectile sender, NPC target, NPC.HitInfo hit, int damageDone, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                ProjectileOnHitNPC(sender, target, hit, damageDone);
            }
        }
        private void UpgradeProjectile_ModifyHitNPC(Projectile sender, NPC target, ref NPC.HitModifiers modifiers, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                ProjectileModifyHitNPC(sender, target, ref modifiers);
            }
        }
        private bool? UpgradeProjectile_OnTileCollide(Projectile sender, Vector2 oldVelocity, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                return ProjectileOnTileCollide(sender, oldVelocity);
            }
            return null;
        }
        private void UpgradeProjectile_OnKill(Projectile sender, int timeLeft, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                ProjectileOnKill(sender, timeLeft);
            }
        }
        private void UpgradeableItemTemplate_ItemStatChange(UpgradeableItemTemplate sender, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                ItemStatChange(sender);
            }
        }
        #endregion
    }
}