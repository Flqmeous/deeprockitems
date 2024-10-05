using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Utilities;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using System;

namespace deeprockitems.Common.Config.UIElements
{
    public class SelectBoxDropdown<TEnum> : UIPanel where TEnum : struct, Enum
    {
        TEnum[] _values;
        SelectBoxOption<TEnum>[] children;
        public SelectBox<TEnum> ParentBox;
        public SelectBoxDropdown(float x, float y, float width, TEnum[] options, SelectBox<TEnum> parent) {

            // Calculate height of the box
            const int TEXT_HEIGHT = 24;
            const int PADDING = 4;

            // Total height for panel
            int height = options.Length * TEXT_HEIGHT + (options.Length - 1) * PADDING;

            // Get longest option
            float computedWidth = 24f + options.Max(option => FontAssets.MouseText.Value.MeasureString(typeof(TEnum).GetEnumName(option)).X);


            // set dimensions
            Left.Pixels = x + width - 48f - computedWidth;
            Top.Pixels = y + height + 8f;
            Width.Pixels = computedWidth;
            Height.Pixels = height;
            // Pass parent for purposes of setting values
            ParentBox = parent;
            SetPadding(4f);

            // Set options
            _values = options;

            // Set children
            children = new SelectBoxOption<TEnum>[options.Length];
            float heightPerChild = height / children.Length;
            var values = typeof(TEnum).GetEnumValues();
            for (int i = 0; i < values.Length; i++)
            {
                children[i] = new SelectBoxOption<TEnum>((TEnum)values.GetValue(i));
                children[i].Left.Pixels = 0f;
                children[i].Top.Pixels = i * heightPerChild;
                children[i].Width = Width;
                children[i].Height.Pixels = heightPerChild;
                Append(children[i]);
            }
        }
        public override void Draw(SpriteBatch spriteBatch) {
            // Draw panel at this position
            var dimensions = GetDimensions().ToRectangle();
            DRGHelpers.DrawPanel(spriteBatch, TextureAssets.InventoryBack9.Value, 8, 8, new(dimensions.X, dimensions.Y), dimensions.Width, dimensions.Height, Color.White);
            // draw children
            foreach (var child in Children)
            {
                child.Draw(spriteBatch);
            }
        }
    }
}
