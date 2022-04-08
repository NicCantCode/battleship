using UnityEngine;

public class PlayerBattleship : Ship
{
    protected override void SinkShip()
    {
        Debug.Log("Player Battleship was sunk!");
    }
}
