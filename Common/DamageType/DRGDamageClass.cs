using deeprockitems.Content.Items.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Common.DamageType
{
    public class DRGDamageClass : DamageClass
    {
        // These items will not benefit from any other modifiers or any damage variation
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) {
            return new StatInheritanceData(0f, 0f, 0f, 0f, 0f);
        }
        public override bool GetEffectInheritance(DamageClass damageClass) {
            return false;
        }
        public override bool UseStandardCritCalcs => false;
    }
}
