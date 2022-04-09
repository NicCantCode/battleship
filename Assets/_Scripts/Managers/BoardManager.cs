using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    private enum Direction { NORTH, EAST, SOUTH, WEST }
    
    private const float CellSize = 0.5f;
    
    [SerializeField] private Transform playerBoardParent;
    [SerializeField] private Transform targetingBoardParent;
    [SerializeField] private Transform enemyShipParent;
    private List<Cell> _playerBoardGrid = new List<Cell>();
    private List<Cell> _enemyBoardGrid = new List<Cell>();


    private List<Cell> _enemyOccupiedCells = new List<Cell>();
    [SerializeField] private List<Ship> _enemyShips = new List<Ship>();
    
    public GameObject cellPrefab;

    private const int BoardWidth = 10;
    private const int BoardHeight = 10;

    // Debug
    public Vector2 gridSize = new Vector2(5, 5);
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    public void BuildPlayerBoard()
    {
        BuildBoard(BoardWidth, BoardHeight, playerBoardParent, true);
    }

    public void BuildTargetingBoard()
    {
        BuildBoard(BoardWidth, BoardHeight, targetingBoardParent, false);
        GenerateEnemyShipPlacements();
    }

    public List<Cell> GetPlayerBoardGrid()
    {
        return _playerBoardGrid;
    }

    public void EnemyTurnStart()
    {
        StartCoroutine(EnemyTurnStartCoroutine());
    }

    IEnumerator EnemyTurnStartCoroutine()
    {
        _gameManager.SetCanPlaceMarkers(false);
        _gameManager.SetEnemyTurn();
        print("Enemy's turn!");
        
        yield return new WaitForSecondsRealtime(Random.Range(0, 5));
        //yield return new WaitForSecondsRealtime(0); // Debug

        var randomVector = new Vector2(Random.Range(0, BoardHeight), Random.Range(0, BoardWidth));
        
        while (GetCellByGridIndex(_playerBoardGrid, randomVector).GetHasMarker())
        {
            randomVector = new Vector2(Random.Range(0, BoardHeight), Random.Range(0, BoardWidth));
        }

        var guessedCell = GetCellByGridIndex(_playerBoardGrid, randomVector);
        
        var result = guessedCell.GetShipState() ? MarkerType.HIT : MarkerType.MISS;
        
        guessedCell.SetMarker(result);
        
        if (result == MarkerType.HIT)
            guessedCell.GetOccupyingShip().MarkShipAsHit();

        print($"Enemy {result} at ({randomVector.x}, {randomVector.y})!");
        
        _gameManager.SetCanPlaceMarkers(true);
        _gameManager.SetPlayerTurn();
        print("Player's turn!");

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
            
            //print($"({randomX}, {randomY}) in the {randomDirection} direction.");
            
            var validCells = GetValidCellsFromPosition(new Vector2(randomX, randomY), randomDirection, shipSizes);

            if (validCells == null) continue;
            //print("Cells are valid!");

            CreateFauxShip(validCells[0].GetShipType(), new Queue<Cell>(validCells));
            
            _enemyOccupiedCells.AddRange(validCells);
            //DebugEnemyShipLocations();
            
            counter++;
        }

        SetCellOccupiedShip();
    }

    private void SetCellOccupiedShip()
    {
        foreach (var cell in _enemyOccupiedCells)
        {
            cell.SetOccupyingShip(_enemyShips.First(s => s.ShipType == cell.GetShipType()));
        }
    }

    private void DebugEnemyShipLocations()
    {
        foreach (var cell in _enemyOccupiedCells)
        {
            cell.DebugSetMarker();
        }
    }

    private void CreateFauxShip(ShipType shipType, Queue<Cell> occupiedCells)
    {
        var shipObject = new GameObject(ReturnShipNameByType(shipType));
        shipObject.transform.SetParent(enemyShipParent);

        Ship ship;
        switch (shipType)
        {
            case ShipType.CARRIER:
                ship = shipObject.AddComponent<Carrier>();
                break;
            case ShipType.BATTLESHIP:
                ship = shipObject.AddComponent<Battleship>();
                break;
            case ShipType.CRUISER:
                ship = shipObject.AddComponent<Cruiser>();
                break;
            case ShipType.SUBMARINE:
                ship = shipObject.AddComponent<Submarine>();
                break;
            case ShipType.DESTROYER:
                ship = shipObject.AddComponent<Destroyer>();
                break;
            default:
                ship = shipObject.AddComponent<Carrier>();
                break;
        }

        ship.SetOccupiedCells(occupiedCells);
        ship.SetShipType(shipType);
        ship.SetShipSize(GetShipSizeByType(shipType));
        ship.SetIsPlaced(true);
        ship.SetShipOwner(ShipOwner.ENEMY);

        foreach (var cell in occupiedCells.ToList())
        {
            cell.SetOccupyingShip(ship);
        }

        _enemyShips.Add(ship);

    }

    private string ReturnShipNameByType(ShipType shipType)
    {
        var shipName = "Enemy ";

        shipName += shipType.ToString().Substring(0,1);
        shipName += shipType.ToString().Substring(1).ToLower();

        return shipName;
    }

    private int GetShipSizeByType(ShipType shipType)
    {
        var shipSize = shipType switch
        {
            ShipType.CARRIER => 5,
            ShipType.BATTLESHIP => 4,
            ShipType.CRUISER => 3,
            ShipType.SUBMARINE => 3,
            ShipType.DESTROYER => 2,
            _ => 0
        };

        return shipSize;
    }

    private List<Cell> GetValidCellsFromPosition(Vector2 cellPosition, Direction startingDirection, List<int> shipSizes)
    {
        var cellToCheck = GetCellByGridIndex(_enemyBoardGrid, cellPosition);
        //print(cellToCheck.name);
        //print($"Does Cell Contain Ship? {cellToCheck.GetShipState()}");
        
        if (cellToCheck == null || cellToCheck.GetShipState()) return null;
        
        var randomShipSize = shipSizes[Random.Range(0, shipSizes.Count)];
        //print($"Ship size to check: {randomShipSize}");

        var isValid = CheckDirectionsFromCell(cellToCheck, startingDirection, randomShipSize, out var validCells);

        if (isValid)
        {
            shipSizes.Remove(randomShipSize);
            
            foreach (var cell in validCells)
            {
                cell.SetShipState(true, GetShipTypeBySize(randomShipSize, shipSizes), null);
            }
        }
        
        return !isValid ? null : validCells;
    }

    private ShipType GetShipTypeBySize(int randomShipSize, List<int> shipSizes)
    {
        if (randomShipSize == 3 && shipSizes.FindAll(i => i == 3).Count == 1) return ShipType.SUBMARINE;
        
        var shipType = randomShipSize switch
        {
            2 => ShipType.DESTROYER,
            3 => ShipType.CRUISER,
            4 => ShipType.BATTLESHIP,
            5 => ShipType.CARRIER,
            _ => ShipType.NONE
        };
        
        return shipType;
    }

    private bool CheckDirectionsFromCell(Cell cellToCheck, Direction startingDirection, int shipSizeToCheck, out List<Cell> validCells)
    {
        var isDirectionValid = true;
        validCells = new List<Cell> { cellToCheck };

        var directionVector = startingDirection switch
        {
            Direction.EAST => new Vector2(1, 0),
            Direction.WEST => new Vector2(-1, 0),
            Direction.SOUTH => new Vector2(0, -1),
            Direction.NORTH => new Vector2(0, 1),
            _ => new Vector2()
        };

        for (var i = 1; i < shipSizeToCheck; i++)
        {
            var cell = GetCellByGridIndex(_enemyBoardGrid, cellToCheck.GetGridLocation() + directionVector * i);
            //print($"Ship Size {shipSizeToCheck}: Check {i}");
            //if (cell != null) print(cell.GetShipState());
            if (cell == null || cell.GetShipState())
            {
                isDirectionValid = false;
                break;
            }
            validCells.Add(cell);
        }

        return isDirectionValid;
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

    private Cell GetCellByGridIndex(List<Cell> board, Vector2 gridIndex)
    {
        var cell = board.FirstOrDefault(c => c.GetGridLocation() == gridIndex);
        if (cell == null) return null;
        return cell;
    }
}
