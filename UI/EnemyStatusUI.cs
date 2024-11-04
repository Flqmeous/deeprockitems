using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using deeprockitems.Content.Buffs;
using Terraria.DataStructures;
using System;
using Terraria.ID;
using deeprockitems.Common.Config;
using Terraria.GameContent;

namespace deeprockitems.UI
{
    public class EnemyStatusUI : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public IEnemyDrawInfo[] DrawingInfo { get; set; }
        public override void SetDefaults(NPC entity)
        {
            DrawingInfo = [
                new StunDrawInfo(entity),
                new TemperatureDrawInfo(entity),
                ];
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float xPos = npc.Center.X;
            float yPos = npc.position.Y;

            if (npc.realLife > -1 && Main.npc[npc.realLife].type == NPCID.TheDestroyer)
            {
                xPos = Main.destroyerHB.X;
                yPos = Main.destroyerHB.Y;
            }

            foreach (var info in DrawingInfo)
            {
                if (info.IsVisible)
                {
                    info.Draw(screenPos, drawColor, xPos, ref yPos);
                }
            }
        }
    }
    public class StunDrawInfo : IEnemyDrawInfo
    {
        #region Interface definitions
        public StunDrawInfo(NPC npc)
        {
            NPC = npc;
        }
        public bool IsVisible { get => NPC.GetGlobalNPC<StunnedEnemyNPC>().IsStunned; }
        public NPC NPC { get; init; }
        #endregion
        #region Fields required for drawing
        private int _stunFrameTimer = 0;
        private int _stunFrame = 0;
        const int FRAME_COUNT = 3;
        #endregion
        public void Draw(Vector2 screenPos, Color drawColor, float xPos, ref float yPos)
        {
            // Increment frame
            _stunFrameTimer++;
            if (_stunFrameTimer > 8)
            {
                _stunFrameTimer = 0;
                _stunFrame++;
                if (_stunFrame >= FRAME_COUNT)
                {
                    _stunFrame = 0;
                }
            }

            yPos -= 10f;
            
            Vector2 adjustedDrawPos = new Vector2(xPos, yPos) - 0.5f * Assets.StunTwinkle.Size();
            int frameHeight = Assets.StunTwinkle.Value.Height / FRAME_COUNT;
            Rectangle frame = new Rectangle(0, _stunFrame * frameHeight, Assets.StunTwinkle.Value.Width, frameHeight);
            // Draw
            Main.EntitySpriteDraw(new DrawData(Assets.StunTwinkle.Value, adjustedDrawPos - Main.screenPosition, frame, Color.White));
            yPos -= 30f;
        }
    }
    public class TemperatureDrawInfo : IEnemyDrawInfo
    {
        #region Interface definitions
        public TemperatureDrawInfo(NPC npc)
        {
            NPC = npc;
        }
        public bool IsVisible { get => NPC.GetGlobalNPC<TemperatureGlobalNPC>().Temperature != 0; }
        public NPC NPC { get; init; }
        #endregion
        private int _meterBlinkTimer = 0;

        public void Draw(Vector2 screenPos, Color drawColor, float xPos, ref float yPos)
        {
            if (ModContent.GetInstance<DRGClientConfig>().temperatureDisplaySetting == TemperatureOptions.Fancy)
            {
                DrawFancyTemperatureMeter(screenPos, drawColor, xPos, ref yPos);
                return;
            }
            DrawSimpleTemperatureMeter(screenPos, drawColor, xPos, ref yPos);
        }
        private void DrawSimpleTemperatureMeter(Vector2 screenPos, Color drawColor, float xPos, ref float yPos)
        {
            // Adjust draw center
            yPos -= 20;

            // Get real drawing position
            Vector2 adjustedDrawPos = new Vector2(xPos, yPos) - 0.5f * TextureAssets.Hb1.Size();

            // Get draw color
            Color meterColor;
            int temperature = NPC.GetGlobalNPC<TemperatureGlobalNPC>().Temperature;
            if (temperature > 0) // If hot
            {
                meterColor = new Color(Heat.R / 255f, Heat.G / 255f, Heat.B / 255f, 0.9f);
            }
            else
            {
                meterColor = new Color(Cryo.R / 255f, Cryo.G / 255f, Cryo.B / 255f, 0.9f);
            }

            // Get width of the meter
            float tempPercent = temperature switch
            {
                > 0 => (float)temperature / NPC.GetGlobalNPC<TemperatureGlobalNPC>().HeatThreshold,
                < 0 => (float)temperature / NPC.GetGlobalNPC<TemperatureGlobalNPC>().ColdThreshold,
                _ => 0f,
            };
            if (tempPercent == 0) return;
            int filledMeterWidth = (int)(tempPercent * 36f);
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
                    Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb2.Value, new Vector2(adjustedDrawPos.X - screenPos.X + filledMeterWidth, adjustedDrawPos.Y - screenPos.Y), (Rectangle?)new Rectangle(2, 0, 2, TextureAssets.Hb2.Height()), meterColor));
                }
                // Draw outline of right edge of meter
                if (filledMeterWidth < 34)
                {
                    Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb2.Value, new Vector2(adjustedDrawPos.X - screenPos.X + filledMeterWidth + 2, adjustedDrawPos.Y - screenPos.Y), (Rectangle?)new Rectangle(filledMeterWidth + 2, 0, 36 - filledMeterWidth - 2, TextureAssets.Hb2.Height()), meterColor));
                }
                // Draw left edge of meter
                if (filledMeterWidth > 2)
                {
                    Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb1.Value, new Vector2(adjustedDrawPos.X - screenPos.X, adjustedDrawPos.Y - screenPos.Y), (Rectangle?)new Rectangle(0, 0, filledMeterWidth - 2, TextureAssets.Hb1.Height()), meterColor));
                }
                // The actual filled meter
                Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb1.Value, new Vector2(adjustedDrawPos.X - screenPos.X + filledMeterWidth - 2, adjustedDrawPos.Y - screenPos.Y), (Rectangle?)new Rectangle(32, 0, 2, TextureAssets.Hb1.Height()), meterColor));
            }
            else // Close to maximum temperature
            {
                // Draw meter background
                if (filledMeterWidth < 36)
                {
                    Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb2.Value, new Vector2(adjustedDrawPos.X - screenPos.X + filledMeterWidth, adjustedDrawPos.Y - screenPos.Y), (Rectangle?)new Rectangle(filledMeterWidth, 0, 36 - filledMeterWidth, TextureAssets.Hb2.Height()), meterColor));
                }
                // Draw filled meter
                if (filledMeterWidth >= 36)
                {
                    float timer = 0.25f * (float)Math.Sin(_meterBlinkTimer++ * 6.15f) + 1f;
                    Color adjustedColor = new Color(meterColor.R * timer / 255f, meterColor.G * timer / 255f, meterColor.B * timer / 255f, meterColor.A / 255f);
                    Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb1.Value, new Vector2(adjustedDrawPos.X - screenPos.X, adjustedDrawPos.Y - screenPos.Y), (Rectangle?)new Rectangle(0, 0, filledMeterWidth, TextureAssets.Hb1.Height()), adjustedColor));
                    return;
                }
                Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb1.Value, new Vector2(adjustedDrawPos.X - screenPos.X, adjustedDrawPos.Y - screenPos.Y), (Rectangle?)new Rectangle(0, 0, filledMeterWidth, TextureAssets.Hb1.Height()), meterColor));
            }
        }
        public Color Cryo { get => new Color(125, 175, 240); }
        public Color Heat { get => new Color(227, 98, 98); }
        private void DrawFancyTemperatureMeter(Vector2 screenPos, Color drawColor, float xPos, ref float yPos)
        {
            // Adjust draw center
            yPos -= 26;

            // Get real drawing position
            Vector2 adjustedDrawPos = new Vector2(xPos, yPos) - 0.5f * Assets.TemperatureIndicator.Size();

            // Get the correct texture based on if we're drawing hot vs cold
            Texture2D texture;
            Color meterColor;
            int temperature = NPC.GetGlobalNPC<TemperatureGlobalNPC>().Temperature;
            if (temperature > 0) // If hot
            {
                texture = Assets.FireStatusIcon.Value;
                meterColor = Heat;
            }
            else
            {
                texture = Assets.CryoStatusIcon.Value;
                meterColor = Cryo;
            }

            // prepare for meter drawing
            // These are coordinates on the sprite
            const int xOff = 24;
            const int yOff = 16;
            float tempPercent = temperature switch
            {
                > 0 => (float)temperature / NPC.GetGlobalNPC<TemperatureGlobalNPC>().HeatThreshold,
                < 0 => (float)temperature / NPC.GetGlobalNPC<TemperatureGlobalNPC>().ColdThreshold,
                _ => 0f,
            };
            if (tempPercent == 0) return;
            int meterWidth = (int)(tempPercent * 38);
            if (meterWidth < 6)
            {
                meterWidth = 6;
            }

            // Draw background of filled meter (very dark, almost black!)
            Color unfilledMeterColor = new(25, 25, 25);
            Main.EntitySpriteDraw(new DrawData(Assets.TemperatureFilledMeter.Value, new Vector2(adjustedDrawPos.X - screenPos.X + xOff, adjustedDrawPos.Y - screenPos.Y + yOff), unfilledMeterColor));

            // There are 3 parts to drawing this meter. The left edge, the right edge, and the center. Let's start with the left edge, it's the easiest
            Main.EntitySpriteDraw(new DrawData(Assets.TemperatureFilledMeter.Value, new Vector2(adjustedDrawPos.X - screenPos.X + xOff, adjustedDrawPos.Y - screenPos.Y + yOff), (Rectangle?)new Rectangle(0, 0, 2, Assets.TemperatureFilledMeter.Height()), meterColor));

            // Right edge.
            Main.EntitySpriteDraw(new DrawData(Assets.TemperatureFilledMeter.Value, new Vector2(adjustedDrawPos.X - screenPos.X + xOff + meterWidth - 2, adjustedDrawPos.Y - screenPos.Y + yOff), (Rectangle?)new Rectangle(Assets.TemperatureFilledMeter.Width() - 2, 0, 2, Assets.TemperatureFilledMeter.Height()), meterColor));

            // Center
            Main.EntitySpriteDraw(new DrawData(Assets.TemperatureFilledMeter.Value, new Vector2(adjustedDrawPos.X - screenPos.X + xOff + 2, adjustedDrawPos.Y - screenPos.Y + yOff), (Rectangle?)new Rectangle(2, 0, meterWidth - 4, Assets.TemperatureFilledMeter.Height()), meterColor));


            /*Rectangle rect = new((int)(adjustedDrawPos.X - screenPos.X + START - 1), (int)(adjustedDrawPos.Y - screenPos.Y + Y_VALUE), (int)meterWidth, HEIGHT);
            Main.EntitySpriteDraw(new DrawData(Assets.WhitePixel.Value, rect, meterColor));
            // draw dashed meter
            bool shouldDraw = true;
            for (int i = 0; i < meterWidth; i++)
            {
                if (i % 4 == 0)
                {
                    shouldDraw = !shouldDraw;
                }
                if (shouldDraw)
                {
                    Rectangle dashRect = new((int)(adjustedDrawPos.X - screenPos.X + START + i), (int)(adjustedDrawPos.Y - screenPos.Y + Y_VALUE + 1), 2, 4);
                    Color darkened = new Color(0.9f * meterColor.ToVector3());
                    Main.EntitySpriteDraw(new DrawData(Assets.WhitePixel.Value, dashRect, darkened));
                }
            }*/

            // Draw the actual fancy meter border
            Main.EntitySpriteDraw(new DrawData(Assets.TemperatureIndicator.Value, adjustedDrawPos - screenPos, Color.White));

            Color backgroundColor = new Color(.25f, .25f, .25f);
            Color tempIconColor = Color.White;
            // Draw temperature display
            if (temperature == NPC.GetGlobalNPC<TemperatureGlobalNPC>().HeatThreshold || temperature == NPC.GetGlobalNPC<TemperatureGlobalNPC>().ColdThreshold)
            {
                backgroundColor = Color.White;
                tempIconColor = meterColor;
            }
            // Background
            Main.EntitySpriteDraw(new DrawData(Assets.TemperatureStatusBackground.Value, adjustedDrawPos - screenPos, backgroundColor));

            // Temperature icon
            Main.EntitySpriteDraw(new DrawData(texture, adjustedDrawPos - screenPos, tempIconColor));
        }
    }
    public interface IEnemyDrawInfo
    {
        public NPC NPC { get; init; }
        public bool IsVisible { get; }
        public void Draw(Vector2 screenPos, Color drawColor, float xPos, ref float yPos);
    }
}
