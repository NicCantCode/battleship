using System;
using TMPro;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hitCounter;
    
    private int _hitCounter;

    private void Start()
    {
        hitCounter.text = "";
        
        for (int i = 0; i < shipSize; i++)
        {
            hitCounter.text += "O";
        }
    }

    public void MarkShipAsHit()
    {
        if (_hitCounter + 1 > shipSize) return;

        System.Text.StringBuilder sb = new System.Text.StringBuilder(hitCounter.text);
        sb[_hitCounter] = 'X';
        hitCounter.text = sb.ToString();
        
        _hitCounter++;

        // If the number of hits equals the total size of the ship, it will sink.
        if (_hitCounter >= shipSize)
        {
            SinkShip();
        }
    }

    protected abstract void SinkShip();
    
    public bool IsShipHit { get; set; }

    [SerializeField] private int shipSize;
    public int ShipSize => shipSize;

    [SerializeField] private ShipType shipType;
    public ShipType ShipType => shipType;

    [SerializeField] private ShipOwner shipOwner;
    public ShipOwner ShipOwner => shipOwner;
}
