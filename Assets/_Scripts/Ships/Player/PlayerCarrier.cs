using UnityEngine;

public class PlayerCarrier : Ship
{
    private readonly Vector3 _direction = new Vector3(0,0,1);


    private void FixedUpdate()
    {
        PrintCellNamesOnOverlap();
    }

    protected override void SinkShip()
    {
        Debug.Log("Player Battleship was sunk!");
    }
}
