using deeprockitems.Content.Upgrades;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    /// <summary>
    /// Stateful buffs act similar to buffs, but they are instanced when adding to an NPC or player rather than by ID. Use this instead of buffs when you want a buff's effect to change under some condition.
    /// </summary>
    public abstract class StatefulBuff : ModType
    {
        protected sealed override void Register() {
            
        }
        public virtual void OnCreate() {

        }
        public List<Tuple<Func<bool>, Action>> StateChanges { get; set; } = new();
        /// <summary>
        /// Adds a condition and resulting action to this buff.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public StatefulBuff AddStateChange(Func<bool> condition, Action action) {
            StateChanges.Add(new Tuple<Func<bool>, Action>(condition, action));
            return this;
        }
        /// <summary>
        /// Called when the buff was reapplied to this <see cref="NPC"/>. Resets the buff time by default.<br/>Return false to prevent resetting the buff time.
        /// </summary>
        /// <returns></returns>
        public virtual bool OnReapply(NPC npc) => true;
        /// <summary>
        /// Called when the buff was reapplied to this <see cref="Player"/>. Resets the buff time by default.<br/>Return false to prevent resetting the buff time.
        /// </summary>
        /// <returns></returns>
        public virtual bool OnReapply(Player player) => true;

        public int TimeLeft { get; set; } = 0;
        /// <summary>
        /// Adds behavior to this stateful buff. Utilize the damage parameter to change the displayed damage/tick.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="damage"></param>
        public virtual void Update(NPC npc, ref int damage) {
            
        }
        /// <summary>
        /// Called after NPC's <see cref="Update(NPC, ref int)">Update</see>. Useful for setting movement speed
        /// </summary>
        /// <param name="npc"></param>
        public virtual void PostUpdate(NPC npc) {

        }
        /// <summary>
        /// Adds behavior to this stateful buff.
        /// </summary>
        /// <param name="player">The player that this buff affects.</param>
        public virtual void Update(Player player) {

        }
        /// <summary>
        /// Called when the buff is removed from this NPC.
        /// </summary>
        /// <param name="npc"></param>
        public virtual void OnRemove(NPC npc) {

        }
        /// <summary>
        /// Called when the buff is removed from this player.
        /// </summary>
        /// <param name="player"></param>
        public virtual void OnRemove(Player player) {

        }
    }
    public static class BuffExtensions {
        public static T AddStatefulBuff<T>(this NPC npc, int time) where T : StatefulBuff, new() {
            var buffs = npc.GetGlobalNPC<StatefulBuffNPC>().Buffs;
            // Determine if buff should be refreshed or recreated.
            if (buffs.FindIndex(buff => buff.GetType() == typeof(T)) is int index && index > -1)
            {
                // Replace buff
                T thisBuff = buffs[index] as T;
                // Reapply buff and extend buff time if requested.
                if (thisBuff.OnReapply(npc) && thisBuff.TimeLeft < time)
                {
                    thisBuff.TimeLeft = time;
                }
                // Return the buff
                return thisBuff;
            }
            // Otherwise create a new buff
            T statefulBuff = new();
            // Create buff
            statefulBuff.OnCreate();
            statefulBuff.TimeLeft = time;
            // Add the buff to the npc
            npc.GetGlobalNPC<StatefulBuffNPC>().Buffs.Add(statefulBuff);
            return statefulBuff;
        }
        public static T AddStatefulBuff<T>(this Player player, int time) where T : StatefulBuff, new() {
            var buffs = player.GetModPlayer<StatefulBuffPlayer>().Buffs;
            // Determine if buff should be refreshed or recreated.
            if (buffs.FindIndex(buff => buff.GetType() == typeof(T)) is int index && index > -1)
            {
                // Replace buff
                T thisBuff = buffs[index] as T;
                // Reapply buff and extend buff time if requested.
                if (thisBuff.OnReapply(player) && thisBuff.TimeLeft < time)
                {
                    thisBuff.TimeLeft = time;
                }
                // Return the buff
                return thisBuff;
            }
            // Otherwise create a new buff
            T statefulBuff = new();
            // Create buff
            statefulBuff.OnCreate();
            statefulBuff.TimeLeft = time;
            // Add the buff to the player
            player.GetModPlayer<StatefulBuffPlayer>().Buffs.Add(statefulBuff);
            return statefulBuff;
        }
    }
    public class StatefulBuffNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public List<StatefulBuff> Buffs { get; set; } = new();
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                foreach (var state in Buffs[i].StateChanges)
                {
                    if (state.Item1())
                    {
                        state.Item2();
                    }
                }
                int newDamage = damage;
                Buffs[i].Update(npc, ref newDamage);
                if (newDamage > damage)
                {
                    damage = newDamage;
                }
                Buffs[i].TimeLeft--;
                if (Buffs[i].TimeLeft <= 0)
                {
                    Buffs[i].OnRemove(npc);
                    Buffs.RemoveAt(i);
                }
            }
        }
        public override void PostAI(NPC npc) {
            base.PostAI(npc);
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                Buffs[i].PostUpdate(npc);
            }
        }
    }
    public class StatefulBuffPlayer : ModPlayer
    {
        public List<StatefulBuff> Buffs { get; set; } = new();
        public override void UpdateBadLifeRegen() {
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                Buffs[i].Update(Player);
                Buffs[i].TimeLeft--;
                if (Buffs[i].TimeLeft <= 0)
                {
                    Buffs[i].OnRemove(Player);
                    Buffs.RemoveAt(i);
                }
            }
        }
    }
}
