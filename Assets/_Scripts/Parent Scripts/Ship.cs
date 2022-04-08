using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    private readonly Vector3 _direction = new Vector3(0,0,1);
    
    private int _hitCounter;
    private bool _isAboveCells;
    
    public bool IsShipHit { get; set; }
    
    private Queue<Cell> _occupiedCells;
    public Queue<Cell> OccupiedCells => _occupiedCells;

    [SerializeField] private bool _isPlaced;
    public bool IsPlaced => _isPlaced;

    [SerializeField] private int shipSize;
    public int ShipSize => shipSize;
    
    [SerializeField] private ShipType shipType;
    public ShipType ShipType => shipType;

    [SerializeField] private ShipOwner shipOwner;
    public ShipOwner ShipOwner => shipOwner;

    protected abstract void SinkShip();

    private void Start()
    {
        _occupiedCells = new Queue<Cell>(shipSize);
        _isPlaced = false;
    }

    public void SetIsPlaced(bool isPlaced)
    {
        _isPlaced = isPlaced;
    }

    private void SendRaycasts(float thisRotation, Transform thisTransform, Vector3 thisPosition)
    {
        var origin = new Vector3();
        var offset = (ShipSize - 1) * 0.25f;

        switch (thisRotation)
        {
            case 90:
            case 270:
                origin = new Vector3(thisPosition.x, thisPosition.y - offset, thisPosition.z);
                break;
            case 0:
            case 180:
                origin = new Vector3(thisPosition.x - offset, thisPosition.y, thisPosition.z);
                break;
        }
        
        for (var i = 0; i < ShipSize; i++)
        {
            var hit = Physics2D.Raycast(origin, _direction, 1);

            if (hit.collider != null)
            {
                _isAboveCells = true;
                if (_occupiedCells.Count == shipSize) _occupiedCells.Dequeue();
                _occupiedCells.Enqueue(hit.transform.gameObject.GetComponent<Cell>());
            }

            if (hit.collider == null)
            {
                _isAboveCells = false;
            }
            
            switch (thisRotation)
            {
                case 90:
                case 270:
                    origin.y += 0.5f;
                    break;
                case 0:
                case 180:
                    origin.x += 0.5f;
                    break;
            }
        }
    }
    
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

    public bool IsValidShipPosition(Queue<Cell> occupiedCellsToCheck)
    {
        var thisTransform = transform;
        var thisPosition = thisTransform.position;
        
        SendRaycasts(thisTransform.rotation.eulerAngles.z, thisTransform, thisPosition);
        
        return _isAboveCells && occupiedCellsToCheck.Count == occupiedCellsToCheck.Distinct().Count() && occupiedCellsToCheck.Count != 0;
    }
}
