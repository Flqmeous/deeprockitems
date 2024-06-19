using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using System;

namespace deeprockitems.Common.PlayerLayers
{
    public class ZhukovsFrontLayer : PlayerDrawLayer
    {
        public static Texture2D ZhukovsTexture { get; set; }
        public override Position GetDefaultPosition() => new Between(PlayerDrawLayers.SolarShield, PlayerDrawLayers.ArmOverItem);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (ZhukovsTexture.Height == 1)
            {
                // Try and get the zhukovs texture
                try
                {
                    ZhukovsTexture = (Texture2D)ModContent.Request<Texture2D>("deeprockitems/Content/Items/Weapons/ZhukovsHeld");
                }
                catch
                {
                    Mod drg = ModContent.GetInstance<deeprockitems>();
                    drg.Logger.Warn("Unable to retrieve Zhukovs' held item texture. Zhukovs will draw like a regular item.");
                    return;
                }
            }
            if (!drawInfo.drawPlayer.ItemAnimationActive) return;
            if (drawInfo.drawPlayer.JustDroppedAnItem) return;
            if (drawInfo.shadow != 0f) return;
            if (ZhukovsTexture is null) return;

            // Set Draw Info defaults
            drawInfo.heldItem = drawInfo.drawPlayer.HeldItem; // Make sure that the held item is updated
            // Gets the lighting of the player's center.
            drawInfo.itemColor = Lighting.GetColor((int)(drawInfo.Position.X + drawInfo.drawPlayer.width * 0.5) / 16, (int)((drawInfo.Position.Y + drawInfo.drawPlayer.height * 0.5) / 16.0));
            float adjustedItemScale = drawInfo.drawPlayer.GetAdjustedItemScale(drawInfo.heldItem); // As it says on the tin

            // Draw.
            if (drawInfo.heldItem.type == ModContent.ItemType<Zhukovs>())
            {
                // This is where the item will be drawed from
                Vector2 itemPosition = drawInfo.drawPlayer.itemLocation - Main.screenPosition;
                // This is the frame of the zhukov's sprite
                Rectangle frame = new Rectangle(0, 0, ZhukovsTexture.Width, ZhukovsTexture.Height);
                // Is just item rotation
                float rotation = drawInfo.drawPlayer.itemRotation;
                // X offset used for the player's direction
                float xOffset = drawInfo.drawPlayer.direction == 1 ? -8f : 48f;
                // Vertical offset used for the player's gravity
                float yOffset = frame.Height * 3 / 5f;
                // Rotation origin, this must be shifted if the player's direction changes.
                Vector2 drawOrigin = new Vector2((int)xOffset, (int)(frame.Height * 4/5));

                DrawData zhukovsFrontLayer = new DrawData(ZhukovsTexture, new Vector2((int)itemPosition.X, (int)itemPosition.Y + yOffset), frame, drawInfo.itemColor, rotation, drawOrigin, adjustedItemScale, drawInfo.itemEffect);
                drawInfo.DrawDataCache.Add(zhukovsFrontLayer);

                /*Vector2 hitBoxCenter = new Vector2((int)(drawInfo.heldItem.width / 2f), (int)(drawInfo.heldItem.height / 2f));
                Vector2 drawOffset = new(2f, (int)(zhukovsSprite.Height / 2f) + (int)(drawInfo.drawPlayer.gravDir * -4f));

                int drawOffX = (int)drawOffset.X;
                hitBoxCenter.Y = drawOffset.Y;
                Vector2 drawOrigin = new Vector2((float)-(float)drawOffX, zhukovsSprite.Height / 2f);
                if (drawInfo.drawPlayer.direction == -1)
                {
                    drawOrigin = new Vector2(zhukovsSprite.Width + drawOffX, zhukovsSprite.Height / 2f);
                }

                // Mainhand in the front.
                item = new DrawData(zhukovsSprite, new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X + hitBoxCenter.X), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y + hitBoxCenter.Y)), zhukovsSprite.Bounds, drawInfo.heldItem.GetAlpha(drawInfo.itemColor), drawInfo.drawPlayer.itemRotation, drawOrigin, adjustedItemScale, drawInfo.itemEffect, 0f);
                drawInfo.DrawDataCache.Add(item);
                return;*/
            }
        }
    }
}
