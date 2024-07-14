using Terraria.ModLoader;
using Terraria;

namespace deeprockitems.Content.Buffs
{
    public class Haste : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<HastePlayer>().HasHaste = true;
        }
    }
    public class HastePlayer : ModPlayer
    {
        public bool HasHaste { get; set; } = false;
        public override void ResetEffects()
        {
            HasHaste = false;
        }
        public override void PreUpdateMovement()
        {
            if (HasHaste)
            {
                Player.moveSpeed *= 1.5f;
            }   
        }
    }
}
