using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using deeprockitems.Assets.Textures;
using deeprockitems.Content.Items.Weapons;
using System.Linq;
using System;

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
                const float distanceMultiplier = 1f;
                // Base distance
                const float distance = 600;
                bool lavaWet = false;
                bool wet = Collision.WetCollision(position, width, height);

                // Total distance of the dots
                for (int i = 0; i < distance * distanceMultiplier; i++)
                {
                    bool visibleLine = i % 4 == 0;

                    // Simulate AI (gravity)
                    velocity.Y += gravityStrength * (1 / distanceMultiplier);

                    // Determine if projectile is wet
                    bool justGotWet;
                    try
                    {
                        lavaWet = Collision.LavaCollision(position, width, height);
                        justGotWet = Collision.WetCollision(position, width, height);
                    }
                    catch
                    {
                        return;
                    }

                    if (justGotWet)
                    {
                        wet = true;
                    }
                    else if (wet)
                    {
                        wet = false;
                    }

                    if (!wet)
                    {
                        lavaWet = false;
                    }

                    // Set velocities
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

                    // Slope collision

                    if (wet)
                    {
                        position += wetVelocity * (1 / distanceMultiplier);
                    }
                    else
                    {
                        position += velocity * (1 / distanceMultiplier);
                    }
                    computedCenter = new(position.X + 0.5f * width, position.Y + 0.5f * height);
                    drawInfo.DrawDataCache.Add(new DrawData(DRGTextures.WhitePixel, new Rectangle((int)(computedCenter.X - Main.screenPosition.X - 0.5f * tracerWidth), (int)(computedCenter.Y - Main.screenPosition.Y - 0.5f * tracerHeight), tracerWidth, tracerHeight), Color.White));


                }
            }
        }
    }
}
