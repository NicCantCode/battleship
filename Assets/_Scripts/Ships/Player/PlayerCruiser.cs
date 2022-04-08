using UnityEngine;

public class PlayerCruiser : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Player Battleship was sunk!");
    }
}
