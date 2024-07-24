using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class StunnedEnemy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<StunnedEnemyNPC>().IsStunned = true;
        }
    }
    public class StunnedEnemyNPC : GlobalNPC
    {
        public bool IsStunned { get; set; } = false;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC entity)
        {
            entity.buffImmune[ModContent.BuffType<StunnedEnemy>()] = entity.boss;
        }
        public override void ResetEffects(NPC npc)
        {
            IsStunned = false;
        }
        public override bool PreAI(NPC npc)
        {
            if (IsStunned)
            {
                return false;
            }
            return base.PreAI(npc);
        }
    }
}
