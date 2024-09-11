using Terraria.UI;

namespace deeprockitems.UI.CooldownUI
{
    public class PlayerCooldownUIState : UIState
    {
        public CooldownMeter PlayerCooldownMeter { get; set; }
        public PlayerCooldownUIState() {
            // Initialize position and width
            PlayerCooldownMeter = new();
            PlayerCooldownMeter.HAlign = PlayerCooldownMeter.VAlign = 0.5f;

            Append(PlayerCooldownMeter);
        }
    }
}
