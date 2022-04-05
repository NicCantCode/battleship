using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public List<GameObject> shipList = new List<GameObject>();

    private void Start()
    {
        SpawnShips();
    }

    private void SpawnShips()
    {
        var offset = new Vector3(0, 0, 0);
        
        var thisTransform = transform;
        var thisPosition = thisTransform.position;
        
        foreach (var ship in shipList)
        {
            Instantiate(ship, new Vector3(thisPosition.x + offset.x, thisPosition.y + offset.y, -0.5f), Quaternion.identity, thisTransform);
            offset.y += 0.5f;
        }
    }
}
