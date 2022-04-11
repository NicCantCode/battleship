using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private readonly Vector3 _direction = new Vector3(0,0,1);
    
    private int _hitCounter;
    private bool _isAboveCells;
    private GameManager _gameManager;
    private LogManager _logManager;

    private Queue<Cell> _occupiedCells;
    public Queue<Cell> OccupiedCells => _occupiedCells;

    private bool _isPlaced;
    public bool IsPlaced => _isPlaced;

    [SerializeField] private int shipSize;
    public int ShipSize => shipSize;
    
    [SerializeField] private ShipType shipType;
    public ShipType ShipType => shipType;

    [SerializeField] private ShipOwner shipOwner;
    public ShipOwner ShipOwner => shipOwner;


    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        _logManager = GameObject.FindGameObjectWithTag("Log Manager").GetComponent<LogManager>();
        _occupiedCells = new Queue<Cell>(shipSize);
        _isPlaced = false;
    }
    
    private void SinkShip()
    {
        switch (ShipOwner)
        {
            case ShipOwner.ENEMY:
                _logManager.LogMessage($"Enemy {shipType.ToString()} was sunk!", new Color(155f / 255f, 0, 0));
                GetGameManagerReference().SubtractFromEnemyShipPool();
                break;
            case ShipOwner.PLAYER:
                _logManager.LogMessage($"Player {shipType.ToString()} was sunk!", new Color(155f / 255f, 0, 0));
                GetGameManagerReference().SubtractFromPlayerShipPool();
                break;
        }
    }

    private GameManager GetGameManagerReference()
    {
        return _gameManager;
    }

    public void SetIsPlaced(bool isPlaced)
    {
        _isPlaced = isPlaced;
    }

    private void SendRaycasts(float thisRotation, Vector3 thisPosition)
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
        var isCellOccupied = false;

        foreach (var potentialCell in occupiedCellsToCheck.ToList())
        {
            foreach (var occupiedCell in _gameManager.GetShipBearingCells())
            {
                if (potentialCell == occupiedCell) isCellOccupied = true;
            }
        }

        SendRaycasts(thisTransform.rotation.eulerAngles.z, thisPosition);
        
        return !isCellOccupied && _isAboveCells && occupiedCellsToCheck.Count == occupiedCellsToCheck.Distinct().Count() && occupiedCellsToCheck.Count != 0;
    }

    public void SetOccupiedCells(Queue<Cell> occupiedCells)
    {
        _occupiedCells = occupiedCells;
    }

    public void SetShipType(ShipType newShipType)
    {
        shipType = newShipType;
    }

    public void SetShipSize(int newShipSize)
    {
        shipSize = newShipSize;
    }

    public void SetShipOwner(ShipOwner newShipOwner)
    {
        shipOwner = newShipOwner;
    }
}
