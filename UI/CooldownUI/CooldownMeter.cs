using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Utilities;
using Terraria.ModLoader;

namespace deeprockitems.UI.CooldownUI
{
    public class CooldownMeter : UIElement, ILoadable
    {
        #region Initialize/dispose of basicEffect
        // BasicEffect is a shader that is required to draw primitives
        static BasicEffect basicEffect;
        void ILoadable.Load(Mod mod) {
            // Initialize the basic effect
            Main.RunOnMainThread(() => {
                basicEffect = new(Main.instance.GraphicsDevice);
                basicEffect.VertexColorEnabled = true;
                basicEffect.TextureEnabled = true;
            });
        }
        void ILoadable.Unload() {
            // Dispose of the basicEffect
            basicEffect?.Dispose();
            basicEffect = null;
        }
        #endregion
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
        public float Angle = 0f;
        public override void Draw(SpriteBatch spriteBatch) {
            // Testing
            //DrawFancyMeter(spriteBatch);
            //return;
            
            // Only draw if the player weapon is not null, and the cooldown is greater than 0
            if (PlayerWeapon is null) return;
            if (PlayerWeapon.Cooldown <= 0) return;
            // We only have the fancy meter right now
            DrawFancyMeter(spriteBatch);
        }
        private void DrawFancyMeter(SpriteBatch spriteBatch) {
            // The percentage of the player cooldown to the weapon's max cooldown. 
            float percentFilledMeter = PlayerWeapon.Cooldown / UpgradableWeapon.COOLDOWN_THRESHOLD;
            // Get fancy meter dimensions
            Rectangle dimensions = GetDimensions().ToRectangle();
            dimensions.Y += 2 * Assets.UI.CooldownUIBackground.Height();
            // Important angles used for the drawing of the meter
            const float initialAngle = 1 * MathHelper.Pi / 4; // The beginning angle of the meter
            const float endAngle = 3 * MathHelper.Pi / 4; // The end angle of the meter
            // The color of the meter will be white for now.
            Color filledMeterColor = Color.White;

            // Since i will be drawing the sectors with primitives instead, we don't need to do any funny business. Let's just get the parameters through
            // Draw the background
            DrawSectorPrimitive(Main.instance.GraphicsDevice, dimensions.Center(), Assets.UI.CooldownUIBackground.Width(), initialAngle, 3 * MathHelper.Pi / 2f, 1, Color.DarkGray);
            // Draw the filled meter to show the cooldown
            //DrawSectorPrimitive(Main.instance.GraphicsDevice, dimensions.Center(), Assets.UI.CooldownUIBackground.Width(), initialAngle, percentFilledMeter * 3 * MathHelper.Pi / 2f, 1, Color.CornflowerBlue);
            // Draw a shape
            Vector3[] vertices = [
                new Vector3(960f, 300f, 0f),
                new Vector3(980f, 200f, 0f),
                new Vector3(1000f, 300f, 0f),
                new Vector3(1000f, 340f, 0f),
                new Vector3(960f, 340f, 0f)
            ];
            // Draw
            DrawPolygonViaPrimitive(Main.instance.GraphicsDevice, Color.Red, vertices);


           /* // A new spritebatch will have to begin with the adjusted parameters. Specifically, the matrix needs to be scaled to 2x.
            spriteBatch.End();
            // This is the matrix with scale of 2x.
            const float matrixScale = 2f;
            Matrix transform = Main.UIScaleMatrix * Matrix.CreateScale(matrixScale);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, transform);
            // Create scaled dimensions for use with this matrix
            Rectangle scaledDimensions = new Rectangle((int)(dimensions.X / matrixScale), (int)(dimensions.Y / matrixScale), (int)(dimensions.Height / matrixScale), (int)(dimensions.Width / matrixScale));
            // When drawing a sector in a circle, we will require the following params:
            SpriteBatch spriteBatch -The spritebatch
            * Texture2D texture - The texture that will make up the circle
            * Rectangle dimensions - The dimensinsions that this sector will be drawn to.Using the scaled dimensions for this.

            * float beginAngle - The starting angle that we will draw from
            * int direction - 0 or 1.

            * float angularDiameter - This will be the angle of the circle that we draw
            * int precision - How many "steps" we will use to draw the sector.Using a high enough number ensures that the drawing doesn't look choppy.

            // Draw the dark background of the meter.
           DrawSector(spriteBatch, Assets.WhitePixel.Value, scaledDimensions.Center(), scaledDimensions.Width / 2f, initialAngle, 3 * MathHelper.Pi / 2f, 1, 20, Color.DarkGray);
           // Draw the lighter, filled in meter.
           DrawSector(spriteBatch, Assets.WhitePixel.Value, scaledDimensions.Center(), scaledDimensions.Width / 2f, initialAngle, percentFilledMeter * 3 * MathHelper.Pi / 2f, 1, 20, Color.CornflowerBlue);
           // Draw the meter texture.
           spriteBatch.Draw(Assets.UI.CooldownUIBackground.Value, scaledDimensions, Color.White);
            // End and begin the corrected spritebatch
            spriteBatch.End();
            spriteBatch.BeginWithDefaultsForUI();*/


            /*// Get dimensions
            Rectangle dimensions = GetDimensions().ToRectangle();
            dimensions.Y += 2 * Assets.UI.CooldownUIBackground.Height();
            // Get percentage (this is what we actually draw with!
            float lerpPercentage = PlayerWeapon.Cooldown / UpgradableWeapon.COOLDOWN_THRESHOLD; // Convert to percentage
            const float initialAngle = 1 * MathHelper.Pi / 4;
            const float endAngle = 3 * MathHelper.Pi / 4;
            // Inner color
            Color innerColor = Color.Lerp(Color.Green, Color.Red, lerpPercentage);
            // Begin new spritebatch at 2x scale. This is how we're simulating the 1/2 pixel size
            spriteBatch.End();
            Matrix transform = Matrix.CreateScale(2f);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, transform);

            // Draw inner inactive meter
            DrawSectorToAndFromRadians(spriteBatch, Assets.WhitePixel.Value, dimensions.Center(), Width.Pixels / 2f, initialAngle, 3 * MathHelper.Pi / 2, 1, Color.DarkGray);
            float lerpAngle = MathHelper.Lerp(initialAngle, endAngle, lerpPercentage);
            // Draw filled meter!
            DrawSectorToAndFromRadians(spriteBatch, Assets.WhitePixel.Value, dimensions.Center(), Width.Pixels / 2f, initialAngle, lerpPercentage * 3 * MathHelper.Pi / 2, 1, innerColor);
            // fix this spritebatch
            spriteBatch.End();
            spriteBatch.BeginWithDefaultsForUI();

            // Draw outer
            spriteBatch.Draw(Assets.UI.CooldownUIBackground.Value, dimensions, Color.White);*/
        }
        private void DrawSectorPrimitive(GraphicsDevice graphics, Vector2 center, float radius, float beginAngle, float sectorAngle, int direction, Color color) {
            // Testing drawing
            // Set transform matrix
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0f, graphics.Viewport.Width, graphics.Viewport.Height, 0f, -1f, 10f);

