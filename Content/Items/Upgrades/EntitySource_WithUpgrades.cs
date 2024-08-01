using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;

namespace deeprockitems.Content.Items.Upgrades
{
    public interface IEntitySource_FromUpgradeableItem : IEntitySource_WithStatsFromItem
    {
        public UpgradeableItemTemplate UpgradeableItem { get; init; }
        public int[] Upgrades { get => UpgradeableItem.Upgrades; }
    }
    public class EntitySource_WithUpgrades : EntitySource_Parent, IEntitySource_FromUpgradeableItem
    {
        public UpgradeableItemTemplate UpgradeableItem { get; init; }
        public Player Player { get; init; }

        public Item Item { get => UpgradeableItem.Item; }
        public EntitySource_WithUpgrades(Player player, UpgradeableItemTemplate upgradeableItem, string? context = null) : base(player, context)
        {
            Player = player;
            UpgradeableItem = upgradeableItem;
        }
    }
    public static class FromUpgradeableItemExtension
    {
        public static IEntitySource_FromUpgradeableItem GetSource_FromUpgradeableItem(this Projectile projectile, Player player, UpgradeableItemTemplate upgradeableItem, string? context = null)
        {
            return new EntitySource_WithUpgrades(player, upgradeableItem, context);
        }
    }
}
