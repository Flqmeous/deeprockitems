using System.ComponentModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using deeprockitems.Common.Config.UIElements;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace deeprockitems.Common.Config
{
    public class DRGClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;


        [Header("UI")]

        [DefaultValue(TemperatureOptions.Vanilla)]
        public TemperatureOptions temperatureDisplaySetting;
    }
    [CustomModConfigItem(typeof(SelectBoxParent<TemperatureOptions>))]
    public enum TemperatureOptions
    {
        Vanilla = 0,
        Fancy,
    }
}
