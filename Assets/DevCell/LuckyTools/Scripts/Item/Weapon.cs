using GameCore.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int dir = -1;
    private int durablitiy;
    private float lastTimeInteract;
    private float timeInteract = 1f;
    private HashSet<GameObject> collideObjects;
    private GameObject hit;

    public int Durablitiy { get => durablitiy; set => durablitiy = value; }
    public GameObject Hit { get => hit; set => hit = value; }

    private void Start()
    {
        Durablitiy = gameObject.GetComponent<Item>().ItemModel.Description.durability;
        collideObjects = new HashSet<GameObject>();
    }

    private void Update()
    {
        if (lastTimeInteract != 0 && Time.time > lastTimeInteract + timeInteract)
        {
            collideObjects = new HashSet<GameObject>();
            lastTimeInteract = 0;
        }
        if (Durablitiy == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (itemModel.Description.InteractObjects.Contains(other.gameObject.name))
        if ((other.gameObject.CompareTag("Tree") == true ||
            other.gameObject.CompareTag("Ground") == true) && other.gameObject.name != "BlockGrass(Clone)")
        {
            lastTimeInteract = Time.time;
            collideObjects.Add(other.gameObject);
        }
    }

    public void DecreaseDurability(int d)
    {
        Durablitiy -= d;
    }

    private void OnTriggerExit(Collider other)
    {
        //if (ItemModel.Description.InteractObjects.Contains(other.gameObject.name))
        if ((other.gameObject.CompareTag("Tree") == true ||
            other.gameObject.CompareTag("Ground") == true) && other.gameObject.name != "BlockGrass(Clone)")
        {
            lastTimeInteract = 0;
            collideObjects.Remove(other.gameObject);
        }
    }

    public void ObtainItem()
    {
        if (collideObjects != null)
        {
            foreach (var collideObject in collideObjects)
            {
                if (collideObject)
                {
                    
                    if (gameObject.name == "Item128(Clone)")
                    {
                        if (collideObject.CompareTag("Tree"))
                        {
                            DecreaseDurability(1);
                        }
                    }
                    else if (gameObject.name == "Item145(Clone)")
                    {
                        if (collideObject.name == "BlockStone(Clone)" || collideObject.name == "BlockStone")
                        {
                            DecreaseDurability(1);
                        }
                        else if (collideObject.name == "BlockOreIron(Clone)" || collideObject.name == "BlockOreIron")
                        {
                            DecreaseDurability(1);
                        }
                    }
                }
            }
        }
    }

    public void MakeHit()
    {
        hit = Instantiate(GameManager.HitPrefabStatic, gameObject.GetComponent<Pickable>().Owner.transform);
        hit.GetComponent<Hit>().Weapon = gameObject;
    }
}
