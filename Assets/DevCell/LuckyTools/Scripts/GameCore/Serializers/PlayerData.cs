using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int number;
    public string playerName;
    public int numItemInHands;
    public Vector3 position;
    public int countLifes = 5;
    public int health;
    public int satiety;
    public int curDir;
}
