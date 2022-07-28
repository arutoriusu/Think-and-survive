using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
