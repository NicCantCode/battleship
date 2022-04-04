using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroyer : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Player Destroyer was sunk!");
    }
}
