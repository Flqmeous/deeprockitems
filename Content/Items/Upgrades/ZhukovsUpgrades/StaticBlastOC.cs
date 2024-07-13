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
            /// <summary>
            /// Which projectile is being arced? Defaults to -1 for no projectile
            /// </summary>
            public int WhoAmICoupledTo = -1;
            public bool HaveICoupledYet = false;
            public override void UpgradeAI(Projectile projectile)
            {
                /*if (!HaveICoupledYet)
                {
                    // Get selection of close projectiles, sorted from nearest to farthest
                    var projectilesToCouple = from proj in Main.projectile
                                              where proj.active
                                              && proj.GetGlobalProjectile<StaticBlastProjectile>().WhoAmICoupledTo == -1
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

                    List<Projectile> projectiles = new();
                    float middleProjectileRotation = 0f;

                    // Spawn electricity projectiles
                    for (int i = 0; i < COUNT; i++)
                    {
                        if (i == 0 || i == COUNT - 1)
                        {
                            continue;
                        }
                        // Compute velocity and position
                        Vector2 velocity = initialVelocity + (i / (COUNT - 1f)) * (finalVelocity - initialVelocity);

                        Vector2 position = initialPosition + (i / (COUNT - 1f)) * (finalPosition - initialPosition);
                        Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<ElectricityArc>(), (int)(projectile.damage * 0.85f), 0f, owner: projectile.owner);
                        proj.extraUpdates = projectile.extraUpdates;
                        projectiles.Add(proj);

                        if (i == (COUNT + 1) / 2f)
                        {
                            // Set rotation based on velocity
                            middleProjectileRotation = proj.velocity.ToRotation() + MathHelper.PiOver2;
                        }
                    }

                    foreach (var proj in projectiles)
                    {
                        proj.rotation = middleProjectileRotation;
                    }
                }*/
                /*// If not coupled to a projectile, try to do it.
                if (WhoAmICoupledTo == -1)
                {
                    // Get selection of uncoupled projectiles, within 31.25 blocks, sorted from nearest to farthest
                    var projectilesToCouple = from proj in Main.projectile
                                              where proj.active
                                              && proj.GetGlobalProjectile<StaticBlastProjectile>().WhoAmICoupledTo == -1
                                              && proj.type == projectile.type
                                              && proj != projectile
                                              && projectile.Center.DistanceSQ(proj.Center) <= 5000
                                              orderby projectile.Center.DistanceSQ(proj.Center) ascending
                                              select proj;

                    // Get closest projectile
                    Projectile closestCandidate = projectilesToCouple.FirstOrDefault();

                    if (closestCandidate is default(Projectile)) return; // If no projectile matched criteria, return
                    // Couple closest projectile
                    WhoAmICoupledTo = closestCandidate.whoAmI;
                    closestCandidate.GetGlobalProjectile<StaticBlastProjectile>().WhoAmICoupledTo = projectile.whoAmI;
                }
                else
                {
                    Projectile coupledTo = Main.projectile[WhoAmICoupledTo];
                    // Make sure projectiles are alive / active
                    if (!coupledTo.active)
                    {
                        ResetCoupling_ProjectileLikelyDied(coupledTo);
                    }
                    // Spawn arc between sender and this projectile
                    // initial
                    Vector2 initial = projectile.Center;
                    Vector2 pos = initial;
                    // Distance
                    float distance = initial.Distance(coupledTo.Center);
                    for (int i = 0; i < 30; i++)
                    {
                        // Draw position of the dust
                        Vector2 drawPos = pos;
                        float drawScale = 0.3f;
                        // If drawing the last couple positions, draw spinning dust
                        if (i == 0 || i == 29)
                        {
                            drawPos += 16 * new Vector2(MathF.Cos((int)Main.timeForVisualEffects + projectile.whoAmI), MathF.Sin((int)Main.timeForVisualEffects + projectile.whoAmI));
                            drawScale = 0.8f;
                        }
                        // Spawn dust
                        var dust = Dust.NewDust(drawPos, 8, 8, DustID.Electric, Scale: drawScale);

                        // Try to damage an NPC every few ticks
                        if (i % 5 == 1)
                        {
                            // Try to damage npc
                            foreach (var npc in Main.npc)
                            {
                                if (!npc.active) continue;
                                if (npc.friendly) continue;
                                if (npc.immune[coupledTo.owner] > 0) continue;

                                if (npc.Hitbox.Intersects(new Rectangle((int)pos.X - 6, (int)pos.Y - 6, 12, 12)))
                                {
                                    var info = npc.GetIncomingStrikeModifiers(coupledTo.DamageType, 0).ToHitInfo(coupledTo.damage * 0.5f, false, 0f, true);
                                    Main.player[coupledTo.owner].StrikeNPCDirect(npc, info);
                                    npc.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 240);
                                    npc.immune[coupledTo.owner] += 10;
                                }
                            }
                        }

                        // Lerp between sender and this projectile
                        pos += initial.DirectionTo(coupledTo.Center) * (distance / 30);
                    }
                }*/
            }
            private void ResetCoupling_ProjectileLikelyDied(Projectile projectile)
            {
                foreach (var proj in Main.projectile)
                {
                    if (proj.active && proj.GetGlobalProjectile<StaticBlastProjectile>().WhoAmICoupledTo == projectile.whoAmI)
                    {
                        proj.GetGlobalProjectile<StaticBlastProjectile>().WhoAmICoupledTo = -1;
                    }
                }
            }
            public override void UpgradeOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 300);
            }
            public override void UpgradeOnKill(Projectile projectile, int timeLeft)
            {
                ResetCoupling_ProjectileLikelyDied(projectile);
            }
        }
        public override void UpgradeProjectile_AI(Projectile sender)
        {
            // Query for active projectiles, of the same type, spawned through upgrade, grab closest
            var projectiles = from proj in Main.projectile
                              where proj.active
                              && proj.type == sender.type
                              && proj != sender
                              && sender.Center.DistanceSQ(proj.Center) <= 250000
                              orderby sender.Center.DistanceSQ(proj.Center) ascending
                              select proj;
            // Electrify!
            Projectile toElectrify = projectiles.FirstOrDefault();

            if (toElectrify is default(Projectile)) return;

            // Spawn arc between sender and this projectile
            // initial
            Vector2 initial = sender.Center;
            Vector2 pos = initial;
            // Distance
            float distance = initial.Distance(toElectrify.Center);
            for (int i = 0; i < 30; i++)
            {
                // Spawn dust
                var dust = Dust.NewDust(pos, 8, 8, DustID.Electric, Scale: 0.3f);

                // Try to damage an NPC every few ticks
                if (i % 5 == 1)
                {
                    // Try to damage npc
                    foreach (var npc in Main.npc)
                    {
                        if (!npc.active) continue;
                        if (npc.friendly) continue;
                        if (npc.immune[toElectrify.owner] > 0) continue;

                        if (npc.Hitbox.Intersects(new Rectangle((int)pos.X - 6, (int)pos.Y - 6, 12, 12)))
                        {
                            var info = npc.GetIncomingStrikeModifiers(toElectrify.DamageType, 0).ToHitInfo(toElectrify.damage * 0.75f, false, 0f, true);
                            Main.player[toElectrify.owner].StrikeNPCDirect(npc, info);
                            npc.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 60);
                            npc.immune[toElectrify.owner] += 10;
                        }
                    }
                }

                // Lerp between sender and this projectile
                pos += initial.DirectionTo(toElectrify.Center) * (distance / 30);
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
