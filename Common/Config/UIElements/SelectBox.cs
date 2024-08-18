/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using deeprockitems.Utilities;
using Terraria.ModLoader;
using System;
using deeprockitems.Assets.Textures;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using System.Linq;

namespace deeprockitems.Common.Config.UIElements
{
    public class TemperatureConfig : ConfigElement
    {
        public SelectBox<TemperatureOptions> SelectBox { get; set; }
        public override void OnInitialize()
        {
            SelectBox = new();
            //Height.Set(2f * Height.Pixels, 1f);
            OverflowHidden = false;
            Append(SelectBox);
        }
    }
    public class SelectBox<TEnum> : ConfigElement where TEnum : struct, Enum
    {
        public CalculatedStyle Dimensions => GetDimensions();
        private Option[] _options = new Option[typeof(TEnum).GetEnumValues().Length];
        public DynamicSpriteFont Font { get; set; } = FontAssets.ItemStack.Value;
        public TEnum SelectedValue { get; set; } = (TEnum)typeof(TEnum).GetEnumValues().GetValue(0);
        public string SelectedText { get => Language.GetText($"Mods.deeprockitems.Configs.{typeof(TEnum).Name}.Value_{typeof(TEnum).GetEnumName(SelectedValue)}").ToString(); }
        public bool IsExpanded { get; set; } = false;
        public override void OnInitialize()
        {
            CalculatedStyle parentDimensions = Parent.GetDimensions();
            // Set list of measured strings
            Vector2[] measuredStrings = new Vector2[_options.Length];
            // Initialize Options and set sizes
            for (int i = 0; i < _options.Length; i++)
            {
                _options[i] = new(this, (TEnum)typeof(TEnum).GetEnumValues().GetValue(i));
                measuredStrings[i] = Font.MeasureString(_options[i].Text);
            }
            // To know how long this box has to be, we have to do some math :(
            float rightEdgeWithMargin = parentDimensions.Width - 24f; // Margin + padding included
            Vector2 stringSize = measuredStrings.OrderBy(v => v.X).Last(); // Get longest string, we'll use this to make the width of the box
            float leftEdge = rightEdgeWithMargin - stringSize.X - 4f; // Factor in 2 pixel padding. We finally have the left edge!

            // Start shrinking this box down.
            Left = new(leftEdge, 1f);
            Top = new(0, 1f);
            Width = new(stringSize.X + 4f, 1f);
            Height = new(parentDimensions.Height, 1f);
            SetPadding(2f); // Add in padding
            OverflowHidden = false;
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            // Open the select box
            if (!IsExpanded)
            {
                ExpandOptions();
                IsExpanded = true;
                return;
            }

            // Find which box was clicked and pass left click to there
            foreach (var option in _options)
            {
                if (option.Bounds.Contains(evt.MousePosition.ToPoint()))
                {
                    option.LeftClick(evt);
                    CollapseOptions();
                    return;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsExpanded)
            {
                CalculatedStyle dimensions = GetDimensions();
                DrawBoxWithText(spriteBatch, DRGTextures.WhitePixel.Value, Font, SelectedText, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 2, Color.White, Color.White);
            }
        }
        *//*public override void OnBind()
        {
            base.OnBind();
            //Dimensions = new((int)GetDimensions().X + 600, (int)GetDimensions().Y + 30, 300, (int)GetDimensions().Height - 60);
            _buttonTexture = this.RequestFromNamespace<Texture2D>("SelectButton").Value;
            var values = Enum.GetValues<TEnum>();
            for (int i = 0; i < values.Length; i++)
            {
                _options[i] = new(this, values[i]);
            }
        }
        public TEnum Value { get; set; }
        Texture2D _buttonTexture;
        Option[] _options = new Option[Enum.GetValues<TEnum>().Length];
        public bool Expanded { get; set; } = false;
        public int OldHeight { get; set; }
        public override void LeftClick(UIMouseEvent evt)
        {
            if (!Expanded)
            {
                Expanded = true;
                // Now we can expand the bounds of this UI element
                OldHeight = (int)Dimensions.Height;
                Height = new(_options.Length * OldHeight, 1f);
                // Resize each child

                var heightPerOption = Dimensions.Height;
                for (int i = 0; i < _options.Length; i++)
                {
                    //_options[i].Bounds = new((int)Dimensions.X, (int)Dimensions.Y + (int)heightPerOption * i, (int)Dimensions.Width - _buttonTexture.Width, (int)heightPerOption);
                }
                return;
            }

            // Unexpand
            Height.Set(OldHeight, 1f);
            Expanded = false;

            // Determine which option was clicked
            foreach (var option in _options)
            {
                if (option.GetDimensions().ToRectangle().Contains(evt.MousePosition.ToPoint())) // If mouse position was contained, click and return.
                {
                    option.LeftClick(evt);
                    break;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (Expanded)
            {
                foreach (var child in _options)
                {
                    child.Draw(spriteBatch);
                }
            }
            else
            {
                // Draw box
                DrawBoxWithText(spriteBatch, DRGTextures.WhitePixel.Value, FontAssets.ItemStack.Value, nameof(Value), (int)Dimensions.X, (int)Dimensions.Y, (int)Dimensions.Width, (int)Dimensions.Height, 2, Color.White, Color.White);
                // Draw select icon
                spriteBatch.Draw(_buttonTexture, new Rectangle((int)Left.Pixels + (int)Dimensions.Width - _buttonTexture.Width, (int)Top.Pixels + (int)Dimensions.Height - _buttonTexture.Height, _buttonTexture.Width, _buttonTexture.Height), Color.White);
            }
        }*//*
        public StyleDimension OldHeight { get; set; }
        public void CollapseOptions()
        {
            if (_options.Length != 0)
            {
                // Reset height
                Height = OldHeight;
            }
        }
        public void ExpandOptions()
        {
            var bounds = GetDimensions();
            // Set size of children
            for (int i = 0; i < _options.Length; i++)
            {
                _options[i].Bounds = new((int)bounds.X, (int)(bounds.Y + i * bounds.Height), (int)bounds.Width, (int)bounds.Height);
            }

            // Set size of self
            OldHeight = new(bounds.Height, 1f);
            Height = new(_options.Length * bounds.Height, 1f);
        }
        public static void DrawBoxWithText(SpriteBatch spriteBatch, Texture2D texture, DynamicSpriteFont font, string text, int x, int y, int width, int height, int thickness, Color boxColor, Color textColor)
        {
            // Draw rectangle
            DrawRectangle(spriteBatch, texture, x, y, width, height, thickness, boxColor);
            // Calculate offset
            Vector2 offset = font.MeasureString(text);
            // Draw string with offset
            spriteBatch.DrawString(font, text, new Vector2(x + 2f, y + 2f), textColor);

        }
        public static void DrawRectangle(SpriteBatch spriteBatch, Texture2D texture, int x, int y, int width, int height, int thickness, Color color)
        {
            // Top edge
            spriteBatch.Draw(texture, new Rectangle(x, y, width, thickness), color);
            // Right edge
            spriteBatch.Draw(texture, new Rectangle(x + width - thickness, y, thickness, height), color);
            // Bottom edge
            spriteBatch.Draw(texture, new Rectangle(x, y + height - thickness, width, thickness), color);
            // Left edge
            spriteBatch.Draw(texture, new Rectangle(x, y, thickness, height), color);
        }
        public class Option
        {
            public Option(SelectBox<TEnum> parent, TEnum value)
            {
                Parent = parent;
                Value = value;
            }
            public TEnum Value { get; set; }
            public string Text { get => Language.GetText($"Mods.deeprockitems.Configs.{typeof(TEnum).Name}.Value_{typeof(TEnum).GetEnumName(Value)}").ToString(); }
            public SelectBox<TEnum> Parent { get; set; }
            public Rectangle Bounds { get; set; }
            public void LeftClick(UIMouseEvent evt)
            {
                // Deselect all options
                if (!Parent.IsExpanded) return;

                Parent.SelectedValue = Value;
                Parent.IsExpanded = false;
                Parent.Height = new(2 * Parent.Height.Pixels, 1f);
            }
            public void Draw(SpriteBatch spriteBatch)
            {
                if (Parent.IsExpanded)
                {
                    var dimensions = Bounds;
                    DrawBoxWithText(spriteBatch, DRGTextures.WhitePixel.Value, Parent.Font, Text, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 2, Color.White, Color.White);
                }
            }
        }
    }
}
*/


using System;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.Config.UI;
using deeprockitems.Assets.Textures;
using ReLogic.Graphics;
using Terraria.Localization;
using Terraria.GameContent;
using System.Linq;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria;
using deeprockitems.Utilities;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;

namespace deeprockitems.Common.Config.UIElements
{
    /// <summary>
    /// This element exists to serve as a "parent" for the select box that will control which temperature display is in use.
    /// </summary>
    public class SelectBoxParent<TEnum> : ConfigElement where TEnum : struct, Enum
    {
        SelectBox ChildSelectBox;
        public SelectBoxParent()
        {
            SetPadding(4f);
            // Create new selectbox to place on here
            ChildSelectBox = new();
            Width.Percent = 1f;

            ChildSelectBox.Left.Percent = 0.66f;
            ChildSelectBox.Height.Pixels = Height.Pixels;
            // Resize height once select box is fixed
            Height.Pixels = Height.Pixels * ChildSelectBox.NumberOfOptions;
            Append(ChildSelectBox);
        }
        #region saving and loading json
        void SetValue(TEnum value) => SetObject(value);

        TEnum GetValue() => (TEnum)GetObject();
        #endregion
        public override void OnBind()
        {
            base.OnBind();
            /*TooltipFunction = () =>
            {
                // Get index
                int index = SelectBox._options.FindFirstIndexOf(this);
                if (parent._options.IndexInRange(index))
                {
                    return Language.GetText($"Mods.deeprockitems.Configs.{typeof(TEnum).Name}.{typeof(TEnum).GetEnumName(Value)}.Tooltip").ToString();
                }
                return "";
            };*/
        }
        class SelectBox : ConfigElement
        {
            /// <summary>
            /// Whether this select box is expanded and an option can be selected
            /// </summary>
            public bool IsExpanded { get; set; }
            /// <summary>
            /// The array of available options. These are automatically created from TEnum's available enums.
            /// </summary>
            Option[] _options;
            Option _selectedOption => _options.Where(o => o.Value.Equals((Parent as SelectBoxParent<TEnum>).GetValue())).First();
            public int NumberOfOptions => _options.Length;
            public int TextMinHeight { get; set; } = 0;
            public int TextMinWidth { get; set; } = 0;
            public Texture2D TextureButton { get; set; } = ModContent.Request<Texture2D>("deeprockitems/Common/Config/UIElements/SelectBox", AssetRequestMode.ImmediateLoad).Value;
            public Texture2D TextureIcon { get; set; } = ModContent.Request<Texture2D>("deeprockitems/Common/Config/UIElements/SelectButton", AssetRequestMode.ImmediateLoad).Value;
            public DynamicSpriteFont Font => FontAssets.MouseText.Value;
            public SelectBox()
            {
                // Initialize options
                var enumValues = (TEnum[])typeof(TEnum).GetEnumValues();
                _options = new Option[enumValues.Length];

                // Populate options and obtain minimum width and height of the select box
                for (int i = 0; i < enumValues.Length; i++)
                {
                    _options[i] = new Option(enumValues[i]);
                    Vector2 size = Font.MeasureString(_options[i].Text) * 0.85f;
                    if (TextMinWidth < size.X)
                    {
                        Width.Set(size.X + 18f + TextureIcon.Width, 0);
                        TextMinWidth = (int)size.X;
                    }
                    if (TextMinHeight < size.Y)
                    {
                        TextMinHeight = (int)size.Y;
                    }
                    // Set size of the options
                    _options[i].Left.Pixels = 0;
                    _options[i].Top.Percent = (float)i / enumValues.Length;
                    _options[i].Width.Percent = 1f;
                    _options[i].Height.Percent = 1f / enumValues.Length;
                }
            }
            public void ExpandOptions()
            {
                Height.Pixels = NumberOfOptions * Height.Pixels;
                foreach (var element in _options)
                {
                    Append(element);
                }
                IsExpanded = true;
            }
            public void CollapseOptions()
            {
                RemoveAllChildren();
                Height.Pixels = Height.Pixels / NumberOfOptions;
                IsExpanded = false;
            }
            public override void LeftClick(UIMouseEvent evt)
            {
                if (!IsExpanded)
                {
                    ExpandOptions();
                }
            }
            public override void Draw(SpriteBatch spriteBatch)
            {
                if (!IsExpanded)
                {
                    var dimensions = GetDimensions();

                    // Draw left edge
                    spriteBatch.Draw(TextureButton, new Vector2(dimensions.X, dimensions.Y), new Rectangle(0, 0, 4, TextureButton.Height), Color.White);
                    // Draw center
                    spriteBatch.Draw(TextureButton, new Rectangle((int)dimensions.X + 4, (int)dimensions.Y, (int)dimensions.Width - 6, (int)dimensions.Height), new Rectangle(4, 0, TextureButton.Width - 8, TextureButton.Height), Color.White);

                    // Draw the special icon
                    spriteBatch.Draw(TextureIcon, new Vector2(dimensions.X + dimensions.Width - 4f - TextureIcon.Width, dimensions.Y + 0.5f * (dimensions.Height - TextureIcon.Height)), Color.White);

                    // Draw right edge
                    spriteBatch.Draw(TextureButton, new Vector2(dimensions.X + dimensions.Width - 4f, dimensions.Y), new Rectangle(TextureButton.Width - 4, 0, 4, TextureButton.Height), Color.White);
                    // Draw text, centered vertically

                    Utils.DrawBorderString(spriteBatch, _selectedOption.Text, new Vector2(dimensions.X + 6f, dimensions.Y + 6f), Color.White, scale: 0.85f);
                }
                else
                {
                    DrawChildren(spriteBatch);
                }
            }
            class Option : UIElement
            {
                public TEnum Value { get; set; }
                public Option(TEnum value)
                {
                    Value = value;
                }
                public string Text { get => Language.GetText($"Mods.deeprockitems.Configs.{typeof(TEnum).Name}.{typeof(TEnum).GetEnumName(Value)}.Label").ToString(); }
                public override void LeftClick(UIMouseEvent evt)
                {
                    var parent = Parent as SelectBox;
                    (parent.Parent as SelectBoxParent<TEnum>).SetValue(Value);
                    parent.CollapseOptions();
                }
                public override void Draw(SpriteBatch spriteBatch)
                {
                    var dimensions = GetDimensions().ToRectangle();

                    // We have 3 drawing cases
                    var parent = Parent as SelectBox;
                    var options = parent._options;

                    Rectangle topEdgeSource;
                    Rectangle bottomEdgeSource;
                    Rectangle centerDestination;

                    const int border = 4; // the size, in pixels, of the border of the texture
                    if (options[0] == this) // Case 1: Top element. Rounded top, flat bottom.
                    {
                        // Draw top 2 corners
                        // Top left
                        spriteBatch.Draw(parent.TextureButton, new Vector2(dimensions.X, dimensions.Y), new Rectangle(0, 0, border, border), Color.White);
                        // Top right
                        spriteBatch.Draw(parent.TextureButton, new Vector2(dimensions.X + dimensions.Width - border, dimensions.Y), new Rectangle(parent.TextureButton.Width - border, 0, border, border), Color.White);


                        // Draw top edge minus corners
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(dimensions.X + border, dimensions.Y, dimensions.Width - 2 * border, border), new Rectangle(border, 0, parent.TextureButton.Width - 2 * border, border), Color.White);
                        // Draw bottom edge
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height - border, dimensions.Width, border), new Rectangle(border, parent.TextureButton.Height - border, parent.TextureButton.Width - 2 * border, border), Color.White);

                        // Draw left edge
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(dimensions.X, dimensions.Y + border, border, dimensions.Height - (int)(1.5f * border)), new Rectangle(0, border, border, parent.TextureButton.Height - 2 * border), Color.White);
                        // Draw right edge
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(dimensions.X + dimensions.Width - border, dimensions.Y + border, border, dimensions.Height - (int)(1.5f * border)), new Rectangle(parent.TextureButton.Width - border, border, border, parent.TextureButton.Height - 2 * border), Color.White);

                        // Draw center
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(dimensions.X + border, dimensions.Y + border, dimensions.Width - 2 * border, dimensions.Height - 2 * border), new Rectangle(border, border, parent.TextureButton.Width - 2 * border, parent.TextureButton.Height - 2 * border), Color.White);

                        // Draw the special icon
                        Vector2 origin = new(0.5f * parent.TextureIcon.Width, 0.5f * parent.TextureIcon.Height);
                        spriteBatch.Draw(parent.TextureIcon, new Vector2(dimensions.X + dimensions.Width - 4f - parent.TextureIcon.Width + origin.X, dimensions.Y + 0.5f * (dimensions.Height - parent.TextureIcon.Height) + origin.Y), null, Color.White, MathHelper.PiOver2, origin, 1f, SpriteEffects.None, 0f);
                    }
                    else if (options[^1] == this) // Case 2: Bottom element. Flat top, rounded bottom
                    {
                        // Draw bottom 2 corners
                        // Bottom left
                        spriteBatch.Draw(parent.TextureButton, new Vector2(dimensions.X, dimensions.Y + dimensions.Height - border), new Rectangle(0, parent.TextureButton.Height - border, border, border), Color.White);
                        // Bottom right
                        spriteBatch.Draw(parent.TextureButton, new Vector2(dimensions.X + dimensions.Width - border, dimensions.Y + dimensions.Height - border), new Rectangle(parent.TextureButton.Width - border, parent.TextureButton.Height - border, border, border), Color.White);


                        // Draw top edge
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, border), new Rectangle(border, 0, parent.TextureButton.Width - 2 * border, border), Color.White);
                        // Draw bottom edge minus corners
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(dimensions.X + border, dimensions.Y + dimensions.Height - border, dimensions.Width - 2 * border, border), new Rectangle(border, parent.TextureButton.Height - border, parent.TextureButton.Width - 8, 4), Color.White);

                        // Draw left edge
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(dimensions.X, dimensions.Y + (int)(0.5f * border), border, dimensions.Height - (int)(1.5f * border)), new Rectangle(0, border, border, parent.TextureButton.Height - 2 * border), Color.White);
                        // Draw right edge
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(dimensions.X + dimensions.Width - border, dimensions.Y + (int)(0.5f * border), border, dimensions.Height - (int)(1.5f * border)), new Rectangle(parent.TextureButton.Width - border, border, border, parent.TextureButton.Height - 2 * border), Color.White);

                        // Draw center
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(dimensions.X + border, dimensions.Y + border, dimensions.Width - 2 * border, dimensions.Height - 2 * border), new Rectangle(border, border, parent.TextureButton.Width - 2 * border, parent.TextureButton.Height - 2 * border), Color.White);
                    }
                    else // Case 3: Middle element. Flat top, flat bottom.
                    {
                        // Draw top edge
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(), new Rectangle(), Color.White);

                        // Draw bottom edge
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(), new Rectangle(), Color.White);

                        // Draw center
                        spriteBatch.Draw(parent.TextureButton, new Rectangle(), new Rectangle(), Color.White);

                        topEdgeSource = new(4, 0, parent.TextureButton.Width - 8, 4);
                        centerDestination = new((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height);
                        bottomEdgeSource = new(4, parent.TextureButton.Height - 4, parent.TextureButton.Width - 8, 4);
                    }
                    // Draw text, centered vertically
                    Utils.DrawBorderString(spriteBatch, Text, new Vector2(dimensions.X + 6f, dimensions.Y + 6f), Color.White, scale: 0.85f);
                }
            }
        }
    }
}