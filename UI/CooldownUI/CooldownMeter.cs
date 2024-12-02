using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Utilities;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.UI;
using deeprockitems.Content.Buffs;
using Terraria.DataStructures;
using Terraria.GameContent;

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
            Main.RunOnMainThread(() => {
                basicEffect?.Dispose();
                basicEffect = null;
            });
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
            if (PlayerWeapon is null || PlayerWeapon.OverheatCooldown <= 0) {
                return;
            }
            // We only have the simple meter right now
            Rectangle dimensions = GetDimensions().ToRectangle();
            DrawSimpleMeter(spriteBatch, new Vector2(dimensions.Center.X, dimensions.Center.Y + 60f), PlayerWeapon.OverheatCooldown / UpgradableWeapon.COOLDOWN_THRESHOLD, CooldownDrawColor);

        }
        public Color CooldownDrawColor => PlayerWeapon.IsWeaponEnabledByCooldown ? Color.White : Color.IndianRed;
        private void DrawSimpleMeter(SpriteBatch spriteBatch, Vector2 center, float percentageFilled, Color drawColor) {
            // Adjust draw center
            center.Y -= 20;

            // Get real drawing position
            Vector2 adjustedDrawPos = new Vector2(center.X, center.Y) - 0.5f * TextureAssets.Hb1.Size();
            int filledMeterWidth = (int)(percentageFilled * 36f);
            if (filledMeterWidth < 3)
            {
                filledMeterWidth = 3;
            }

            // If the meter is not full
            if (filledMeterWidth < 34)
            {
                // Draw background of meter
                if (filledMeterWidth < 36)
                {
                    spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(adjustedDrawPos.X + filledMeterWidth, adjustedDrawPos.Y), (Rectangle?)new Rectangle(2, 0, 2, TextureAssets.Hb2.Height()), drawColor);
                }
                // Draw outline of right edge of meter
                if (filledMeterWidth < 34)
                {
                    spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(adjustedDrawPos.X + filledMeterWidth + 2, adjustedDrawPos.Y), (Rectangle?)new Rectangle(filledMeterWidth + 2, 0, 36 - filledMeterWidth - 2, TextureAssets.Hb2.Height()), drawColor);
                }
                // Draw left edge of meter
                if (filledMeterWidth > 2)
                {
                    spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(adjustedDrawPos.X, adjustedDrawPos.Y), (Rectangle?)new Rectangle(0, 0, filledMeterWidth - 2, TextureAssets.Hb1.Height()), drawColor);
                }
                // The actual filled meter
                spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(adjustedDrawPos.X + filledMeterWidth - 2, adjustedDrawPos.Y), (Rectangle?)new Rectangle(32, 0, 2, TextureAssets.Hb1.Height()), drawColor);
            }
            else // Close to maximum temperature
            {
                // Draw meter background
                if (filledMeterWidth < 36)
                {
                    Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb2.Value, new Vector2(adjustedDrawPos.X + filledMeterWidth, adjustedDrawPos.Y), (Rectangle?)new Rectangle(filledMeterWidth, 0, 36 - filledMeterWidth, TextureAssets.Hb2.Height()), drawColor));
                }
                // Draw filled meter
                Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb1.Value, new Vector2(adjustedDrawPos.X, adjustedDrawPos.Y), (Rectangle?)new Rectangle(0, 0, filledMeterWidth, TextureAssets.Hb1.Height()), drawColor));
            }
        }
        private void DrawFancyMeter(SpriteBatch spriteBatch) {
            // The percentage of the player cooldown to the weapon's max cooldown. 
            float percentFilledMeter = PlayerWeapon.OverheatCooldown / UpgradableWeapon.COOLDOWN_THRESHOLD;
            // Get fancy meter dimensions
            Rectangle dimensions = GetDimensions().ToRectangle();
            dimensions.Y += 2 * Assets.UI.CooldownUIBackground.Height();
            // The color of the meter will be white for now.
            Color drawColor = Color.White;
            // Draw the outer layer of the fancy meter
            
        }
        private void DrawSectorPrimitive(GraphicsDevice graphics, Vector2 center, float radius, float beginAngle, float sectorAngle, int direction, Color color) {
            
        }
        private void DrawFancyCooldownMeterPrimitives(GraphicsDevice graphics, Vector2 center, float radius, float percentFilled, Color color) {
            /*// In order to draw this correctly, we're going to draw a 20-gon and remove some vertices.
            // Get the vertices involved in a 20-gon
            List<Vector3> oldVertices = [..GetVerticesOfPolygon(center, 99, radius)];
            List<Vector3> newVertices = [];
            // Programmatically remove the top vertices that fall between the beginning angle (3Pi/4) and the percentage from 3Pi/4 to 9Pi/4
            const float startAngle = 1 * MathHelper.Pi / 4;
            const float endAngle = 3 * MathHelper.Pi / 4;
            float sizeOfSector = percentFilled * 3 * MathHelper.PiOver2;
            int numberOfWraps = 0;
            float oldAngle = 0f;
            Main.NewText("begin");
            foreach (var vertex in oldVertices)
            {
                // Always add the center coordinate
                if (vertex.X == center.X && vertex.Y == center.Y)
                {
                    newVertices.Add(vertex);
                    continue;
                }
                // Normalize the vectors before we use them to calculate angles
                Vector2 normalized = new Vector2(vertex.X - center.X, vertex.Y - center.Y);
                // Calculate angle
                float angle = (float)Math.Atan2(normalized.X, -normalized.Y);

                // If the angle is within the start and end angle, don't add that vertex (aka, add the center)
                if (startAngle < angle && angle < endAngle)
                {
                    newVertices.Add(new Vector3(center.X, center.Y, 0f));
                    continue;
                }

                // Now start doing the wrap shenanigans
                if (oldAngle < -3f && angle > MathHelper.Pi - 3f)
                {
                    numberOfWraps--;
                }
                oldAngle = angle;
                // Wrapped angle measure
                float wrappedAngleMeasure = angle + numberOfWraps * MathHelper.TwoPi + MathHelper.TwoPi;
                Main.NewText(wrappedAngleMeasure);

                if (startAngle + MathHelper.TwoPi - sizeOfSector < wrappedAngleMeasure)
                {
                    newVertices.Add(new Vector3(center.X, center.Y, 0f));
                    continue;
                }

                // Add the vertex
                newVertices.Add(vertex);
            }
            // Draw the new "polygon"
            DrawPrimPolygon(graphics, color, [..newVertices]);*/
        }
        private Vector3[] GetVerticesOfPolygon(Vector2 center, int numberOfSides, float radius, float rotationOffset = 0f) {
            if (numberOfSides < 3)
            {
                throw new Exception("Polygons cannot have less than 3 sides!");
            }
            // First get the internal angle in radians
            //float internalAngle = (numberOfSides - 2) * MathHelper.Pi / numberOfSides;
            float angleToRotateBy = 2 * MathHelper.Pi / numberOfSides;
            // Calculate side length
            float sideLength = (float)Math.Sqrt(2 * radius * radius * (1 - Math.Cos(2 * MathHelper.Pi / numberOfSides)));
            Vector3 originalPoint = new Vector3(center.X, center.Y - radius, 0f);
            // Now we need to dynamically create the vertices
            // Add the center as the first index
            List<Vector3> vertexPositions = [new Vector3(center.X, center.Y, 0f)];
            // Dynamically create vertices based on rotation and iteration
            for (int i = 0; i < numberOfSides; i++)
            {
                // Rotated X coordinate
                float rotatedX = (int)(center.X + (originalPoint.X - center.X) * Math.Cos(i * angleToRotateBy + rotationOffset) - (originalPoint.Y - center.Y) * Math.Sin(i * angleToRotateBy + rotationOffset));
                // Rotated Y coordinate
                float rotatedY = (int)(center.Y + (originalPoint.X - center.X) * Math.Sin(i * angleToRotateBy + rotationOffset) - (originalPoint.Y - center.Y) * Math.Cos(i * angleToRotateBy + rotationOffset));
                // Add the vector to the vertex positions list
                vertexPositions.Add(new Vector3(rotatedX, rotatedY, 0f));
            }
            // Return the vertices
            return [.. vertexPositions];
        }
        private void DrawPrimPolygonCenteredOnPoint(GraphicsDevice graphics, Vector2 center, int numberOfSides, float radius, Color color, float rotationOffset = 0f) {
            var vertices = GetVerticesOfPolygon(center, numberOfSides, radius, rotationOffset);
            // Call primitive drawing
            DrawPrimPolygon(graphics, Color.White, [..vertices]);
        }
        private void DrawPrimPolygon(GraphicsDevice graphics, Color color, params Vector3[] vertices) {
            // Create the projection matrix
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0f, Main.screenWidth, Main.screenHeight, 0f, -1f, 10f);
            // Create list of vertices
            VertexPositionColorTexture[] verticesWithPCT = new VertexPositionColorTexture[vertices.Length];

            // We're going to manually have to create these indices. :(
            // Create the list of indices (3 * (n - 1)). n is the number of vertices
            short[] indices = new short[3 * (vertices.Length - 1)];
            for (short i = 0; i < vertices.Length; i++)
            {
                // Begin adding vertices and indices
                // Create vertex
                verticesWithPCT[i] = new VertexPositionColorTexture(vertices[i], color, Vector2.Zero);
                // if the iterator is on the last one, break
                if (i == vertices.Length - 1) break;
                // Begin adding indices in sets of 3. They always begin with 0.
                indices[3 * i] = 0;
                // Add the next one (i + 1)
                indices[3 * i + 1] = (short)(i + 1);
                // Add the final index ((i+1) % (length - 1))
                indices[3 * i + 2] = (short)((i + 1) % (vertices.Length - 1) + 1);
            }
/*            // Seal the indices
            indices[^1] = 0;*/
            // Draw
            graphics.Textures[0] = Assets.WhitePixel.Value;
            // Draw each vertex
            foreach (var pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                // Draw
                graphics.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, verticesWithPCT, 0, verticesWithPCT.Length, indices, 0, indices.Length);
            }
        }
    }
}
