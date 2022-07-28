using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWeapon : MonoBehaviour
{
    public So[] soList;
    public GameObject weapon;

    private void Start()
    {
        var weaponstats = weapon.GetComponent<so_weapon>();

        for (int i = 0; i < soList.Length; i++)
        {
            weaponstats.id = soList[i].id;
            weaponstats.name = soList[i].name;
            weaponstats.damage = soList[i].damage;
            Instantiate(weapon);
            //Instantiate(model, weapon.transform);
        }
        
    }
}
