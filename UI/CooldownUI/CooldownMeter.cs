using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Content.Items.Weapons;

namespace deeprockitems.UI.CooldownUI
{
    public class CooldownMeter : UIElement
    {
#nullable enable
        private static UpgradableWeapon? PlayerWeapon {
            get
            {
                try
                {
                    return Main.LocalPlayer.HeldItem.ModItem as UpgradableWeapon;
                }
                catch
                {
                    return null;
                }
            }
        }
#nullable disable
        public CooldownMeter() {
            // Set width and height
            Width.Pixels = 32f;
            Height.Pixels = 32f;
        }
        public override void Draw(SpriteBatch spriteBatch) {
            // Only draw if the player weapon is not null, and the cooldown is greater than 0
            if (PlayerWeapon is null) return;
            if (PlayerWeapon.Cooldown <= 0) return;
            // Get dimensions
            Rectangle dimensions = GetDimensions().ToRectangle();
            dimensions.Y += 2 * Assets.UI.CooldownUIBackground.Height();
            // Get percentage (this is what we actually draw with!
            float lerpPercentage = PlayerWeapon.Cooldown / UpgradableWeapon.COOLDOWN_THRESHOLD; // Convert to percentage
            const float initialAngle = 1 * MathHelper.Pi / 4;
            const float endAngle = 3 * MathHelper.Pi / 4;
            // Inner color
            Color innerColor = Color.Lerp(Color.Green, Color.Red, lerpPercentage);
            // Draw inner inactive meter
            DrawSectorToAndFromRadians(spriteBatch, Assets.WhitePixel.Value, dimensions.Center(), Width.Pixels / 2f, initialAngle, 3 * MathHelper.Pi / 2, 1, Color.DarkGray);

            float lerpAngle = MathHelper.Lerp(initialAngle, endAngle, lerpPercentage);
            // Draw filled meter!
            DrawSectorToAndFromRadians(spriteBatch, Assets.WhitePixel.Value, dimensions.Center(), Width.Pixels / 2f, initialAngle, lerpPercentage * 3 * MathHelper.Pi / 2, 1, innerColor);
            // Draw outer
            spriteBatch.Draw(Assets.UI.CooldownUIBackground.Value, dimensions, Color.White);
        }
        private void DrawSectorToAndFromRadians(SpriteBatch spriteBatch, Texture2D texture, Vector2 centerPos, float radius, float begin, float sizeOfSector, int direction, Color color) {

           /* if (direction == -1)
            {
                for (float rad = adjustedBeginAngle; rad <= adjustedEndAngle; rad += 0.1f)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)(centerPos.X), (int)(centerPos.Y - texture.Height * 0.5f), (int)radius, texture.Height), null, color, rad + 4 * MathHelper.Pi, new Vector2(texture.Width, texture.Height / 2f), SpriteEffects.None, 0f);
                }
            }
            else*/
            {
                float adjustedBeginAngle = begin + 2 * MathHelper.Pi;
                float adjustedEndAngle = adjustedBeginAngle - sizeOfSector;
                for (float rad = adjustedBeginAngle; rad >= adjustedEndAngle; rad -= 0.1f)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)(centerPos.X), (int)(centerPos.Y - texture.Height * 0.5f), (int)radius, texture.Height), null, color, rad, new Vector2(texture.Width, texture.Height / 2f), SpriteEffects.None, 0f);
                }
            }
        }
    }
}
