﻿using Terraria;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeState : UIState
    {
        public UpgradePanel Panel;
        public override void OnInitialize()
        {
            Panel = new();
            // Set size and location
            Panel.Left.Pixels = 100;
            Panel.Top.Pixels = 300;
            Panel.Height.Pixels = 300;
            Panel.Width.Pixels = 800;
            Append(Panel);
        }
    }
}
