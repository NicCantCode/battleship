using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySubmarine : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Enemy Submarine was sunk!");
    }
}
