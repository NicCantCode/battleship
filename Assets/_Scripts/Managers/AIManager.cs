using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private GameManager _gameManager;
    private BoardManager _boardManager;
    private LogManager _logManager;
    
    private Stack<Cell> _playerBoardStack;
    
    // Intermediate Difficulty Variables
    private Cell _originHitCell;
    private Vector2 _currentSearchDirection;
    private int _searchStep;
    private bool _searchingForHitShip;
    private bool _foundDirectionToSearch;
    private bool _directionAlreadyReversed;
    private bool _bothDirectionAxisChecked;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        _boardManager = GameObject.FindGameObjectWithTag("Board Manager").GetComponent<BoardManager>();
        _logManager = GameObject.FindGameObjectWithTag("Log Manager").GetComponent<LogManager>();
        _searchingForHitShip = false;
        _foundDirectionToSearch = false;
        _directionAlreadyReversed = false;
        _bothDirectionAxisChecked = false;
        _searchStep = 1;
    }

    public void SetPlayerBoardStack(Stack<Cell> playerBoardStack)
    {
        _playerBoardStack = playerBoardStack;
    }

    public void EnemyTurnStart()
    {
        StartCoroutine(EnemyTurnStartCoroutine());
    }

    IEnumerator EnemyTurnStartCoroutine()
    {
        _gameManager.SetCanPlaceMarkers(false);
        _logManager.LogMessage("Enemy's turn!", Color.yellow);
        
        yield return new WaitForSecondsRealtime(0); // Debug
        
        DoAI(DifficultyManager.Instance.difficulty, out var guessedCell, out var result);

        _logManager.LogMessage($"Enemy {result} at {Utils.GridPositionToBattleshipPositionAsString(guessedCell.GetGridLocation())}!", result == MarkerType.HIT ? Color.red : Color.white);
        
        if (result == MarkerType.HIT) guessedCell.GetOccupyingShip().MarkShipAsHit();
        
        _gameManager.SetCanPlaceMarkers(true);
        _logManager.LogMessage("Player's turn!", Color.green);
    }

    private void DoAI(Difficulty gameDifficulty, out Cell guessedCell, out MarkerType result)
    {
        switch (gameDifficulty)
        {
            case Difficulty.SIMPLE:
                AISimple(_playerBoardStack, out var guessedCellSimple, out var resultSimple);
                guessedCell = guessedCellSimple;
                result = resultSimple;
                break;
            case Difficulty.INTERMEDIATE:
                AIIntermediate(_playerBoardStack, out var guessedCellIntermediate, out var resultIntermediate);
                guessedCell = guessedCellIntermediate;
                result = resultIntermediate;
                break;
            default:
                guessedCell = null;
                result = MarkerType.MISS;
                break;
        }
    }
    
    // This AI chooses a random cell each turn using a randomized stack of the board cells
    private void AISimple(Stack<Cell> playerBoard, out Cell guessedCell, out MarkerType result)
    {
        guessedCell = playerBoard.Pop();
        result = guessedCell.GetShipState() ? MarkerType.HIT : MarkerType.MISS;
        guessedCell.SetMarker(result);
    }

    // This AI is a mess and I think it is due to the fundamental architecture of how I made this game and,
    // it isn't worth fixing at the current time. It works well enough that I am comfortable calling it a day
    // and moving onto other projects.
    
    // This AI chooses a random cell each turn using a randomized stack of the board cells
    // This AI is hit ship aware and will progressively search around hit markers for the ship
    private void AIIntermediate(Stack<Cell> playerBoard, out Cell guessedCell, out MarkerType result)
    {
        print($"Searching for hit ship: {_searchingForHitShip}");
        
        // When hit ship is found, behaviour changes to radial search around hit marker
        if (_searchingForHitShip)
        {
            print($"Found direction to search: {_foundDirectionToSearch}");
            
            // If a direction to search was already found, don't execute this code
            if (!_foundDirectionToSearch)
            {
                // 4 for the 4 cardinal directions - get a valid direction to start going in
                for (var i = 0; i < 4; i++)
                {
                    print($"Searching at ({GetCellLocationInDirectionFromOrigin(_originHitCell, _currentSearchDirection).x},{GetCellLocationInDirectionFromOrigin(_originHitCell, _currentSearchDirection).y})");
                    // Check if cell to check is inside the bounds of the board
                    if (ShipUtils.CheckIfCellIsInsideBoardBounds(new Vector2(10, 10), GetCellLocationInDirectionFromOrigin(_originHitCell, _currentSearchDirection)))
                    {
                        print("Cell is inside the bounds of the board.");
                        
                        // Check if cell in valid direction has a marker, if it does continue search
                        if (ShipUtils.GetCellByGridIndex(_boardManager.GetPlayerBoardGrid(), GetCellLocationInDirectionFromOrigin(_originHitCell, _currentSearchDirection)).GetHasMarker())
                        {
                            print("Cell has a valid marker, continuing search.");
                            NextDirectionClockwise();
                            continue;
                        }
                    
                        print($"Cell has no valid marker, search concluded with direction vector: ({_currentSearchDirection.x},{_currentSearchDirection.y})");
                        // if it is a valid cell and doesn't have a marker break from loop and use current direction to continue searching
                        _foundDirectionToSearch = true;
                        break;
                    }

                    print("Cell is not inside the bounds of the board, changing direction clockwise.");
                    NextDirectionClockwise(); // if not a valid cell in this direction, rotate direction to check clockwise
                }
            }
            
            // Search in current direction until a marker is found or until a MISS is registered
            if (_foundDirectionToSearch)
            {
                guessedCell = ShipUtils.GetCellByGridIndex(_boardManager.GetPlayerBoardGrid(), GetCellLocationInDirectionFromOrigin(_originHitCell, _currentSearchDirection * _searchStep));

                if (guessedCell == null)
                {
                    print($"Cell at ({GetCellLocationInDirectionFromOrigin(_originHitCell, _currentSearchDirection * _searchStep).x},{GetCellLocationInDirectionFromOrigin(_originHitCell, _currentSearchDirection * _searchStep).y}) is not in bounds of board.");
                    if (_directionAlreadyReversed)
                    {
                        print($"Both direction axis checked: {_bothDirectionAxisChecked}");
                        if (!_bothDirectionAxisChecked)
                        {
                            print("NULL: Resetting search step to 1.");
                            print("NULL: Resetting direction already reversed to false.");
                            print("NULL: Resetting found direction to search to false.");
                            print("NULL: Changing current ship search direction to new axis.");
                            print("Returning to ship search algorithm at new axis.");
                            _searchStep = 1;
                            _directionAlreadyReversed = false;
                            _foundDirectionToSearch = false;
                            _currentSearchDirection = ShipUtils.GetVectorByDirection(Direction.EAST);
                            _bothDirectionAxisChecked = true;
                            
                            guessedCell = ShipUtils.GetCellByGridIndex(_boardManager.GetPlayerBoardGrid(), GetCellLocationInDirectionFromOrigin(_originHitCell, _currentSearchDirection * _searchStep));
                        } 
                        else if (_bothDirectionAxisChecked)
                        {
                            print("NULL: Resetting search step to 1.");
                            print("NULL: Resetting direction already reversed to false.");
                            print("NULL: Resetting found direction to search to false.");
                            print("NULL: Resetting searching for hit ship to false.");
                            print("Returning to random search algorithm.");
                            _searchStep = 1;
                            _directionAlreadyReversed = false;
                            _foundDirectionToSearch = false;
                            _searchingForHitShip = false;
                            _bothDirectionAxisChecked = false;
                            
                            guessedCell = playerBoard.Pop();
                            result = guessedCell.GetShipState() ? MarkerType.HIT : MarkerType.MISS;
                            guessedCell.SetMarker(result);

                            print($"RANDOM: {guessedCell.name} is a {result}.");
        
                            // If randomly selected cell was a hit, activate radial search on next turn
                            if (result == MarkerType.HIT)
                            {
                                print("NULL: Setting searching for hit ship to true.");
                                print($"NULL: Setting origin cell to {guessedCell.name}.");
                                print($"NULL: Setting direction search vector to ({Vector2.up.x},{Vector2.up.y}).");
                                print("Entering ship search algorithm.");
                                _searchingForHitShip = true;
                                _originHitCell = guessedCell;
                                _currentSearchDirection = ShipUtils.GetVectorByDirection(Direction.NORTH);
                            }
                        
                            return;
                        }
                    }
                    else if (!_directionAlreadyReversed)
                    {
                        print("NULL: Reversing search direction.");
                        print("NULL: Resetting search step to 1.");
                        _directionAlreadyReversed = true;
                        _searchStep = 1;
                        ReverseSearchDirection();
                        
                        guessedCell = ShipUtils.GetCellByGridIndex(_boardManager.GetPlayerBoardGrid(), GetCellLocationInDirectionFromOrigin(_originHitCell, _currentSearchDirection * _searchStep));
                    }
                }

                result = guessedCell.GetShipState() ? MarkerType.HIT : MarkerType.MISS;
                guessedCell.SetMarker(result);
                
                print($"SEARCH: {guessedCell.name} is a {result}.");
                print($"Removing {guessedCell.name} from stack.");
                RemoveGuessedCellFromStack(guessedCell);

                if (result == MarkerType.HIT)
                {
                    print("HIT: Increasing search step.");
                    _searchStep++;
                }

                if (result == MarkerType.MISS && _directionAlreadyReversed)
                {
                    if (_bothDirectionAxisChecked)
                    {
                        print("MISS: Resetting search step to 1.");
                        print("MISS: Resetting direction already reversed to false.");
                        print("MISS: Resetting found direction to search to false.");
                        print("MISS: Resetting searching for hit ship to false.");
                        print("MISS: Resetting both axis checked to false.");
                        print("Returning to random search algorithm.");
                        _searchStep = 1;
                        _directionAlreadyReversed = false;
                        _foundDirectionToSearch = false;
                        _searchingForHitShip = false;
                        _bothDirectionAxisChecked = false;
                    }
                    else if (!_bothDirectionAxisChecked)
                    {
                        print("MISS: Resetting search step to 1.");
                        print("MISS: Resetting direction already reversed to false.");
                        print("MISS: Resetting found direction to search to false.");
                        print("MISS: Changing current ship search direction to new axis.");
                        print("MISS: Setting both axis checked to true.");
                        print("Returning to ship search algorithm at new axis.");
                        _searchStep = 1;
                        _directionAlreadyReversed = false;
                        _foundDirectionToSearch = false;
                        _currentSearchDirection = ShipUtils.GetVectorByDirection(Direction.EAST);
                        _bothDirectionAxisChecked = true;
                    }
                }
                else if (result == MarkerType.MISS && !_directionAlreadyReversed)
                {
                    print("MISS: Reversing search direction.");
                    print("MISS: Resetting search step to 1.");
                    _directionAlreadyReversed = true;
                    _searchStep = 1;
                    ReverseSearchDirection();
                }

                return;
            }
        }

        print("Getting random cell.");
        // Same as Simple AI when a hit ship isn't found yet
        guessedCell = playerBoard.Pop();
        result = guessedCell.GetShipState() ? MarkerType.HIT : MarkerType.MISS;
        guessedCell.SetMarker(result);

        print($"RANDOM: {guessedCell.name} is a {result}.");
        
        // If randomly selected cell was a hit, activate radial search on next turn
        if (result == MarkerType.HIT)
        {
            print("RANDOM: Setting searching for hit ship to true.");
            print($"RANDOM: Setting origin cell to {guessedCell.name}.");
            print($"RANDOM: Setting direction search vector to ({Vector2.up.x},{Vector2.up.y}).");
            print("Entering ship search algorithm.");
            _searchingForHitShip = true;
            _originHitCell = guessedCell;
            _currentSearchDirection = ShipUtils.GetVectorByDirection(Direction.NORTH);
        }
        
    }

    private void RemoveGuessedCellFromStack(Cell guessedCell)
    {
        var tempList = new List<Cell>(_playerBoardStack);

        foreach (var cell in tempList)
        {
            if (guessedCell.GetGridLocation() == cell.GetGridLocation())
            {
                tempList.Remove(cell);
                break;
            }
        }

        _playerBoardStack = Utils.ToStack(tempList);
    }

    private Vector2 GetCellLocationInDirectionFromOrigin(Cell originCell, Vector2 directionVector)
    {
        return originCell.GetGridLocation() + directionVector;
    }

    private void ReverseSearchDirection()
    {
        if (_currentSearchDirection == Vector2.up) _currentSearchDirection = Vector2.down;
        else if (_currentSearchDirection == Vector2.right) _currentSearchDirection = Vector2.left;
        else if (_currentSearchDirection == Vector2.down) _currentSearchDirection = Vector2.up;
        else if (_currentSearchDirection == Vector2.left) _currentSearchDirection = Vector2.right;
    }

    private void NextDirectionClockwise()
    {
        if (_currentSearchDirection == Vector2.up) _currentSearchDirection = Vector2.right;
        else if (_currentSearchDirection == Vector2.right) _currentSearchDirection = Vector2.down;
        else if (_currentSearchDirection == Vector2.down) _currentSearchDirection = Vector2.left;
        else if (_currentSearchDirection == Vector2.left) _currentSearchDirection = Vector2.up;
    }
}