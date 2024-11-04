using deeprockitems.Content.Buffs;
using Terraria;

namespace deeprockitems.Content.Buffs
{
    public class Sludged : InstancedBuff
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            Main.NewText(TimeLeft);
            int dps = StrongSludge ? 30 : 15;
            npc.lifeRegen -= dps * 2;
            damage = dps;
        }
        public bool StrongSludge { get; set; } = false;
    }
}