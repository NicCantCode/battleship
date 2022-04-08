using UnityEngine;

public class PlayerSubmarine : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Player Battleship was sunk!");
    }
}
