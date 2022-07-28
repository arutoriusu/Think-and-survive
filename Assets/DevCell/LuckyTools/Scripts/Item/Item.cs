using GameCore.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private ItemModel itemModel;

    public ItemModel ItemModel { get => itemModel; set => itemModel = value; }
}
