using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyer : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Enemy Destroyer was sunk!");
    }
}
