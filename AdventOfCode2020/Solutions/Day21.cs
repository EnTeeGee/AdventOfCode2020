using AdventOfCode2020.Common;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day21
    {
        [Solution(21, 1)]
        public int Solution1(string input)
        {
            var foods = Parser.ToArrayOf(input, it => new Food(it));
            var allergenMap = GetAllergenMap(foods);

            return foods.SelectMany(it => it.Ingredients).Where(it => !allergenMap.ContainsKey(it)).Count();
        }

        [Solution(21, 2)]
        public string Solution2(string input)
        {
            var foods = Parser.ToArrayOf(input, it => new Food(it));
            var allergenMap = GetAllergenMap(foods);

            return string.Join(",", allergenMap.OrderBy(it => it.Value).Select(it => it.Key).ToArray());
        }

        private Dictionary<string, string> GetAllergenMap(Food[] foods)
        {
            var remainingAllergens = foods.SelectMany(it => it.Allergens).Distinct().ToArray();
            var allergenMap = new Dictionary<string, string>();

            while (remainingAllergens.Any())
            {
                foreach (var item in remainingAllergens)
                {
                    var foodsWith = foods.Where(it => it.Allergens.Contains(item));
                    var ingredientsToTry = foodsWith.SelectMany(it => it.Ingredients).Distinct().Where(it => !allergenMap.ContainsKey(it)).ToArray();
                    string match = null;
                    foreach (var ingredient in ingredientsToTry)
                    {
                        if (foodsWith.All(it => it.Ingredients.Contains(ingredient)))
                        {
                            if (match == null)
                                match = ingredient;
                            else
                            {
                                match = null;
                                break;
                            }
                        }
                    }

                    if (match != null)
                    {
                        allergenMap.Add(match, item);
                    }
                }

                remainingAllergens = remainingAllergens.Where(it => !allergenMap.ContainsValue(it)).ToArray();
            }

            return allergenMap;
        }

        private class Food
        {
            public string[] Ingredients { get; }
            public string[] Allergens { get; }

            public Food(string input)
            {
                var sections = Parser.Split(input, " (contains ");
                Ingredients = Parser.Split(sections[0], " ");
                Allergens = Parser.Split(sections[1], ", ", ")");
            }
        }
    }
}
