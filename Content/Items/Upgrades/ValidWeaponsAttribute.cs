using deeprockitems.Content.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;

namespace deeprockitems.Content.Items.Upgrades
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ValidWeaponsAttribute(params Type[] weapons) : Attribute
    {
        public List<Type> AllowedWeapons { get; set; } = [..weapons];
        public ValidWeaponsAttribute Add<T>() where T : UpgradeableItemTemplate
        {
            AllowedWeapons.Add(typeof(T));
            return this;
        }
    }
}
