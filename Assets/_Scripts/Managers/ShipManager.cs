using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public List<GameObject> shipList = new List<GameObject>();
    private List<Ship> _spawnedShips = new List<Ship>();

    private void Awake()
    {
        SpawnShips();
    }
    
    public void SetFinishedPlacingShips()
    {
        var allShipsPlaced = false;
        foreach (var ship in _spawnedShips)
        {
            if (!ship.IsPlaced)
            {
                allShipsPlaced = false;
                continue;
            }

            allShipsPlaced = true;
        }

        if (!allShipsPlaced) return;
        foreach (var ship in _spawnedShips)
        {
            ship.transform.GetComponentInChildren<DragAndSnap>().ToggleDrag();
        }

    }

    private void SpawnShips()
    {
        var offset = new Vector3(0, 0, 0);
        
        var thisTransform = transform;
        var thisPosition = thisTransform.position;
        
        foreach (var ship in shipList)
        {
            var spawnedShip = Instantiate(ship, new Vector3(thisPosition.x + offset.x, thisPosition.y + offset.y, -0.5f), Quaternion.identity, thisTransform);
            _spawnedShips.Add(spawnedShip.GetComponentInChildren<Ship>());
            offset.y += 0.5f;
        }
    }
}
