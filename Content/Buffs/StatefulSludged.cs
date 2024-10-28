using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class StatefulSludged : StatefulBuff
    {
        public bool StrongSludge { get; set; } = false;
        public override void OnCreate() {

        }
        /// <summary>
        /// Adds behavior to this stateful buff.
        /// </summary>
        /// <param name="player">The player that this buff affects.</param>
        public override void Update(Player player) {
            if (StrongSludge)
            {
                player.lifeRegen -= 30;
            }
            else
            {
                player.lifeRegen -= 15;
            }
        }
        /// <summary>
        /// Adds behavior to this stateful buff. Utilize the damage parameter to change the displayed damage/tick.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="damage"></param>
        public override void Update(NPC npc, ref int damage) {
            int damagePerSecond = 15;
            if (StrongSludge)
            {
                damagePerSecond = 30;
            }
            npc.lifeRegen -= damagePerSecond * 2;
            // Display dps
            damage = damagePerSecond;
        }
    }
}
