using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    private GameObject weapon;

    public GameObject Weapon { get => weapon; set => weapon = value; }

    private void Start()
    {
        Destroy(gameObject, GLOBAL.HIT_DURATION);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ground>())
        {
            other.GetComponent<Ground>().Hits += 1;
        }
        //collideObject.GetComponent<RegularTree>().HitsForLogSpawn += 1;
    }
}
