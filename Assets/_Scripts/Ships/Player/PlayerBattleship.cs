using UnityEngine;

public class PlayerBattleship : Ship
{


    private void FixedUpdate()
    {
        PrintCellNamesOnOverlap();
    }

    protected override void SinkShip()
    {
        Debug.Log("Player Battleship was sunk!");
    }

    
}
