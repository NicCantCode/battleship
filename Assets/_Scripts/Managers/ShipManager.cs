using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField] private Transform shipParent;
    [SerializeField] private List<GameObject> shipList = new List<GameObject>();
    
    private readonly List<Ship> _spawnedShips = new List<Ship>();
    private GameManager _gameManager;
    private BoardManager _boardManager;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        _boardManager = GameObject.FindGameObjectWithTag("Board Manager").GetComponent<BoardManager>();
    }

    public void SpawnPlayerShips()
    {
        SpawnShips(shipParent);
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
        _gameManager.SetDragEnabled(false);

        foreach (var cell in _boardManager.GetPlayerBoardGrid())
        {
            cell.GetComponent<Collider2D>().enabled = false;
        }

    }

    private void SpawnShips(Transform parent)
    {
        var offset = new Vector3(0, 0, 0);
        var thisPosition = parent.transform.position;
        
        foreach (var ship in shipList)
        {
            var spawnedShip = Instantiate(ship, new Vector3(thisPosition.x + offset.x, thisPosition.y + offset.y, -0.5f), Quaternion.identity, parent);
            _spawnedShips.Add(spawnedShip.GetComponentInChildren<Ship>());
            offset.y += 0.5f;
        }
    }
}
