using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deeprockitems.Content.Upgrades
{
    public class RecipeBinding
    {
        public enum RecipeType
        {
            Unset = 0,
            SpecificTypeOnly = 1,
            MultipleAcceptedTypes = 2,
        }
        public RecipeType Type = RecipeType.Unset;
        public RecipeBinding(int type, int stack) {
            AcceptedTypes = [type];
            Stack = stack;
        }
        public RecipeBinding(int[] types, int stack) {
            AcceptedTypes = types;
            Stack = stack;
        }
        public int[] AcceptedTypes;
        public int Stack;
    }
}
