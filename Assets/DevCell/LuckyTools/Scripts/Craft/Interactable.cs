using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private GameObject playerInteract;
    private bool isUsing = false;

    public bool IsUsing { get => isUsing; set => isUsing = value; }
    public GameObject lastInteractPlayer { get => playerInteract; set => playerInteract = value; }

    public void SetInteractableObject(GameObject playerObj)
    {
        lastInteractPlayer = playerObj;
    }

    public void ClearInteractableObject(GameObject playerObj, string player)
    {
        isUsing = false;
        if (lastInteractPlayer && lastInteractPlayer.name == player)
        {
            lastInteractPlayer = null;
        }
    }

    public bool PutItemInObject(GameObject itemInHands)
    {
        if (gameObject.CompareTag("Bench") && gameObject.transform.childCount < 6)
        {
            itemInHands.transform.SetParent(gameObject.transform);
            return true;
        } else if (gameObject.CompareTag("Furnace"))
        {
            int itemInHandsNum = int.Parse(itemInHands.name.Replace("Item", "").Replace("(Clone)", ""));
            if (CanPutItemInFurnace(itemInHandsNum))
            {
                if (CraftRecipes.Resource.Contains(itemInHandsNum))
                {
                    itemInHands.transform.SetParent(gameObject.transform.GetChild(1));
                    return true;
                }
                else if (CraftRecipes.Meltable.Contains(itemInHandsNum))
                {
                    itemInHands.transform.SetParent(gameObject.transform.GetChild(2));
                    return true;
                }
            }
            
        }
        return false;
    }

    public bool CanPutItemInFurnace(int itemInHandsNum)
    {
        if (CraftRecipes.Resource.Contains(itemInHandsNum) && transform.GetChild(1).childCount == 0 ||
            CraftRecipes.Meltable.Contains(itemInHandsNum) && transform.GetChild(2).childCount == 0)
        {
            return true;
        }
        return false;
    }
}
