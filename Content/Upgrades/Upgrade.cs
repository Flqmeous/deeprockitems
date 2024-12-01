﻿using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;

namespace deeprockitems.Content.Upgrades
{
    public class Upgrade {
        public Upgrade(string internalName, Texture2D sprite) {
            InternalName = internalName;
            Texture = sprite;
            Behavior = new();
            Recipe = new();
            UpgradeState = new UpgradeStateBinding() {
                IsEquipped = false,
                IsUnlocked = false,
            };
        }
        public readonly string InternalName;
        public Texture2D Texture { get; set; }
        public string LocalizedKey { get; set; }
        public LocalizedText DisplayName { get => Language.GetOrRegister($"{LocalizedKey}.DisplayName", () => InternalName); }
        public LocalizedText HoverText { get => Language.GetOrRegister($"{LocalizedKey}.HoverText", () => "Hover text"); }
        public UpgradeStateBinding UpgradeState { get; set; }
        public UpgradeBehavior Behavior { get; set; }
        public UpgradeRecipe Recipe { get; set; }
    }
}
