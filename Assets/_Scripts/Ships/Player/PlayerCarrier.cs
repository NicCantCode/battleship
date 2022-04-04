using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCarrier : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Player Carrier was sunk!");
    }
}
