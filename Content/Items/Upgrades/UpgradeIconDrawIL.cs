using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MonoMod;
using MonoMod.Cil;
using Terraria.UI;
using System.Reflection;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Mono.Cecil.Cil;
using deeprockitems.Assets.Textures;

namespace deeprockitems.Content.Items.Upgrades
{
    public class UpgradeIconDrawIL : ModSystem
    {
        public override void Load()
        {
            IL_ItemSlot.DrawItemIcon += IL_ItemSlot_DrawItemIcon;
        }

        private void IL_ItemSlot_DrawItemIcon(ILContext il)
        {
            try
            {
                // Create cursor
                ILCursor c = new(il);

                // Navigate to the correct location (right before PostDraw)
                c.GotoNext(i => i.MatchCall(typeof(ItemLoader).GetMethod(nameof(ItemLoader.PostDrawInInventory), BindingFlags.Public | BindingFlags.Static)));
                c.Index--;

                // Push all required values to stack, then call my delegate
                // We need item, scale, position, spritebatch
                c.Emit(OpCodes.Ldarg_1);
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldarg_S, (byte)4);
                c.Emit(OpCodes.Ldarg_3);
                c.Emit(OpCodes.Ldarg_2);

                c.EmitDelegate<Action<int, Item, float, Vector2, SpriteBatch>>((context, item, scale, position, spriteBatch) =>
                {
                    if (context != 31 && item.ModItem is UpgradeTemplate upgrade && upgrade.ValidWeapons.Count > 0)
                    {
                        // Initialize index
                        upgrade.IndexToDrawWeapon = -1;

                        // Determine correct index to draw
                        if (Main.timeForVisualEffects % 90 == 0) upgrade.WeaponDrawTimer++;
                        upgrade.IndexToDrawWeapon = upgrade.WeaponDrawTimer % upgrade.ValidWeapons.Count;

                        if (upgrade.IndexToDrawWeapon != -1)
                        {
                            Texture2D texture;
                            // Get texture from index of weapon to draw
                            if (DRGTextures.WeaponIconography.ContainsKey(upgrade.ValidWeapons[upgrade.IndexToDrawWeapon]))
                            { 
                                texture = DRGTextures.WeaponIconography[upgrade.ValidWeapons[upgrade.IndexToDrawWeapon]].Value;
                            }
                            else
                            {
                                texture = TextureAssets.Item[upgrade.ValidWeapons[upgrade.IndexToDrawWeapon]].Value;
                            }

                            // Get position of bottom of slot:
                            float newScale = scale * (texture.Width <= 40f ? 0.65f : 0.5f);
                            float yOffset = texture.Height * 0.5f * newScale;
                            Vector2 bottomLeftOfSlot = new Vector2(position.X - 0.5f * scale * 52 + 4f, position.Y + scale * 0.5f * 52 - 4f);
                            Vector2 drawPos = new Vector2(bottomLeftOfSlot.X, bottomLeftOfSlot.Y - yOffset);
                            spriteBatch.Draw(texture, new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)(texture.Width * newScale), (int)(texture.Height * newScale)), Color.White);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MonoModHooks.DumpIL(Mod, il);
            }
            // Navigate to the spot where
        }
    }
}
