using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarrier : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Enemy Carrier was sunk!");
    }
}
