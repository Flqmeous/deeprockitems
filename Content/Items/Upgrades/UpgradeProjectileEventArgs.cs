using System;
using Terraria;
using Terraria.DataStructures;

namespace deeprockitems.Content.Items.Upgrades
{
    public class UpgradeProjectileEventArgs : EventArgs
    {
        public int TimeLeft { get; set; }
        public int[] Upgrades { get; set; }
        public bool? ShouldContinue { get; set; } = null;
    }
}
