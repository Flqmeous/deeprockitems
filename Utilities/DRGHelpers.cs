﻿using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace deeprockitems.Utilities
{
    public static class DRGHelpers
    {
        /// <summary>
        /// Draws a panel with the provided texture in 9 parts. Drawing involves stretching the edges and center to fit the dimensions.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        /// <param name="insetWidth"></param>
        /// <param name="insetHeight"></param>
        /// <param name=""></param>
        public static void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, int insetWidth, int insetHeight, Vector2 position, float width, float height, Color color) {
            // Draw each edge
            Rectangle dimensions = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
            // Left
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + insetHeight, insetWidth, dimensions.Height - 2 * insetHeight), new Rectangle(0, insetHeight, insetWidth, texture.Height - 2 * insetHeight), color);
            // Right
            spriteBatch.Draw(texture, new Rectangle(dimensions.X + dimensions.Width - insetWidth, dimensions.Y + insetHeight, insetWidth, dimensions.Height - 2 * insetHeight), new Rectangle(texture.Width - insetWidth, insetHeight, insetWidth, texture.Height - 2 * insetHeight), color);
            // Top
            spriteBatch.Draw(texture, new Rectangle(dimensions.X + insetWidth, dimensions.Y, dimensions.Width - 2 * insetWidth, insetHeight), new Rectangle(insetWidth, 0, texture.Width - 2 * insetWidth, insetHeight), color);
            // Bottom
            spriteBatch.Draw(texture, new Rectangle(dimensions.X + insetWidth, dimensions.Y + dimensions.Height - insetHeight, dimensions.Width - 2 * insetWidth, insetHeight), new Rectangle(insetWidth, texture.Height - insetHeight, texture.Width - 2 * insetWidth, insetHeight), color);

            // Draw corners, top left
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, insetWidth, insetHeight), new Rectangle(0, 0, insetWidth, insetHeight), color);
            // top right
            spriteBatch.Draw(texture, new Rectangle(dimensions.X + dimensions.Width - insetWidth, dimensions.Y, insetWidth, insetHeight), new Rectangle(texture.Width - insetWidth, 0, insetWidth, insetHeight), color);
            // bottom left
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height - insetHeight, insetWidth, insetHeight), new Rectangle(0, texture.Height - insetHeight, insetWidth, insetHeight), color);
            // bottom right
            spriteBatch.Draw(texture, new Rectangle(dimensions.X + dimensions.Width - insetWidth, dimensions.Y + dimensions.Height - insetHeight, insetWidth, insetHeight), new Rectangle(texture.Width - insetWidth, texture.Height - insetHeight, insetWidth, insetHeight), color);

            // Draw center
            spriteBatch.Draw(texture, new Rectangle(dimensions.X + insetWidth, dimensions.Y + insetHeight, dimensions.Width - 2 * insetWidth, dimensions.Height - 2 * insetHeight), new Rectangle(insetWidth, insetHeight, texture.Width - 2 * insetWidth, texture.Height - 2 * insetHeight), color);
        }
        public static Color GetTeamColor(int team)
        {
            Color color = team switch
            {
                1 => new(.9f, .3f, .3f, .85f), // Red
                2 => new(.3f, .9f, .3f, .85f), // Green
                3 => new(.4f, .8f, .8f, .8f), // Blue
                4 => new(.8f, .8f, .25f, .85f), // Yellow
                5 => new(.8f, .25f, .75f, .85f), // Pink
                _ => new(.95f, .95f, .95f, .8f) // No team
            };
            return color;
        }
        /// <summary>
        /// Makes the player shake their weapon a small amount. Returns the position that the item will move to.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static Vector2 ShakeWeapon(this Player player, float multiplier = 1f)
        {
            float targetX = player.itemLocation.X + multiplier * Main.rand.NextFloat(-2.5f, 2.5f);
            float targetY = player.itemLocation.Y + multiplier * Main.rand.NextFloat(-2.5f, 2.5f);

            if (multiplier == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(multiplier), $"multiplier cannot be zero.");
            }

            return new Vector2(targetX, targetY);
        }
        /// <summary>
        /// A genericized version of <see cref="ShakeWeapon"/>. Allows any position to be used for shaking.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="multiplier"></param>
        public static Vector2 ShakePosition(this Vector2 origin, float multiplier = 1f)
        {
            float targetX = origin.X + multiplier * Main.rand.NextFloat(-2.5f, 2.5f);
            float targetY = origin.Y + multiplier * Main.rand.NextFloat(-2.5f, 2.5f);

            if (multiplier == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(multiplier), $"Multiplier cannot be zero.");
            }

            return new(targetX, targetY);
        }
        /// <summary>
        /// Returns the namespace of a given type.
        /// </summary>
        /// <param name="t">The type to retrieve the namespace of</param>
        /// <returns></returns>
        public static string GetNamespace(this object self)
        {
            return self.GetType().Namespace;
        }
        /// <summary>
        /// Returns the requested asset within the namespace of the object. Invoke like <code>this.RequestFromNamespace</code>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Asset<T> RequestFromNamespace<T>(this object self, string name) where T : class
        {
            // Swap periods with slashes
            var newString = self.GetNamespace().Replace('.', '/');
            return ModContent.Request<T>(newString + "/" + name, AssetRequestMode.ImmediateLoad);
        }
    }
}