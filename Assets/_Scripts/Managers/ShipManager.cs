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
        
        foreach (var ship in shipList)
        {
            Instantiate(ship, transform.position + offset, Quaternion.identity, transform);
            offset.y += 0.5f;
        }
    }
}
