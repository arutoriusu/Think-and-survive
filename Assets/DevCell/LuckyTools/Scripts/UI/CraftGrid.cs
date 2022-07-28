using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftGrid : MonoBehaviour
{
    [SerializeField] GameObject[] panelSlots;
    [SerializeField] GameObject[] outlines;
    GameObject objectNear;
    private GameObject player;
    private Slider Readiness;
    private int currentSlot = 0;
    private bool choiceInterfaceOpen;

    public GameObject Player { get => player; set => player = value; }
    public int CurrentSlot { get => currentSlot; set => currentSlot = value; }
    public bool ChoiceInterfaceOpen { get => choiceInterfaceOpen; set => choiceInterfaceOpen = value; }

    public void Start()
    {
        if (TypeCraftMechanism() == 2)
        {
            Readiness = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        }
    }

    public void Update()
    {
        if (TypeCraftMechanism() == 2)
        {
            CheckFurnaceParameters(objectNear);
        }
    }

    public void OnDisable()
    {
        CloseChoiceInterface();
    }

    private int TypeCraftMechanism()
    {
        if (objectNear && objectNear.CompareTag("Bench"))
        {
            return 1;
        }
        if (objectNear && objectNear.CompareTag("Furnace"))
        {
            return 2;
        }
        return 0;
    }

    public void SetAllItemsToCraftGrid()
    {
        //IPlayer iplayer = Player.GetComponent(Type.GetType(Player.name.Replace("(Clone)",""))) as IPlayer;
        if (player.GetComponent<Player>().ObjectNearZMinus.CompareTag("Furnace") ||
                player.GetComponent<Player>().ObjectNearZMinus.CompareTag("Bench"))
        {
            objectNear = player.GetComponent<Player>().ObjectNearZMinus;
        }
        else if (player.GetComponent<Player>().ObjectNearZPlus.CompareTag("Furnace") ||
            player.GetComponent<Player>().ObjectNearZPlus.CompareTag("Bench"))
        {
            objectNear = player.GetComponent<Player>().ObjectNearZPlus;
        }
        if (objectNear.CompareTag("Bench"))
        {
            ItemsFromBench(objectNear);
        } else if (objectNear.CompareTag("Furnace"))
        {
            ItemsFromFurnace(objectNear);
        }
    }

    private void ItemsFromFurnace(GameObject objectNear)
    {
        if (objectNear.transform.GetChild(1).childCount > 0)
        {
            SetItemToSlot(objectNear.transform.GetChild(1).GetChild(0).name, 0);
        }
        if (objectNear.transform.GetChild(2).childCount > 0)
        {
            SetItemToSlot(objectNear.transform.GetChild(2).GetChild(0).name, 1);
        }
    }

    private void ItemsFromBench(GameObject objectNear)
    {
        for (int i = 1; i < objectNear.transform.childCount; i++)
        {
            SetItemToSlot(objectNear.transform.GetChild(i).name, i-1);
        }
    }

    private void CheckFurnaceParameters(GameObject objectNear)
    {
        Readiness.value = objectNear.GetComponent<FurnaceManufacture>().Readiness;
    }

    public void SetItemToSlot(string itemName, int i)
    {
        if (itemName != "Result")
        {
            int index = int.Parse(itemName.Replace("Item", "").Replace("(Clone)", ""));
            var icon = Instantiate(ItemsGenerator.ItemsIcons[index]);
            icon.transform.parent = panelSlots[i].transform;
            icon.transform.localScale = new Vector3(800, 800, 800);
            icon.transform.localPosition = new Vector3(0, 0, 0);
            icon.GetComponent<SpriteRenderer>().sortingOrder = 1;
            icon.SetActive(true);
        }
        
    }

    public void Close()
    {
        objectNear = null;
        CloseChoiceInterface();
        CleanSlots();
        gameObject.SetActive(false);
    }

    public void CleanSlots()
    {
        for (int i = 0; i < panelSlots.Length; i++)
        {
            if (panelSlots[i].transform.childCount != 0)
            {
                Destroy(panelSlots[i].transform.GetChild(0).gameObject);
            }
        }
    }

    public void Open()
    {
        if (!gameObject.activeSelf)
        {
            SetAllItemsToCraftGrid();
        }
        
        gameObject.SetActive(true);
    }

    public void OpenCraftGridChoiceInterface()
    {
        currentSlot = 0;
        ChoiceInterfaceOpen = true;
        outlines[0].SetActive(true);
    }

    public void LeftChoice()
    {
        outlines[CurrentSlot].SetActive(false);
        if (CurrentSlot == 0)
        {
            CurrentSlot = outlines.Count()-1;
        }
        else
        {
            CurrentSlot -= 1;
        }
        outlines[CurrentSlot].SetActive(true);
    }

    public void RightChoice()
    {
        outlines[CurrentSlot].SetActive(false);
        if (CurrentSlot == outlines.Count() - 1)
        {
            CurrentSlot = 0;
        }
        else
        {
            CurrentSlot += 1;
        }
        outlines[CurrentSlot].SetActive(true);
    }

    public void CloseChoiceInterface()
    {
        ChoiceInterfaceOpen = false;
        foreach (var item in outlines)
        {
            item.SetActive(false);
        }
    }
}
