using UnityEngine;

public class PlayerDestroyer : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Player Battleship was sunk!");
    }
}
