using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayersData
{
    public PlayerData player1;
    public PlayerData player2;

    public PlayersData(PlayerData pd1, PlayerData pd2)
    {
        player1 = pd1;
        player2 = pd2;
    }
}
