using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public abstract class InstancedBuff : ModBuff
    {
        public override void SetStaticDefaults() {
            IsTemplateInstance = true;
        }
        public bool IsTemplateInstance { get; set; } = false;
        #region Singleton members
        public sealed override bool ReApply(NPC npc, int time, int buffIndex) {
            return base.ReApply(npc, time, buffIndex);
        }
        public sealed override void Update(NPC npc, ref int buffIndex) {
            //npc.GetGlobalNPC<InstancedNPC>().ApplyInstancedBuff(npc, this, buffIndex);
        }
        #endregion

        #region Instance members
        public NPC ThisNPC { get; set; }
        public int TimeLeft
        {
            get
            {
                if (ThisNPC is null) return -1;
                return ThisNPC.buffTime[BuffIndex];
            }
        }
        public int InstancedType { get; set; } = -1;
        public int BuffIndex { get; set; } = -1;
        public virtual void UpdateLifeRegen(NPC npc, ref int damage) {

        }
        public virtual bool ReapplyNPC(NPC npc) => true;
        #endregion
    }
    public static class Extensions
    {
        /// <summary>
        /// Attempts to add an instanced buff to this NPC. 
        /// </summary>
        /// <typeparam name="T">The instanced buff</typeparam>
        /// <param name="npc"></param>
        /// <param name="time"></param>
        /// <param name="instancedBuff">Is null if the function returns false</param>
        /// <param name="quiet"></param>
        /// <returns></returns>
        public static bool AddInstancedBuff<T>(this NPC npc, int time, out T? instancedBuff, bool quiet = false) where T : InstancedBuff, new() {
            // Get global npc
            InstancedNPC globalNpc = npc.GetGlobalNPC<InstancedNPC>();
            // Search for the buff type
            int buffIndex = -1;
            int buffType = ModContent.BuffType<T>();
            T buff;
            buffIndex = npc.FindBuffIndex(buffType);
            // If this buff isn't on the NPC yet, add it
            if (buffIndex == -1)
            {
                // Add buff
                npc.AddBuff(buffType, time, quiet);
                buffIndex = npc.FindBuffIndex(buffType);
                if (buffIndex == -1) // This is rare, but in case the enemy can't receive the buff, we need to exit anyway.
                {
                    instancedBuff = null;
                    return false;
                }
                // Create an instance of the buff with buff parameters
                buff = new T() {
                    InstancedType = buffType,
                    // TimeLeft = time,
                    BuffIndex = buffIndex,
                    ThisNPC = npc
                };
                globalNpc.InstancedBuffs.Add(buff);
                instancedBuff = buff;
                return true;
            }
            // Buff exists on NPC, reapply.
            int instancedIndex = globalNpc.InstancedBuffs.FindIndex(buff => buff.InstancedType == buffType);
            if (instancedIndex == -1) // Add the instanced buff, realistically should never happen
            {
                buff = new() {
                    InstancedType = buffType,
                    BuffIndex = buffIndex,
                    ThisNPC = npc,
                };
                globalNpc.InstancedBuffs.Add(buff);
                instancedIndex = globalNpc.InstancedBuffs.FindIndex(b => b == buff);
            }
            // Now we can grab the instanced of this buff
            buff = (T)globalNpc.InstancedBuffs[instancedIndex];
            // Actually reapply the buff!
            if (buff.ReapplyNPC(npc))
            {
                npc.buffTime[buffIndex] = time;
            }
            // Set outputs
            instancedBuff = buff;
            return true;
        }
    }
    public class InstancedBuffSystem : ModSystem
    {
        public override void Load() {
            IL_NPC.DelBuff += IL_NPC_DelBuff;
        }

        private void IL_NPC_DelBuff(ILContext il) {
            var cursor = new ILCursor(il);
            // We are going to navigate to 4 lines before the subtraction call.
            cursor.GotoNext(i => i.MatchSub());
            cursor.Index -= 4;
            // Create delegate that will shift the indices of the instanced buffs.
            static void deleteBuffHook(NPC npc, int oldIndex) {
                InstancedNPC globalNpc = npc.GetGlobalNPC<InstancedNPC>();
                var query = globalNpc.InstancedBuffs.Where(buff => buff.InstancedType == npc.buffType[oldIndex]);
                if (query.Any())
                {
                    var buff = query.First();
                    buff.BuffIndex--; // Shift the buffindex down one.
                }
            };
            // Push our values to the stack and then push the delegate.
            cursor.EmitLdarg0(); // arg 0 is the 'this' keyword
            cursor.EmitLdloc1(); // loc 1 is j (old buff index)
            cursor.EmitDelegate(deleteBuffHook);
        }
    }
    public class InstancedNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public List<InstancedBuff> InstancedBuffs = new();
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            // Update each buff. We are removing buffs as we iterate, so we have to loop backwards
            for (int i = InstancedBuffs.Count - 1; i >= 0; i--)
            {
                // Remove any buffs that have buff time less than 0.
                if (InstancedBuffs[i].TimeLeft <= 0)
                {
                    InstancedBuffs.RemoveAt(i);
                    continue; // Don't continue with this index.
                }
                // This ensures that buffs only set damage if it was greater than or equal to. Realistically, each buff should check this, but just in case we will do it here as well.
                int newDamage = damage;
                // Call the life regen hook.
                InstancedBuffs[i].UpdateLifeRegen(npc, ref newDamage);
                // Set damage if greater or equal to.
                if (newDamage > damage)
                {
                    damage = newDamage;
                }
            }
        }
    }
}
