using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using deeprockitems.Assets.Textures;
using deeprockitems.Content.Items.Weapons;
using System.Linq;
using System;
using deeprockitems.Utilities;

namespace deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades
{
    public class TracerRoundDrawLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeldItem);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.HeldItem.ModItem is not SludgePump pump) return;
            if (pump.Upgrades.Contains(ModContent.ItemType<TracerRounds>()) && Main.myPlayer == drawInfo.drawPlayer.whoAmI)
            {
                // Simulate a projectile
                Vector2 velocity = (Main.MouseWorld - drawInfo.drawPlayer.Center);
                velocity.Normalize();
                velocity = drawInfo.drawPlayer.HeldItem.shootSpeed * velocity;
                float gravityStrength = velocity.Y > 30f || pump.Overclock == ModContent.ItemType<AntiGravOC>() ? 0f : 0.5f;

                //Projectile.NewProjectile(drawInfo.drawPlayer.GetSource_ItemUse(drawInfo.drawPlayer.HeldItem), drawInfo.drawPlayer.Center, velocity, ModContent.ProjectileType<SludgeBall>(), 0, 0, drawInfo.drawPlayer.whoAmI);
                // Start is the player's center
                Vector2 start = drawInfo.drawPlayer.Center;
                int width = 20;
                int height = 20;
                int tracerWidth = 2;
                int tracerHeight = 2;
                // This is the center of the projecitle. Used for drawing the tracer
                Vector2 computedCenter = start;
                // This is the top left of the projectile. The projectile's center spawns on the player's center
                Vector2 position = new Vector2(start.X - 0.5f * width, start.Y - 0.5f * height);
                // How close together to put the dots
                const float distanceMultiplier = 10f;
                // Base distance
                const float distance = 120;
                // Test for wetness very first
                bool wet = Collision.WetCollision(position, width, height);
                Color playerColor = DRGHelpers.GetTeamColor(drawInfo.drawPlayer.team); // Get a color corresponding to team color

                // Iteration count
                int timer = 0;

                // Total distance of the dots
                for (int i = 0; i < distance; i++)
                {
                    // Simulate AI (gravity)
                    velocity.Y += gravityStrength;

                    // Determine if projectile is wet
                    bool justGotWet;
                    try
                    {
                        justGotWet = Collision.WetCollision(position, width, height);
                    }
                    catch
                    {
                        return;
                    }

                    // This sets wetness on the frame _after_ entering water.
                    if (justGotWet)
                    {
                        wet = true;
                    }
                    else if (wet)
                    {
                        wet = false;
                    }

                    // Set velocities depending on the fluid.
                    Vector2 wetVelocity = new Vector2(0, 0);
                    if (wet)
                    {
                        if (Collision.shimmer)
                        {
                            wetVelocity = velocity * 0.375f;
                        }
                        else if (Collision.honey)
                        {
                            wetVelocity = velocity * 0.25f;
                        }
                        else
                        {
                            wetVelocity = velocity * 0.5f;
                        }
                    }
                    // Interpolation. This yields something slightly different to a parabola, but it looks close enough
                    for (int j = 0; j < distanceMultiplier; j++)
                    {
                        // Use wet velocity if wet
                        if (wet)
                        {
                            position += wetVelocity * (1 / distanceMultiplier);
                        }
                        else
                        {
                            position += velocity * (1 / distanceMultiplier);
                        }
                        computedCenter = new(position.X + 0.5f * width, position.Y + 0.5f * height);
                        // Check visibility
                        if ((30 + timer - ((int)Main.timeForVisualEffects % 30)) % 30 < 15)
                        {
                            // Draw the individual squares
                            drawInfo.DrawDataCache.Add(new DrawData(DRGTextures.WhitePixel, new Rectangle((int)(computedCenter.X - Main.screenPosition.X - 0.5f * tracerWidth), (int)(computedCenter.Y - Main.screenPosition.Y - 0.5f * tracerHeight), tracerWidth, tracerHeight), playerColor));
                        }
                        timer++;
                    }

                }
            }
        }
    }
}
