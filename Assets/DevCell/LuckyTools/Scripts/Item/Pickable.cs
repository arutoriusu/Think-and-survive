using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private GameObject owner;

    public GameObject Owner { get => owner; set => owner = value; }

    private void Start()
    {

        gameObject.AddComponent<BoxCollider>().isTrigger = true;
        gameObject.GetComponent<BoxCollider>().size = new Vector3(8, 12, 8);
        gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 6, 0);
        gameObject.AddComponent<Rigidbody>().useGravity = false;

        gameObject.GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezePositionX;
        gameObject.GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezePositionZ;
        gameObject.GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezeRotationX;
        gameObject.GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameObject != other.gameObject.GetComponent<Player>().ItemInHands && !Owner)
            {
                other.gameObject.GetComponent<Player>().ItemNear = gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().ItemNear = null;
        }
    }
}
