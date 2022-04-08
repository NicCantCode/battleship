using System;
using UnityEngine;

public class PlayerCarrier : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Player Battleship was sunk!");
    }
}
