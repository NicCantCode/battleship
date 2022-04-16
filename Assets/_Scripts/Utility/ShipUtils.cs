using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ShipUtils
{
    public static List<Cell> GetValidCellsFromPosition(List<Cell> enemyBoardGrid, Vector2 cellPosition, Direction startingDirection, List<int> shipSizes)
    {
        var cellToCheck = GetCellByGridIndex(enemyBoardGrid, cellPosition);
        
        if (cellToCheck == null || cellToCheck.GetShipState()) return null;
        
        var randomShipSize = shipSizes[Random.Range(0, shipSizes.Count)];

        var isValid = CheckDirectionsFromCell(enemyBoardGrid, cellToCheck, startingDirection, randomShipSize, out var validCells);

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

    public static void SetCellOccupiedShip(List<Cell> enemyOccupiedCells, List<Ship> enemyShips)
    {
        foreach (var cell in enemyOccupiedCells)
        {
            cell.SetOccupyingShip(enemyShips.First(s => s.ShipType == cell.GetShipType()));
        }
    }

    public static void CreateFauxShip(List<Ship> enemyShips, Transform enemyShipParent, ShipType shipType, Queue<Cell> occupiedCells)
    {
        var shipObject = new GameObject(ReturnShipNameByType(shipType));
        shipObject.transform.SetParent(enemyShipParent);

        var ship = shipObject.AddComponent<Ship>();

        ship.SetOccupiedCells(occupiedCells);
        ship.SetShipType(shipType);
        ship.SetShipSize(GetShipSizeByType(shipType));
        ship.SetIsPlaced(true);
        ship.SetShipOwner(ShipOwner.ENEMY);

        foreach (var cell in occupiedCells.ToList())
        {
            cell.SetOccupyingShip(ship);
        }

        enemyShips.Add(ship);

    }

    public static Vector2 GetVectorByDirection(Direction direction)
    {
        return direction switch
        {
            Direction.EAST => new Vector2(1, 0),
            Direction.WEST => new Vector2(-1, 0),
            Direction.SOUTH => new Vector2(0, -1),
            Direction.NORTH => new Vector2(0, 1),
            _ => new Vector2()
        };
    }

    public static bool CheckIfCellIsInsideBoardBounds(Vector2 boardSize, Vector2 cellGridLocation)
    {
        return cellGridLocation.x >= 0 && cellGridLocation.x < boardSize.x && 
               cellGridLocation.y >= 0 && cellGridLocation.y < boardSize.y;
    }
    
    public static int GetShipSizeByType(ShipType shipType)
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

    public static string ReturnShipNameByType(ShipType shipType)
    {
        var shipName = "Enemy ";

        shipName += shipType.ToString().Substring(0,1);
        shipName += shipType.ToString().Substring(1).ToLower();

        return shipName;
    }

    public static ShipType GetShipTypeBySize(int randomShipSize, List<int> shipSizes)
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

    public static bool CheckDirectionsFromCell(List<Cell> enemyBoardGrid, Cell cellToCheck, Direction startingDirection, int shipSizeToCheck, out List<Cell> validCells)
    {
        var isDirectionValid = true;
        validCells = new List<Cell> { cellToCheck };

        var directionVector = GetVectorByDirection(startingDirection);

        for (var i = 1; i < shipSizeToCheck; i++)
        {
            var cell = GetCellByGridIndex(enemyBoardGrid, cellToCheck.GetGridLocation() + directionVector * i);
            
            if (cell == null || cell.GetShipState())
            {
                isDirectionValid = false;
                break;
            }
            validCells.Add(cell);
        }

        return isDirectionValid;
    }

    public static Cell GetCellByGridIndex(List<Cell> board, Vector2 gridIndex)
    {
        var cell = board.FirstOrDefault(c => c.GetGridLocation() == gridIndex);
        if (cell == null) return null;
        return cell;
    }
}