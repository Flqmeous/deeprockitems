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
using deeprockitems.Utilities;
using Microsoft.Build.Utilities;

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
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (IsOverclock)
            {
                string positive = this.GetLocalizedValue("Positives");
                string negative = this.GetLocalizedValue("Negatives");
                TooltipHelpers.InsertTooltip(tooltips, new TooltipLine(Mod, "UpgradeEffects", positive + (negative == "" ? "" : "\n" + negative)), "OneDropLogo");
            }
        }
        public override void SetStaticDefaults()
        {
            /*UpgradeProjectile.ProjectileSpawned += UpgradeProjectile_OnSpawn;
            UpgradeProjectile.ProjectileAI += UpgradeProjectile_AI;
            UpgradeProjectile.ProjectileHitNPC += UpgradeProjectile_OnHitNPC;
            UpgradeProjectile.ProjectileModifyNPC += UpgradeProjectile_ModifyHitNPC;
            UpgradeProjectile.ProjectileHitTile += UpgradeProjectile_OnTileCollide;
            UpgradeProjectile.ProjectileKilled += UpgradeProjectile_PreKill;
            UpgradeProjectile.ProjectileSendData += UpgradeProjectile_SyncData;*/
            UpgradeableItemTemplate.ItemStatChange += UpgradeableItemTemplate_ItemStatChangeOnEquip;
            UpgradeableItemTemplate.ItemStatChange += UpgradeableItemTemplate_ItemStatChangeOnRemove;
            UpgradeableItemTemplate.ItemShootPrimaryUse += UpgradeItem_ShootPrimaryUse;
            UpgradeableItemTemplate.ItemShootAltUse += UpgradeItem_ShootAltUse;
            UpgradeableItemTemplate.ItemModifyShootPrimaryUse += UpgradeItem_ModifyShootStatsPrimary;
            UpgradeableItemTemplate.ItemModifyShootAltUse += UpgradeItem_ModifyShootStatsPrimary;

            UpgradeableItemTemplate.ItemHold += UpgradeableItemTemplate_ItemHold;
        }
        public abstract class UpgradeGlobalProjectile<T> : GlobalProjectile where T : UpgradeTemplate
        {
            protected int _type { get => ModContent.ItemType<T>(); }

            public override bool InstancePerEntity => true;
            public bool UpgradeEquipped { get; set; } = false;
            public sealed override void OnSpawn(Projectile projectile, IEntitySource source)
            {
                if (source is EntitySource_ItemUse_WithAmmo { Item.ModItem: UpgradeableItemTemplate item })
                {
                    if (item.Upgrades.Contains(_type))
                    {
                        UpgradeEquipped = true;
                        UpgradeOnSpawn(projectile, source);
                    }
                }
            }
            public virtual void UpgradeOnSpawn(Projectile projectile, IEntitySource source) { }
            public sealed override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (UpgradeEquipped)
                {
                    UpgradeOnHitNPC(projectile, target, hit, damageDone);
                }
            }
            public virtual void UpgradeModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) { }
            public sealed override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
            {
                if (UpgradeEquipped)
                {
                    UpgradeModifyHitNPC(projectile, target, ref modifiers);
                }
            }
            public override sealed void AI(Projectile projectile)
            {
                if (UpgradeEquipped)
                {
                    UpgradeAI(projectile);
                }
            }
            public sealed override bool PreKill(Projectile projectile, int timeLeft)
            {
                if (UpgradeEquipped)
                {
                    return UpgradePreKill(projectile, timeLeft);
                }
                return base.PreKill(projectile, timeLeft);
            }
            public virtual bool UpgradePreKill(Projectile projectile, int timeLeft) => true;
            public override sealed void OnKill(Projectile projectile, int timeLeft)
            {
                if (UpgradeEquipped)
                {
                    UpgradeOnKill(projectile, timeLeft);
                }
            }
            public virtual void UpgradeOnKill(Projectile projectile, int timeLeft) { }
            public virtual void UpgradeAI(Projectile projectile) { }
            public virtual void UpgradeOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) { }
            public sealed override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
            {
                if (UpgradeEquipped)
                {
                    return UpgradeOnTileCollide(projectile, oldVelocity);
                }
                return base.OnTileCollide(projectile, oldVelocity);
            }
            public virtual bool UpgradeOnTileCollide(Projectile projectile, Vector2 oldVelocity) => true;
        }

        public virtual void UpgradeProjectile_SyncData(Projectile sender, Dictionary<string, object> data) { }

        #region Public event virtual methods
        public virtual void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem) { }
        public virtual void ItemStatChangeOnRemove(UpgradeableItemTemplate modItem) { }
        public virtual bool UpgradeItem_ShootPrimaryUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool callBase)
        {
            callBase = true;
            return true;
        }
        public virtual void ItemHold(UpgradeableItemTemplate sender, Player player) { }
        public virtual bool UpgradeItem_ShootAltUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool callBase)
        {
            callBase = true;
            return true;
        }
        /*public virtual void ItemModifyShootPrimaryUse(UpgradeableItemTemplate sender, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
        public virtual void ItemModifyShootAltUse(UpgradeableItemTemplate sender, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }*/
        #endregion
        #region Private event handlers
        public virtual void UpgradeProjectile_OnSpawn(Projectile sender, IEntitySource source) { }
        public virtual void UpgradeProjectile_AI(Projectile sender) { }
        public virtual void UpgradeProjectile_OnHitNPC(Projectile sender, NPC target, NPC.HitInfo hit, int damageDone) { }
        public virtual void UpgradeProjectile_ModifyHitNPC(Projectile sender, NPC target, ref NPC.HitModifiers modifiers) { }
        public virtual bool? UpgradeProjectile_OnTileCollide(Projectile sender, Vector2 oldVelocity, out bool callBase)
        {
            callBase = true;
            return null;
        }
        public virtual bool? UpgradeProjectile_PreKill(Projectile sender, int timeLeft, out bool callBase)
        {
            callBase = true;
            return null;
        }
        public virtual void UpgradeItem_ModifyShootStatsPrimary(UpgradeableItemTemplate sender, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, out bool callBase)
        {
            callBase = true;
        }
        public virtual void UpgradeItem_ModifyShootStatsAlt(UpgradeableItemTemplate sender, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, out bool callBase)
        {
            callBase = true;
        }
        private void UpgradeableItemTemplate_ItemStatChangeOnEquip(UpgradeableItemTemplate sender, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                ItemStatChangeOnEquip(sender);
            }
        }
        private void UpgradeableItemTemplate_ItemStatChangeOnRemove(UpgradeableItemTemplate sender, int[] upgrades)
        {
            if (!upgrades.Contains(Item.type))
            {
                ItemStatChangeOnRemove(sender);
            }
        }
        /*        private void UpgradeableItemTemplate_ItemModifyShootPrimaryUse(UpgradeableItemTemplate sender, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, int[] upgrades)
                {
                    if (upgrades.Contains(Item.type))
                    {
                        ItemModifyShootPrimaryUse(sender, player, ref position, ref velocity, ref type, ref damage, ref knockback);
                    }
                }
                private void UpgradeableItemTemplate_ItemModifyShootAltUse(UpgradeableItemTemplate sender, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, int[] upgrades)
                {
                    if (upgrades.Contains(Item.type))
                    {
                        ItemModifyShootAltUse(sender, player, ref position, ref velocity, ref type, ref damage, ref knockback);
                    }
                }*/
        private void UpgradeableItemTemplate_ItemHold(UpgradeableItemTemplate sender, Player player, int[] upgrades)
        {
            if (upgrades.Contains(Item.type))
            {
                ItemHold(sender, player);
            }
        }
        #endregion
    }
}