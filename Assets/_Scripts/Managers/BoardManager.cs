using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    private const int BoardWidth = 10;
    private const int BoardHeight = 10;
    private const float CellSize = 0.5f;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform playerBoardParent;
    [SerializeField] private Transform targetingBoardParent;
    [SerializeField] private Transform enemyShipParent;

    private List<Cell> _playerBoardGrid = new List<Cell>();
    private List<Cell> _enemyBoardGrid = new List<Cell>();

    private List<Cell> _enemyOccupiedCells = new List<Cell>();
    private List<Ship> _enemyShips = new List<Ship>();
    
    private AIManager _aiManager;

    private void Awake()
    {
        _aiManager = GameObject.FindGameObjectWithTag("AI Manager").GetComponent<AIManager>();
    }

    public void BuildPlayerBoard()
    {
        BuildBoard(BoardWidth, BoardHeight, playerBoardParent, true);
        _aiManager.SetPlayerBoardStack(Utils.ToStack(Utils.Shuffle(new List<Cell>(_playerBoardGrid))));
    }

    public void BuildTargetingBoard()
    {
        BuildBoard(BoardWidth, BoardHeight, targetingBoardParent, false);
        GenerateEnemyShipPlacements();
    }

    private void BuildBoard(int width, int height, Transform parent, bool isPlayerBoard)
    {
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var cell = Instantiate(cellPrefab, new Vector3(i * CellSize, j * CellSize, 0) + parent.transform.position, Quaternion.identity, parent);
                cell.name = "Cell " + "(" + i +"," + j +")";
                var cellComponent = cell.GetComponent<Cell>();
                cellComponent.SetIsInPlayerBoard(isPlayerBoard);
                cellComponent.SetGridLocation(new Vector2(i, j));
                
                if (isPlayerBoard)
                {
                    _playerBoardGrid.Add(cellComponent);
                }
                else
                {
                    _enemyBoardGrid.Add(cellComponent);
                }
            }
        }
    }

    public List<Cell> GetPlayerBoardGrid()
    {
        return _playerBoardGrid;
    }

    private void GenerateEnemyShipPlacements()
    {
        var counter = 0;
        var directionsAsArray = Enum.GetValues(typeof(Direction));
        var shipSizes = new List<int> { 5, 4, 3, 3, 2 };
        
        while (counter < 5)
        {
            var randomX = Random.Range(0, BoardWidth);
            var randomY = Random.Range(0, BoardHeight);
            var randomDirectionIndex = Random.Range(0, directionsAsArray.Length);
            var randomDirection = (Direction) directionsAsArray.GetValue(randomDirectionIndex);

            var validCells = ShipUtils.GetValidCellsFromPosition(_enemyBoardGrid, new Vector2(randomX, randomY), randomDirection, shipSizes);

            if (validCells == null) continue;

            ShipUtils.CreateFauxShip(_enemyShips, enemyShipParent, validCells[0].GetShipType(), new Queue<Cell>(validCells));
            
            _enemyOccupiedCells.AddRange(validCells);
            
            counter++;
        }

        ShipUtils.SetCellOccupiedShip(_enemyOccupiedCells, _enemyShips);
    }
}