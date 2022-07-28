using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public void Use()
    {
        if (gameObject.GetComponent<Item>().ItemModel.Description.itemClass == "Lockpick")
        {
            if (Random.Range(0,100) > 50)
            {
                Destroy(gameObject);
            }
            return;
        }
        Destroy(gameObject);
    }
}
