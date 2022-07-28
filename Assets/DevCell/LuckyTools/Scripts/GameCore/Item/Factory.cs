using GameCore.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Factory
{
    private Dictionary<int, ItemModel> itemFactory;

    public Factory()
    {
        itemFactory = new Dictionary<int, ItemModel>();
    }

    public void Init(ItemDescriptions descriptions)
    {
        CreateItemModels(descriptions);
    }

    private void CreateItemModels(ItemDescriptions descriptions)
    {
        for (int i = 0; i < 256; i++)
        {
            int j = 0;
            while (j != descriptions.ListItems.Count)
            {
                ItemDescription itemDesc = descriptions.ListItems[j];
                if (itemDesc.id == i)
                {
                    itemFactory[i] = new ItemModel(itemDesc);
                    break;
                }
                j++;
            };
        }
    }

    public ItemModel GetItemModel(int itemNum)
    {
        try
        {
            return itemFactory[itemNum];
        }
        catch
        {
            return null;
        }
        
    }

}
