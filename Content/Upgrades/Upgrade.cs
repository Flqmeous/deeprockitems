using deeprockitems.Content.Items.Weapons;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace deeprockitems.Content.Upgrades
{
    public class Upgrade
    {
        public Upgrade(string internalName, Texture2D sprite)
        {
            InternalName = internalName;
            ID = autoIncrementID++;
            Texture = sprite;
        }
        public bool IsEquipped { get; set; }
        public int ID { get; set; }
        private static int autoIncrementID;
        public readonly string InternalName;
        public Texture2D Texture { get; set; }
        public string LocalizedKey { get; set; }
        public LocalizedText DisplayName { get => Language.GetOrRegister($"{LocalizedKey}.DisplayName", () => InternalName); }
        public LocalizedText HoverText { get => Language.GetOrRegister($"{LocalizedKey}.HoverText", () => "Hover text"); }
        public bool IsUnlocked { get; set; } = false;
        public delegate void ItemStatChangeCallback();
        public delegate void ItemShootHook();
        public delegate void ItemModifyShootStatsHook();
        public Action Item_ModifyStats { get; set; }
        public Action<Projectile, IEntitySource> Projectile_OnSpawnHook { get; set; }
        public Action<Projectile> Projectile_AIHook { get; set; }
        public Action<Projectile, NPC, NPC.HitInfo, int> Projectile_OnHitNPCHook { get; set; }
        public delegate void ProjectileModifyHitNPC(Projectile projectile, NPC npc, NPC.HitModifiers inModifiers);
        public Func<Projectile, NPC, NPC.HitModifiers, NPC.HitModifiers> Projectile_ModifyHitNPCHook { get; set; }
    }
}
