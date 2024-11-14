using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using MonoMod.Core.Platforms;

namespace deeprockitems.Content.Tiles
{
    public class SludgeGlobalTile : GlobalTile
    {
        static int tileWidth = 18;
        /*static KeyValuePair<int, Point> TopLeftOuterCorner => new(6, new(0, 0));
        static KeyValuePair<int, Point> TopEdge => new(14, new(18, 0));
        static KeyValuePair<int, Point> TopRightOuterCorner => new(12, new(36, 0));
        static KeyValuePair<int, Point> LeftEdge => new(7, new(0, 18));
        static KeyValuePair<int, Point> BottomLeftOuterCorner => new(3, new(0, 36));
        static KeyValuePair<int, Point> RightEdge => new(13, new(36, 18));
        static KeyValuePair<int, Point> BottomRightOuterCorner => new(9, new(36, 36));
        static KeyValuePair<int, Point> BottomEdge => new(11, new(18, 36));
        static KeyValuePair<int, Point> AllEdge => new(0, new(18, 18));
        static KeyValuePair<int, Point> TopLeftInnerCorner => new(new(0, 54);
        static KeyValuePair<int, Point> InverseTopEdge => new(18, 54);
        static KeyValuePair<int, Point> TopRightInnerCorner => new(36, 54);
        static KeyValuePair<int, Point> InverseLeftEdge => new(0, 72);
        static KeyValuePair<int, Point> BottomLeftInnerCorner => new(0, 90);
        static KeyValuePair<int, Point> InverseRightEdge => new(36, 72);
        static KeyValuePair<int, Point> BottomRightInnerCorner => new(36, 90);
        static KeyValuePair<int, Point> InverseBottomEdge => new(18, 90);*/
        //static KeyValuePair<int, Point> UnusedCenter => new(18, 72);
        static bool HasSludge(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileFrameImportant[tile.TileType];
        }
        private static int GetXFrame(SludgeDirections direction) {
            switch (direction)
            {
                case SludgeDirections.Negative:
                    return 54;

                case SludgeDirections.Positive:
                    return 18;

                case SludgeDirections.Negative | SludgeDirections.Positive:
                    return 36;

                default:
                    return 0;
            }
        }
        private static void DrawSludgeFromFrame(int i, int j, float rotation, SludgeDirections direction) {
            // Create offset
            Vector2 offset = Main.screenPosition - (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange));
            Main.EntitySpriteDraw(new DrawData(Assets.WhitePixel.Value, new Rectangle(i * 16 - (int)offset.X, j * 16 - (int)offset.Y, 16, 16), Color.White));
            /*Vector2 tilePos = new((i + 12) * 16 + 8, (j + 12) * 16 + 8);
            Vector2 drawOffset = 12 * new Vector2((float)Math.Cos(rotation + MathHelper.PiOver2), (float)Math.Sin(rotation + MathHelper.PiOver2));
            Rectangle sourceRect = new(GetXFrame(direction), 0, 16, 16);
            Main.EntitySpriteDraw(new DrawData(Assets.Tiles.Sludge.Value, tilePos - Main.screenPosition - drawOffset, sourceRect, Color.White, rotation, new Vector2(8, 8), 1f, SpriteEffects.None));
        */}
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch) {
            base.PostDraw(i, j, type, spriteBatch);
            SludgeTileSystem system = ModContent.GetInstance<SludgeTileSystem>();
            if (system.IsTileSludged(i, j, out SludgeTile? tile))
            {
                SludgeDirections direction = SludgeDirections.None;
                // Brute force method of drawing: draw top, right, bottom, left
                // Draw top face
                if (tile.CoveredSurfaces.HasFlag(SludgeSurfaces.Top))
                {
                    // Check for connected textures through the left and right faces
                    if (system.IsTileSludged(i - 1, j)) {
                        direction |= SludgeDirections.Negative;
                    }
                    if (system.IsTileSludged(i + 1, j))
                    {
                        direction |= SludgeDirections.Positive;
                    }
                    // Send to drawing
                    DrawSludgeFromFrame(i, j, 0f, direction);
                }
                // Draw right face
                if (tile.CoveredSurfaces.HasFlag(SludgeSurfaces.Right))
                {
                    // Check for connected textures through the left and right faces
                    if (system.IsTileSludged(i, j - 1))
                    {
                        direction |= SludgeDirections.Negative;
                    }
                    if (system.IsTileSludged(i, j + 1))
                    {
                        direction |= SludgeDirections.Positive;
                    }
                    // Send to drawing
                    DrawSludgeFromFrame(i, j, MathHelper.PiOver2, direction);
                }
                // Draw bottom face
                if (tile.CoveredSurfaces.HasFlag(SludgeSurfaces.Bottom))
                {
                    // Check for connected textures through the left and right faces
                    if (system.IsTileSludged(i - 1, j))
                    {
                        direction |= SludgeDirections.Negative;
                    }
                    if (system.IsTileSludged(i + 1, j))
                    {
                        direction |= SludgeDirections.Positive;
                    }
                    // Send to drawing
                    DrawSludgeFromFrame(i, j, MathHelper.Pi, direction);
                }
                // Draw left face
                if (tile.CoveredSurfaces.HasFlag(SludgeSurfaces.Left))
                {
                    // Check for connected textures through the left and right faces
                    if (system.IsTileSludged(i, j - 1))
                    {
                        direction |= SludgeDirections.Negative;
                    }
                    if (system.IsTileSludged(i, j + 1))
                    {
                        direction |= SludgeDirections.Positive;
                    }
                    // Send to drawing
                    DrawSludgeFromFrame(i, j, 3 * MathHelper.PiOver2, direction);
                }
            }
        }
    }
    [Flags]
    public enum SludgeSurfaces {
        None = 0,
        Top = 1,
        Right = 2,
        Bottom = 4,
        Left = 8
    }
    [Flags]
    public enum SludgeDirections {
        None = 0,
        Negative = 1,
        Positive = 2
    }
    public class SludgeTileSystem : ModSystem {
        public List<SludgeTile> SludgedTiles { get; set; } = new();
        public void AddNewSludgeTile(int x, int y, int time, SludgeSurfaces surfaces) {
            if (IsTileSludged(x, y, out SludgeTile tile))
            {
                if (tile.Timer < time)
                {
                    tile.Timer = time;
                }
                return;
            }
            SludgedTiles.Add(new SludgeTile() { X = x, Y = y, Timer = time, CoveredSurfaces = surfaces});
        }
        public bool IsTileSludged(int x, int y) {
            return SludgedTiles.Where(t => t.X == x && t.Y == y).Any();
        }
        /// <summary>
        /// Finds if a tile is sludged via coordinates. Outs the instance of the sludged tile, if found.
        /// </summary>
        public bool IsTileSludged(int x, int y, out SludgeTile? tile) {
            var query = SludgedTiles.Where(t => t.X == x && t.Y == y);
            if (query.Any())
            {
                tile = query.First();
                return true;
            }
            tile = null;
            return false;
        }
        public override void ClearWorld() {
            SludgedTiles = new();
        }
        public override void PostUpdateWorld() {
            // Make non-blocks never be sludged
            for (int i = SludgedTiles.Count - 1; i >= 0; i--)
            {
                SludgeTile tile = SludgedTiles[i];
                // Tick down timer and maybe delete the sludged tile
                if (tile.Timer-- <= 0)
                {
                    SludgedTiles.RemoveAt(i);
                    continue;
                }
                if (Main.tile[tile.X, tile.Y].HasTile && Main.tileSolid[Main.tile[tile.X, tile.Y].TileType] && !Main.tileFrameImportant[Main.tile[tile.X, tile.Y].TileType]) continue;
                SludgedTiles.RemoveAt(i);
                continue;
            }
            /*for (int i = 0; i < Main.tile.Width; i++)
            {
                for (int j = 0; j < Main.tile.Height; j++)
                {
                    if (SludgeTimer[i, j] > 0)
                    {
                        SludgeTimer[i, j]--;
                    }
                    if (SludgeTimer[i, j] <= 0 || Main.tile[i, j].HasTile && !Main.tileSolid[Main.tile[i, j].TileType] && Main.tileFrameImportant[Main.tile[i, j].TileType]) continue;
                    SludgedTiles[i, j] = false;
                    SludgeTimer[i, j] = 0;
                }
            }*/
        }
    }
    public class SludgeTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Timer { get; set; }
        public SludgeSurfaces CoveredSurfaces { get; set; }
    }
}
