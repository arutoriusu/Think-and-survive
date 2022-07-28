using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMechanism : MonoBehaviour
{
    public GameObject[] GetAllChilds()
    {
        GameObject[] childs = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            if (gameObject.CompareTag("Bench"))
            {
                childs[i] = transform.GetChild(i).gameObject;
            }
            else if (gameObject.CompareTag("Furnace"))
            {
                if (transform.GetChild(i).childCount > 0)
                {
                    childs[i] = transform.GetChild(i).GetChild(0).gameObject;
                }
            }
        }
        return childs;
    }

    public bool TryUseRecipe()
    {
        string craftItems = "";

        foreach (var i in GetAllChilds())
        {
            if (!i)
            {
                continue;
            }
            string index = "";
            for (int j = 0; j < i.name.Length; j++)
            {
                if (char.IsDigit(i.name[j]))
                    index += i.name[j];
            }
            craftItems += index + " ";
        }

        int resultRecipe = -1;
        int countRecipe = -1;
        if (gameObject.CompareTag("Bench") == true)
        {
            var result = CraftRecipes.FindBenchRecipe(craftItems);
            resultRecipe = result.result;
            countRecipe = result.count;
        }
        else if (gameObject.CompareTag("Furnace") == true)
        {
            var result = CraftRecipes.FindFurnaceRecipe(craftItems);
            resultRecipe = result.result;
            countRecipe = result.count;
        }

        if (resultRecipe != -1)
        {
            DestroyAllChilds(gameObject);
            CreateItem(resultRecipe, countRecipe);
            return true;
        }
        return false;
    }

    public void DestroyAllChilds(GameObject parent)
    {
        if (gameObject.CompareTag("Bench"))
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (parent.transform.GetChild(i).gameObject.name != "Result")
                {
                    Destroy(parent.transform.GetChild(i).gameObject);
                }
            }
        } else if (gameObject.CompareTag("Furnace"))
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (parent.transform.GetChild(i).childCount != 0)
                {
                    Destroy(parent.transform.GetChild(i).GetChild(0).gameObject);
                }
            }
        }
        
    }

    public void CreateItem(int result, int count)
    {
        int countCreate = count;

        while (countCreate != 0)
        {
            var myItem = GameManager.CreateItem(result, new Vector3(
                    gameObject.transform.position.x,
                    gameObject.transform.position.y + 20,
                    gameObject.transform.position.z
                ));
            myItem.transform.parent.parent = gameObject.transform.GetChild(0);
            countCreate -= 1;
        }
    }
}