            // Apply vertices. Create a square with RGBW centered on the UI element
            VertexPositionColorTexture[] vertices = [
                new VertexPositionColorTexture(new Vector3(center.X - radius, center.Y - radius, 0f), Color.Red, Vector2.Zero), // top left
                new VertexPositionColorTexture(new Vector3(center.X + radius, center.Y - radius, 0f), Color.Green, Vector2.Zero), // top right
                new VertexPositionColorTexture(new Vector3(center.X + radius, center.Y + radius, 0f), Color.Blue, Vector2.Zero), // bottom right
                new VertexPositionColorTexture(new Vector3(center.X - radius, center.Y + radius, 0f), Color.White, Vector2.Zero) // bottom left
            ];
            // Set texture coordinates
            vertices[0].TextureCoordinate = new(0, 0);
            vertices[1].TextureCoordinate = new(1, 0);
            vertices[2].TextureCoordinate = new(1, 1);
            vertices[3].TextureCoordinate = new(0, 1);
            // create indices
            short[] indices = new short[vertices.Length + 1];
            for (short i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }
            // This closes the shape
            indices[4] = 0;
            // Set textures
            graphics.Textures[0] = Assets.WhitePixel.Value;
            // Draw each vertex
            foreach (var pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                // Draw
                graphics.DrawUserIndexedPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length, indices, 0, indices.Length);
            }
        }
        private void DrawPolygonViaPrimitive(GraphicsDevice graphics, Color color, params Vector3[] vertices) {
            // Create the projection matrix
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0f, graphics.Viewport.Width, graphics.Viewport.Height, 0f, -1f, 10f);
            // Create list of vertices
            VertexPositionColorTexture[] verticesWithPCT = new VertexPositionColorTexture[vertices.Length];
            short[] indices = new short[vertices.Length + 1];
            for (short i = 0; i < vertices.Length; i++)
            {
                // Create vertex
                verticesWithPCT[i] = new VertexPositionColorTexture(vertices[i], color, Vector2.Zero);
                indices[i] = i;
            }
            // Seal the indices
            indices[^1] = 0;
            // Draw
            graphics.Textures[0] = Assets.WhitePixel.Value;
            // Draw each vertex
            foreach (var pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                // Draw
                graphics.DrawUserIndexedPrimitives(PrimitiveType.LineStrip, verticesWithPCT, 0, verticesWithPCT.Length, indices, 0, indices.Length);
            }
        }
        private void DrawSector(SpriteBatch spriteBatch, Texture2D texture, Vector2 center, float radius, float beginAngle, float sectorAngle, int direction, int precision, Color color) {
            // Precision will never be allowed to be less than 3
            if (precision < 3)
            {
                precision = 3;
            }
            // Fix the beginning angle
            beginAngle += 2 * MathHelper.Pi;
            // Create the calculated end angle
            float endAngle = beginAngle - sectorAngle;
            // Create the step value based on precision
            float angleStep = sectorAngle / precision;
            // Begin for loop
            for (float iteratorAngle = beginAngle; iteratorAngle >= endAngle; iteratorAngle -= angleStep)
            {
                // Draw destination, origin, and maybe the angle are all going to be a bitch to calclulate
                float arcLength = radius * (beginAngle - endAngle);
                int width = (int)radius;
                int height = (int)(arcLength / precision);
                Rectangle drawDestination = new Rectangle((int)center.X, (int)center.Y, width, height);
                Vector2 origin = new Vector2(drawDestination.Width, drawDestination.Height);
                spriteBatch.Draw(texture, drawDestination, null, color, iteratorAngle, origin, SpriteEffects.None, 0f);
            }
        }
        private void DrawSectorToAndFromRadians(SpriteBatch spriteBatch, Texture2D texture, Vector2 centerPos, float radius, float begin, float sizeOfSector, int direction, Color color) {
            /*float adjustedBeginAngle = begin + 2 * MathHelper.Pi;
            float adjustedEndAngle = adjustedBeginAngle - sizeOfSector;
            for (float rad = adjustedBeginAngle; rad >= adjustedEndAngle; rad -= 0.05f)
            {
                spriteBatch.Draw(texture, new Rectangle((int)(centerPos.X), (int)(centerPos.Y - texture.Height * 0.25f), (int)(radius * 0.5f), texture.Height), null, color, rad, new Vector2(texture.Width, texture.Height / 2f), SpriteEffects.None, 0f);
            }*/

        }
    }
}
