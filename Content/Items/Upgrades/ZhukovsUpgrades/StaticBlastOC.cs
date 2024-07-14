using deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;
using Terraria.Audio;
using deeprockitems.Content.Buffs;
using Microsoft.Build.Evaluation;
using deeprockitems.Content.Projectiles.ZhukovProjectiles;
using deeprockitems.Assets.Textures;
using Microsoft.Xna.Framework.Graphics;

namespace deeprockitems.Content.Items.Upgrades.ZhukovsUpgrades
{
    public class StaticBlastOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<StaticBlastOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddRecipeGroup(nameof(ItemID.AdamantiteBar), 10)
            .AddIngredient(ItemID.SoulofMight, 15)
            .AddIngredient(ItemID.ThunderStaff)
            .AddTile(TileID.MythrilAnvil)
            .Register();

            Recipe.Create(ModContent.ItemType<StaticBlastOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddRecipeGroup(nameof(ItemID.AdamantiteBar), 10)
            .AddIngredient(ItemID.SoulofMight, 15)
            .AddIngredient(ItemID.ThunderSpear)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
        public class StaticBlastProjectile : UpgradeGlobalProjectile<StaticBlastOC>
        {
            public List<int> WhoHaveICoupledTo = [];
            public bool HaveICoupledYet = false;
            /// <summary>
            /// Which projectile is being arced? Defaults to -1 for no projectile
            /// </summary>
            public int WhoAmICoupledTo = -1;
            public override void UpgradeOnSpawn(Projectile projectile, IEntitySource source)
            {
                // Find another projectile at this position with same owner, and couple them
                foreach (var proj in Main.projectile)
                {
                    if (!proj.active) continue;
                    
                    // Floats arent precise so it only needs to be good enough
                    if (proj.owner == projectile.owner && proj.type == projectile.type && proj.DistanceSQ(projectile.position) <= 50f)
                    {
                        // Couple
                        WhoAmICoupledTo = proj.whoAmI;
                        proj.GetGlobalProjectile<StaticBlastProjectile>().WhoAmICoupledTo = projectile.whoAmI;
                    }
                }
            }
            public List<Vector2> PointsToElectrify = [];
            public override void UpgradeAI(Projectile projectile)
            {
                // Test couplings
                if (!CheckCoupling(projectile))
                {
                    return;
                }

                // Spawn dust
                Dust.NewDust(projectile.Center + new Vector2(MathF.Cos((int)Main.timeForVisualEffects + projectile.whoAmI), MathF.Sin((int)Main.timeForVisualEffects + projectile.whoAmI)), 4, 4, DustID.Electric, Scale: 0.8f);

                PointsToElectrify = [projectile.Center];

                // Check for NPC between the coupled projectiles in striking range
                /*foreach (var npc in Main.npc)
                {
                    if (!npc.active) continue;
                    
                    // If too far away, don't even bother
                    if (npc.Center.DistanceSQ(projectile))
                }*/

                Vector2 initial = projectile.Center;
                Vector2 lerpPos = initial;
                Vector2 directionTo = projectile.Center.DirectionTo(Main.projectile[WhoAmICoupledTo].Center);
                float distance = projectile.Center.Distance(Main.projectile[WhoAmICoupledTo].Center);

                // Check if damage can be dealt 9 times
                const int CONTROL_POINTS = 9;
                List<int> npcs_already_added = [];
                for (int i = 0; i < CONTROL_POINTS; i++)
                {
                    // Lerp to each control point
                    lerpPos = initial + directionTo * (i / (float)CONTROL_POINTS) * distance;

                    // Find NPCs that can be damaged
                    var npcs = from npc in Main.npc
                               where npc.active
                               && !npc.friendly
                               && PointsToElectrify.Count < 15
                               && npc.DistanceSQ(lerpPos) <= (5 * 16 * 5 * 16) // 5 block distance
                               && !npcs_already_added.Contains(npc.whoAmI)
                               select npc;

                    // Damage npcs
                    foreach (var npc in npcs)
                    {
                        PointsToElectrify.Add(npc.Center); // Add NPC hitbox for drawing

                        if (npc.immune[projectile.owner] == 0)
                        {
                            var info = npc.CalculateHitInfo((int)(projectile.damage * 0.75f), 1, damageType: DamageClass.Ranged, damageVariation: true); // Damage npc
                            Main.player[projectile.owner].StrikeNPCDirect(npc, info); // Strike NPC
                            npc.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 300); // Add electricity buff
                            npc.immune[projectile.owner] = 10; // Become immune for 10 frames
                        }
                    }
                    // Add npcs
                    npcs_already_added.AddRange(npcs.Select(npc => npc.whoAmI));
                }

                // Add the final electrification point (the right projectile)
                PointsToElectrify.Add(Main.projectile[WhoAmICoupledTo].Center);


                /*if (HaveICoupledYet)
                {
                    // Find if the children have died, if so, uncouple
                    CheckCoupling();

                    // Spawn dust
                    Dust.NewDust(projectile.Center + new Vector2(MathF.Cos((int)Main.timeForVisualEffects + projectile.whoAmI), MathF.Sin((int)Main.timeForVisualEffects + projectile.whoAmI)), 4, 4, DustID.Electric, Scale: 0.8f);
                    return;
                }
                if (!HaveICoupledYet)
                {
                    // Get selection of close projectiles, sorted from nearest to farthest
                    var projectilesToCouple = from proj in Main.projectile
                                              where proj.active
                                              && !proj.GetGlobalProjectile<StaticBlastProjectile>().HaveICoupledYet
                                              && proj.type == projectile.type
                                              && proj != projectile
                                              && projectile.Center.DistanceSQ(proj.Center) <= 5000
                                              orderby projectile.Center.DistanceSQ(proj.Center) ascending
                                              select proj;

                    // Get closest projectile
                    Projectile closestCandidate = projectilesToCouple.FirstOrDefault();

                    if (closestCandidate is default(Projectile)) return; // If no projectile matched criteria, return

                    // Couple
                    HaveICoupledYet = true;
                    closestCandidate.GetGlobalProjectile<StaticBlastProjectile>().HaveICoupledYet = true;

                    const int COUNT = 9;

                    Vector2 initialVelocity = projectile.velocity;
                    Vector2 finalVelocity = closestCandidate.velocity;

                    Vector2 initialPosition = projectile.position;
                    Vector2 finalPosition = closestCandidate.position;

                    List<Projectile> electricityProjectiles = new();
                    float middleProjectileRotation = 0f;

                    // Initialize whoamI with two bullets
                    WhoHaveICoupledTo = [projectile.whoAmI, closestCandidate.whoAmI];


                    float fractionalSpaceBetween = 1;

                    // Divide distance equally by count to yield how much space should be between each projectile
                    float projectileSpace = fractionalSpaceBetween / COUNT;

                    // Simulate projectile space with fractions
                    float currentDistance = 0;

                    // For each projectile, place in the middle of each projectile's space
                    for (int i = 0; i < COUNT; i++)
                    {

                        // Add half of projectile's assumed space
                        currentDistance += 0.5f * projectileSpace;

                        // Lerp velocity with fractional distance
                        Vector2 velocity = initialVelocity + currentDistance * (finalVelocity - initialVelocity);

                        // Spawn projectile
                        Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), Main.player[projectile.owner].Center, velocity, ModContent.ProjectileType<ElectricityArc>(), (int)(projectile.damage * 0.5f), 0f, owner: projectile.owner);
                        proj.extraUpdates = projectile.extraUpdates;
                        if (i == (COUNT - 1) / 2)
                        {
                            // Set rotation
                            middleProjectileRotation = proj.velocity.ToRotation() + MathHelper.PiOver2;
                        }
                        // Add electrified projectiles to list
                        WhoHaveICoupledTo.Add(proj.whoAmI);
                        electricityProjectiles.Add(proj);

                        // Add other half of space
                        currentDistance += 0.5f * projectileSpace;
                    }

                    // Set all other rotations from the middle one
                    foreach (Projectile proj in electricityProjectiles)
                    {
                        proj.rotation = middleProjectileRotation;
                    }

                    // Clone data to second bullet
                    closestCandidate.GetGlobalProjectile<StaticBlastProjectile>().WhoHaveICoupledTo = [WhoHaveICoupledTo[1], WhoHaveICoupledTo[0], .. WhoHaveICoupledTo[2..]];
                }*/
            }
            private bool CheckCoupling(Projectile projectile)
            {
                /*// For each whoAmI
                foreach (int whoAmI in WhoHaveICoupledTo)
                {
                    // If projectile is inactive, continue
                    if (Main.projectile[whoAmI].active) continue;

                    // If projectile is inactive, then _something_ died. Kill 'em all (Metallica, July 25, 1983)!
                    KillAndResetCoupling();
                    return;
                }*/
                // If there is a desync between coupling or projectiles, reset
                if (WhoAmICoupledTo == -1)
                {
                    return false;
                }
                
                // Check if both projectiles are active
                if (Main.projectile[WhoAmICoupledTo].active && projectile.active)
                {
                    // Continue as normal
                    return true;
                }
                // Otherwise, detach both couplings
                Main.projectile[WhoAmICoupledTo].GetGlobalProjectile<StaticBlastProjectile>().WhoAmICoupledTo = -1;
                WhoAmICoupledTo = -1;
                return false;
            }
            private void DetachCoupling()
            {

            }
            private void KillAndResetCoupling()
            {
                foreach (int whoAmI in WhoHaveICoupledTo)
                {
                    if (Main.projectile[whoAmI].type == ModContent.ProjectileType<ElectricityArc>())
                    {
                        Main.projectile[whoAmI].Kill();
                    }
                    else
                    {
                        // Reset coupling
                        Main.projectile[whoAmI].GetGlobalProjectile<StaticBlastProjectile>().HaveICoupledYet = false;
                    }
                }
            }
            public override void UpgradeOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 300);
            }
            public override void UpgradeOnKill(Projectile projectile, int timeLeft)
            {
                // KillAndResetCoupling();
            }
            public override void PostDraw(Projectile projectile, Color lightColor)
            {
                // Don't draw if there's only 2 points to electrify
                if (PointsToElectrify.Count < 3)
                {
                    return;
                }
                // Iterate through pairs
                for (int i = 0; i < PointsToElectrify.Count - 1; i++)
                {
                    Vector2 point1 = PointsToElectrify[i];
                    Vector2 point2 = PointsToElectrify[i + 1];

                    Vector2 midpoint = new((point1.X + point2.X) / 2f, (point1.Y + point2.Y) / 2f);

                    // Calculate scale via distance
                    // the arc is 48 pixels, 3 blocks long at 1f scale.
                    float pixelDistance = point1.Distance(point2);

                    int frame = Main.rand.Next(0, 3);
                    int frameHeight = DRGTextures.ElectricityArc.Height / 3;
                    Rectangle sourceFrame = new(0, frame * frameHeight, DRGTextures.ElectricityArc.Width, frameHeight);

                    // Get scale from distance between control points
                    float multiplier = pixelDistance / 48f;

                    // Draw
                    Main.EntitySpriteDraw(DRGTextures.ElectricityArc, midpoint - Main.screenPosition, sourceFrame, Color.White, point1.DirectionTo(point2).ToRotation(), sourceFrame.Size() / 2f, new Vector2(multiplier, frame),SpriteEffects.None);
                }
            }
        }
        public override void ItemStatChangeOnEquip(UpgradeableItemTemplate modItem)
        {
            modItem.Item.useAnimation = modItem.Item.useTime;
        }
        public override bool UpgradeItem_ShootPrimaryUse(UpgradeableItemTemplate sender, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool callBase)
        {
            // Get 30 degree offset
            Vector2 topVelocity = velocity.RotatedBy(-Math.PI / 6);
            Vector2 bottomVelocity = velocity.RotatedBy(Math.PI / 6);
            // Spawn projectiles
            Projectile.NewProjectile(source, position, topVelocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, bottomVelocity, type, damage, knockback, player.whoAmI);
            // Play sound
            SoundEngine.PlaySound(SoundID.Item41, player.Center);
            callBase = false;
            return false;
        }
    }
}
