using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace deeprockitems.Common.PlayerLayers
{
    public class ZhukovsDrawPlayer : ModPlayer
    {
        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (!drawInfo.drawPlayer.ItemAnimationActive) return;
            if (drawInfo.heldItem.type != ModContent.ItemType<Content.Items.Weapons.Zhukovs>()) return;

            PlayerDrawLayers.HeldItem.Hide();
        }
    }
}
