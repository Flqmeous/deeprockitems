using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace deeprockitems.UI.Upgrades
{
    public class ItemSlotWrapper : UIElement
    {
        internal ItemSlot _slot;
        public override void LeftClick(UIMouseEvent evt)
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            ItemSlot.Handle()
        }
    }
}
