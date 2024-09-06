using deeprockitems.Content.Items.Weapons;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace deeprockitems
{
    public class Recipes : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup AnyCopper = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBar)}", ItemID.CopperBar, ItemID.TinBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.CopperBar), AnyCopper);
            RecipeGroup AnyGold = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldBar)}", ItemID.GoldBar, ItemID.PlatinumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldBar), AnyGold);
            RecipeGroup AnyEvilBar = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.DemoniteBar)}", ItemID.DemoniteBar, ItemID.CrimtaneBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.DemoniteBar), AnyEvilBar);
            RecipeGroup AnyPowder = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.VilePowder)}", ItemID.VilePowder, ItemID.ViciousPowder);
            RecipeGroup.RegisterGroup(nameof(ItemID.VilePowder), AnyPowder);
            RecipeGroup AnyCobaltBar = new(() => $"{Language.GetTextValue("LegacyMisc")} {Lang.GetItemNameValue(ItemID.CobaltBar)}", ItemID.CobaltBar, ItemID.PalladiumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.CobaltBar), AnyCobaltBar);
            RecipeGroup AnyAdamantiteBar = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.AdamantiteBar)}", ItemID.AdamantiteBar, ItemID.TitaniumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.AdamantiteBar), AnyAdamantiteBar);
        }
        public override void PostAddRecipes()
        {
            var weaponRecipes = from recipe in Main.recipe
                                where recipe.createItem.ModItem is UpgradeableItemTemplate
                                select recipe;
            // Sort weapons
            foreach (var recipe in weaponRecipes)
            {
                // Put each recipe after luminite bar
                recipe.SortAfterFirstRecipesOf(ItemID.LunarBar);
            }
        }
    }
}