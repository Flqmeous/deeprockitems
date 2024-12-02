using Terraria.ModLoader;
using Terraria.DataStructures;

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
