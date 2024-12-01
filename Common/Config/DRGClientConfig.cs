using System.ComponentModel;
using Terraria.ModLoader.Config;
using deeprockitems.Common.Config.UIElements;

namespace deeprockitems.Common.Config
{
    public class DRGClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;


        [Header("UI")]

        [DefaultValue(TemperatureOptions.Vanilla)]
        public TemperatureOptions temperatureDisplaySetting;
    }
    [CustomModConfigItem(typeof(SelectBox<TemperatureOptions>))]
    public enum TemperatureOptions
    {
        Vanilla = 0,
        Fancy,
    }
}
