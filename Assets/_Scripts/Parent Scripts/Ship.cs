using System;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    private int _hitCounter;
    
    public bool IsShipHit { get; set; }

    [SerializeField] private int shipSize;
    public int ShipSize => shipSize;

    [SerializeField] private ShipType shipType;
    public ShipType ShipType => shipType;

    [SerializeField] private ShipOwner shipOwner;
    public ShipOwner ShipOwner => shipOwner;

    protected abstract void SinkShip();
    
    public void MarkShipAsHit()
    {
        if (_hitCounter + 1 > shipSize) return;

        _hitCounter++;

        // If the number of hits equals the total size of the ship, it will sink.
        if (_hitCounter >= shipSize)
        {
            SinkShip();
        }
    }
}
