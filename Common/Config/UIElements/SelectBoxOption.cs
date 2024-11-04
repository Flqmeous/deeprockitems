using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace deeprockitems.Common.Config.UIElements
{
    public class SelectBoxOption<TEnum> : UIElement, ILoadable where TEnum : struct, Enum
    {
        TEnum _value;
        public SelectBoxOption(TEnum value) {
            // pass value
            _value = value;
            OnLeftClick += SelectBoxOption_OnLeftClick;
            _ = Name;
            _ = Tooltip;
        }
        public LocalizedText Name { get => Language.GetOrRegister($"Mods.deeprockitems.Configs.{typeof(TEnum).Name}.{typeof(TEnum).GetEnumName(_value)}.Label"); }
        public LocalizedText Tooltip { get => Language.GetOrRegister($"Mods.deeprockitems.Configs.{typeof(TEnum).Name}.{typeof(TEnum).GetEnumName(_value)}.Tooltip"); }
        private void SelectBoxOption_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
            if (Parent is SelectBoxDropdown<TEnum> box)
            {
                box.ParentBox.SetValue(_value);
                box.ParentBox.HideOptions();
            }
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            var dimensions = GetDimensions().ToRectangle();
            // Draw
            string text = typeof(TEnum).GetEnumName(_value) ?? "";
            Vector2 offset = Utils.DrawBorderString(spriteBatch, Name.Value, new Vector2(dimensions.X + 4f, dimensions.Y), Color.White);
            if (IsMouseHovering)
            {
                UICommon.TooltipMouseText(Tooltip.Value);
            }
        }

        public void Load(Mod mod) {
            _ = Name;
            _ = Tooltip;
        }
        public void Unload() {
        }
    }
}
