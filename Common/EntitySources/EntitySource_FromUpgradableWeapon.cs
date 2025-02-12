﻿using deeprockitems.Content.Items.Weapons;
using Terraria;
using Terraria.DataStructures;

namespace deeprockitems.Common.EntitySources
{
    public class EntitySource_FromUpgradableWeapon : EntitySource_ItemUse_WithAmmo
    {
        public new UpgradableWeapon Item { get => _item; }
        public UpgradableWeapon _item;
        public EntitySource_FromUpgradableWeapon(Player player, UpgradableWeapon item, int ammoItemId, string context = null) : base(player, item.Item, ammoItemId, context) {
            _item = item;
        }
    }
}
