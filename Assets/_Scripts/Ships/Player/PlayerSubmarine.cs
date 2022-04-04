using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSubmarine : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Player Submarine was sunk!");
    }
}
