using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmartFormat.Core.Parsing;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace deeprockitems.Common.Config.UIElements
{
    public class SelectBox<TEnum> : ConfigElement where TEnum : struct, Enum
    {
        #region Serialization for config
        public void SetValue(TEnum value) => SetObject(value);
        public TEnum GetValue() => (TEnum)GetObject();
        #endregion
        #region State handling
        TEnum[] options;
        public bool IsExpanded { get; set; } = false;
        #endregion
        float computedWidth;
        public SelectBox() {
            // Get all possible options for the select box
            options = (TEnum[])typeof(TEnum).GetEnumValues();

            // compute size for the hitbox area
            float textWidth = options.Max(option => FontAssets.MouseText.Value.MeasureString(typeof(TEnum).GetEnumName(option)).X);
            float buttonWidth = Assets.UI.SelectButton.Value.Width;
            computedWidth = 24f + buttonWidth + textWidth;
            OnLeftClick += SelectBox_OnLeftClick;
        }
        private void SelectBox_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
            // Either expand or collapse the options depending on the state
            if (!IsExpanded)
            {
                ShowOptions();
            }
            else
            {
                HideOptions();
            }
        }
        private SelectBoxDropdown<TEnum> optionsPanel;
        public void ShowOptions() {
            IsExpanded = true;

            // Draw a new panel to showcase all the options, height gets set automatically but it requires a width
            // Get dimensions
            var width = GetDimensions().Width;
            optionsPanel = new SelectBoxDropdown<TEnum>(Left.Pixels, Top.Pixels, width, options, this);
            Parent.Parent.Append(optionsPanel);
        }
        public void HideOptions() {
            optionsPanel.Remove();
            optionsPanel = null;
            IsExpanded = false;
        }
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            var dimensions = GetDimensions().ToRectangle();
            // Get where we actually want to draw it at
            var elementPos = new Vector2(dimensions.X + dimensions.Width - computedWidth - 48f, dimensions.Y + (0.5f * dimensions.Height - 0.75f * 0.5f * dimensions.Height));

            // Draw the visual indicator for the element
            DRGHelpers.DrawPanel(spriteBatch, Assets.UI.SelectBox.Value, 4, 4, elementPos, computedWidth, dimensions.Height * 0.75f, Color.White);

            // Draw text
            string text = typeof(TEnum).GetEnumName(GetValue()) ?? "";
            Vector2 offset = Utils.DrawBorderString(spriteBatch, text, new Vector2(elementPos.X + 6f, elementPos.Y), Color.White);

            // Draw button
            spriteBatch.Draw(Assets.UI.SelectButton.Value, new Vector2(elementPos.X + computedWidth - Assets.UI.SelectButton.Width() - 6f, elementPos.Y + 2f), Color.White);
        }
    }
}