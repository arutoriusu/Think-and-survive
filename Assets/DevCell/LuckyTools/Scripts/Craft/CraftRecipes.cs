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
        recipesBench.Add("197 232", GetResultRecipe(213, 1)); // ����� + ���� = ������
        recipesBench.Add("197 248 197 248", GetResultRecipe(229, 1)); // �����+�������+�����+�������=���
        recipesBench.Add("197 246", GetResultRecipe(177, 1)); // ����� + ������� = �������� ���
        recipesBench.Add("246 197 246", GetResultRecipe(145, 1)); // ������� + ����� + ������� = �������� �����
        recipesBench.Add("197 197 197", GetResultRecipe(100, 1)); // ����� + ����� + ����� = �����
        recipesBench.Add("246 246 197", GetResultRecipe(129, 1)); // ������� + ������� + ����� = �������� �����
        recipesBench.Add("100 197", GetResultRecipe(160, 1)); // ����� + ����� = ���������� ������
        recipesBench.Add("96", GetResultRecipe(100, 3)); ; // ������ = 3 �����
        recipesBench.Add("231 246", GetResultRecipe(202, 1)); // �������� ������ + ������� = ���� ������
        recipesBench.Add("202 197", GetResultRecipe(161, 1)); // ���� ������ + ����� = �������� ������
        recipesBench.Add("197 202", GetResultRecipe(178, 1)); // ����� + ���� ������ = �������� ���
        recipesBench.Add("231 197 231", GetResultRecipe(146, 1)); // �������� ������ + ����� + �������� ������ = �������� �����
        recipesBench.Add("202 231 197", GetResultRecipe(130, 1)); // ���� ������ + �������� ������ + ����� = �������� �����
        recipesBench.Add("100 100 100 100", GetResultRecipe(219, 1)); // 4 ����� = ���������� �����
        recipesBench.Add("202 202 202 202", GetResultRecipe(220, 1)); // 4 ����� ������ = �������� �����
        recipesBench.Add("197 96 197", GetResultRecipe(80, 1)); // ����� + ������ + ����� = ������
        recipesBench.Add("197 247 247 197", GetResultRecipe(80, 1)); //����� + ����� + ����� + ����� = ������
        recipesBench.Add("197 238 247", GetResultRecipe(81, 1)); // ����� + ����� + ����� = �����
        recipesBench.Add("253 26", GetResultRecipe(95, 1)); // ����+����=�����
        recipesBench.Add("246 202", GetResultRecipe(89, 3)); // ������� + ���� ������ = 3 ������
        recipesBench.Add("89 100 100 100", GetResultRecipe(82, 1)); // ������+�����+�����+�����=������
        recipesBench.Add("236 246", GetResultRecipe(79, 1)); // ����� + ������� = ������� ����
        recipesBench.Add("233", GetResultRecipe(253, 1)); // ������� = ����
        recipesBench.Add("202 236", GetResultRecipe(68, 1)); //���� ������ + ����� = �������� ������
        recipesBench.Add("246 248 197 197", GetResultRecipe(48,1)); //������� + ������� + ����� + ����� = �������� ����� (����� ����, ����� ������ ��� ������� ���� � ��������� �����)
        recipesBench.Add("68 68 89", GetResultRecipe(49, 1)); // �������� ������ + �������� ������ + ������ = �������� ����� (����� ����, ����� ������ ��� ������� ���� � ��������� �����)
        recipesBench.Add("246 68", GetResultRecipe(69, 1)); // ������� + �������� ������ = �������

        // �����
        recipesFurnace.Add("97", GetResultRecipe(231, 1)); // �������� ���� = �������� ������
        recipesFurnace.Add("95", GetResultRecipe(217, 1)); //����� � �����=����

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
