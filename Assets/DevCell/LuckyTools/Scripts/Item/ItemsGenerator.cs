using GameCore.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class ItemsGenerator : MonoBehaviour
{
    [SerializeField] GameObject itemsObjects;
    [SerializeField] GameObject itemsIconsParent;
    [SerializeField] Texture2D itemsAtlas;
    [SerializeField] ItemDescriptions _itemDescriptions;

    private static GameObject[] itemsIcons;
    private static GameObject[] items;
    private static Factory _factory;

    public static GameObject[] Items { get => items; set => items = value; }
    public static GameObject[] ItemsIcons { get => itemsIcons; set => itemsIcons = value; }
    public static Factory Factory { get => _factory; set => _factory = value; }

    public ItemsGenerator()
    {
        ItemsIcons = new GameObject[256];
        items = new GameObject[256];
    }

    void Start()
    {
        Factory = new Factory();
        Factory.Init(_itemDescriptions);
        GenerateItems();
    }

    private void GenerateItems()
    {
        int numOfItem = 0;
        int rowItem = 0;
        int columnItem = 0;
        Color[][][] items = new Color[256][][];

        for (int y = 0; y < itemsAtlas.height; y += 16)
        {
            for (int x = 0; x < itemsAtlas.width; x += 16)
            {
                Texture2D itemTexture = new Texture2D(16, 16);

                var itemIcon = new GameObject("ItemIcon" + numOfItem);
                Sprite sprite = Sprite.Create(itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), Vector2.zero);
                itemIcon.AddComponent<SpriteRenderer>().sprite = sprite;
                rowItem = 0;
                items[numOfItem] = new Color[16][];

                GameObject currentItem = new GameObject("Item" + numOfItem);
                for (int localY = y; localY < y + 16; localY++)
                {
                    columnItem = 0;
                    items[numOfItem][rowItem] = new Color[16];
                    for (int localX = x; localX < x + 16; localX++)
                    {
                        Color pixelColor = itemsAtlas.GetPixel(localX, localY);
                        itemTexture.SetPixel(localX, localY, pixelColor);
                        items[numOfItem][rowItem][columnItem] = pixelColor;
                        var cube = GenerateBlock(localX - x, localY - y, pixelColor, currentItem);
                        if (cube)
                        {
                            cube.transform.parent = currentItem.transform;
                        }
                        columnItem += 1;
                    }
                }
                Items[numOfItem] = currentItem;
                currentItem.transform.parent = itemsObjects.transform;
                
                itemTexture.Apply();
                itemIcon.transform.parent = itemsIconsParent.transform;

                ItemsIcons[numOfItem] = itemIcon;
                itemIcon.SetActive(false);
                currentItem.SetActive(false);

                numOfItem += 1;
                rowItem += 1;
            }
        }
    }

    private GameObject GenerateBlock(int x, int y, Color pixelColor, GameObject currentItem)
    {
        if (pixelColor.a == 0)
        {
            return null;
        }

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(cube.GetComponent<BoxCollider>());
        cube.GetComponent<Renderer>().material.color = pixelColor;
        cube.transform.position = new Vector3(x, y, 0);
        cube.transform.parent = currentItem.transform;
        return cube;

    }
}
