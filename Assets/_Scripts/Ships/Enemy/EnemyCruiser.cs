using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCruiser : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Enemy Cruiser was sunk!");
    }
}
