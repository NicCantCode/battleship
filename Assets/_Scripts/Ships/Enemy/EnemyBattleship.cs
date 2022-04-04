using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleship : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Enemy Battleship was sunk!");
    }
}
