using Terraria;
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
            Panel.Left.Pixels = 73f;
            Panel.Top.Pixels = Main.instance.invBottom;
            Panel.Height.Pixels = 200;
            Panel.Width.Pixels = 420;
            Append(Panel);
        }
    }
}
