using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularTree : MonoBehaviour
{
    private int punchesForStickSpawn = 0;
    private int hitsForLogSpawn = 0;
    private int bias = 5;

    public int PunchesForStickSpawn { get => punchesForStickSpawn; set => punchesForStickSpawn = value; }
    public int HitsForLogSpawn { get => hitsForLogSpawn; set => hitsForLogSpawn = value; }

    private void FixedUpdate()
    {
        if (PunchesForStickSpawn == 10)
        {
            GameManager.CreateItem(197, new Vector3(transform.position.x, transform.position.y + bias, 0));
            punchesForStickSpawn = 0;
            PunchesForStickSpawn = 0;
        }
        if (HitsForLogSpawn == 10)
        {
            GameManager.CreateItem(96, new Vector3(transform.position.x, transform.position.y + bias, 0));
            HitsForLogSpawn = 0;
        }
    }
}
