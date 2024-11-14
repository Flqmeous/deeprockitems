using deeprockitems.Common.EntitySources;
using System;
using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Upgrades
{
    public class UpgradeBehavior
    {
        public Action<Item> Item_ModifyStats { get; set; }
        public Action<Item, Player> Item_HoldItemHook { get; set; }
        public Action<Projectile, IEntitySource> Projectile_OnSpawnHook { get; set; }
        public Action<Projectile> Projectile_AIHook { get; set; }
        public Action<Projectile, NPC, NPC.HitInfo, int> Projectile_OnHitNPCHook { get; set; }
        public Func<Projectile, Color, bool> Projectile_PreDrawHook { get; set; }
        public Func<Projectile, NPC, NPC.HitModifiers, NPC.HitModifiers> Projectile_ModifyHitNPCHook { get; set; }
        public ProjectilePreKill Projectile_PreKillHook { get; set; }
        public delegate bool ProjectilePreKill(Projectile projectile, int timeLeft);
        // Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback
        public Action<Item, Player, EntitySource_FromUpgradableWeapon, Projectile> Item_OnShoot { get; set; }
        public delegate bool ProjectileOnTileCollide(Projectile projectile, Vector2 oldVelocity);
        public ProjectileOnTileCollide Projectile_OnTileCollideHook { get; set; }
    }
}
