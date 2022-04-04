using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCruiser : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Player Cruiser was sunk!");
    }
}
