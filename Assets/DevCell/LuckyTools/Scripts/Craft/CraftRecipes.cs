using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftRecipes
{
    public struct ResultAndCount
    {
        public int result;
        public int count;
    }

    private static Dictionary<string, ResultAndCount> recipesBench = new Dictionary<string, ResultAndCount>();
    private static Dictionary<string, ResultAndCount> recipesFurnace = new Dictionary<string, ResultAndCount>();
    private static HashSet<int> resource = new HashSet<int>();
    private static HashSet<int> meltable = new HashSet<int>();

    public static HashSet<int> Resource { get => resource; set => resource = value; }
    public static HashSet<int> Meltable { get => meltable; set => meltable = value; }

    static CraftRecipes()
    {
        recipesBench.Add("197 232", GetResultRecipe(213, 1)); // палка + перо = стрела
        recipesBench.Add("197 248 197 248", GetResultRecipe(229, 1)); // Палка+Паутина+Палка+Паутина=Лук
        recipesBench.Add("197 246", GetResultRecipe(177, 1)); // палка + кремень = каменный меч
        recipesBench.Add("246 197 246", GetResultRecipe(145, 1)); // кремень + палка + кремень = каменная кирка
        recipesBench.Add("197 197 197", GetResultRecipe(100, 1)); // палка + палка + палка = доска
        recipesBench.Add("246 246 197", GetResultRecipe(129, 1)); // кремень + кремень + палка = каменный топор
        recipesBench.Add("100 197", GetResultRecipe(160, 1)); // доска + палка = деревянная лопата
        recipesBench.Add("96", GetResultRecipe(100, 3)); ; // бревно = 3 доски
        recipesBench.Add("231 246", GetResultRecipe(202, 1)); // железный слиток + кремень = лист железа
        recipesBench.Add("202 197", GetResultRecipe(161, 1)); // лист железа + палка = железная лопата
        recipesBench.Add("197 202", GetResultRecipe(178, 1)); // палка + лист железа = железный меч
        recipesBench.Add("231 197 231", GetResultRecipe(146, 1)); // железный слиток + палка + железный слиток = железная кирка
        recipesBench.Add("202 231 197", GetResultRecipe(130, 1)); // лист железа + железный слиток + палка = железный топор
        recipesBench.Add("100 100 100 100", GetResultRecipe(219, 1)); // 4 доски = деревянная дверь
        recipesBench.Add("202 202 202 202", GetResultRecipe(220, 1)); // 4 листа железа = железная дверь
        recipesBench.Add("197 96 197", GetResultRecipe(80, 1)); // палка + бревно + палка = костер
        recipesBench.Add("197 247 247 197", GetResultRecipe(80, 1)); //Палка + Уголь + Угорь + Палка = Костер
        recipesBench.Add("197 238 247", GetResultRecipe(81, 1)); // палка + слизь + уголь = факел
        recipesBench.Add("253 26", GetResultRecipe(95, 1)); // Мука+Яйцо=Тесто
        recipesBench.Add("246 202", GetResultRecipe(89, 3)); // кремень + лист железа = 3 крепеж
        recipesBench.Add("89 100 100 100", GetResultRecipe(82, 1)); // Крепеж+Доска+Доска+Доска=Сундук
        recipesBench.Add("236 246", GetResultRecipe(79, 1)); // Кость + Кремень = Костная мука
        recipesBench.Add("233", GetResultRecipe(253, 1)); // пшеница = мука
        recipesBench.Add("202 236", GetResultRecipe(68, 1)); //Лист железа + Кость = Железная трубка
        recipesBench.Add("246 248 197 197", GetResultRecipe(48,1)); //Кремень + Паутина + Палка + Палка = Каменное копье (можно бить, можно кидать при зажатии вниз и подбирать потом)
        recipesBench.Add("68 68 89", GetResultRecipe(49, 1)); // Железная трубка + Железная трубка + Крепеж = Железное копье (можно бить, можно кидать при зажатии вниз и подбирать потом)
        recipesBench.Add("246 68", GetResultRecipe(69, 1)); // Кремень + Железная трубка = Отмычка

        // печка
        recipesFurnace.Add("97", GetResultRecipe(231, 1)); // железная руда = железный слиток
        recipesFurnace.Add("95", GetResultRecipe(217, 1)); //Тесто в печку=Хлеб

        Resource.Add(97);
        Resource.Add(95);

        Meltable.Add(197);
        Meltable.Add(247);
        //
    }

    public static ResultAndCount FindFurnaceRecipe(string items)
    {
        ResultAndCount rac;
        rac.result = -1;
        rac.count = -1;
        if (recipesFurnace.TryGetValue(items.Replace(" ", ""), out ResultAndCount res))
        {
            return res;
        }
        return rac;
    }

    private static ResultAndCount GetResultRecipe(int result, int count)
    {
        ResultAndCount rac;
        rac.result = result;
        rac.count = count;
        return rac;
    }

    public static ResultAndCount FindBenchRecipe(string key)
    {
        string[] indexes = key.Split(' ').Where(x => !string.IsNullOrEmpty(x) && x != "Result").ToArray();

        ResultAndCount rac;
        rac.result = -1;
        rac.count = -1;

        string resultKeys = string.Join(" ", indexes);
        
        if (recipesBench.TryGetValue(resultKeys, out ResultAndCount res))
        {
            return res;
        }
        return rac;
    }
}
