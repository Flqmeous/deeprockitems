using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class ElectrifiedEnemy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ElectrifiedEnemyNPC>().IsElectrified = true;
        }
    }
    public class ElectrifiedEnemyNPC : GlobalNPC
    {
        public bool IsElectrified { get; set; }
        public override bool InstancePerEntity => true;
        public override void ResetEffects(NPC npc)
        {
            IsElectrified = false;
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (IsElectrified)
            {
                npc.lifeRegen -= 20; // 10 dps
            }
        }
    }
}
