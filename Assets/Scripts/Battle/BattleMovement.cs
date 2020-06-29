using System.Collections.Generic;
using UnityEngine;


public class BattleMovement
{
  private static LinkedList<Tile> _path = new LinkedList<Tile>();
  private static int _cost = 0;

  public static void ResetPath()
  {
    _path.Clear();
    _cost = 0;
  }


  //PRE: tile is in range of unit
  public static void AddTile(Tile tile, Unit unit)
  {
    if(unit.Movement < _cost + tile.Cost)
    {
      HidePath();
      RecalculatePath(unit, tile);
    } else
    {
      _path.AddLast(tile);
      _cost += tile.Cost;
    }
  }

  
  public static void ShowPath()
  {
    foreach(Tile tile in _path)
    {
      tile.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }
  }


  public static void HidePath()
  {
    foreach (Tile tile in _path)
    {
      tile.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }
  }


  public static ISet<Tile> FindViableMoves(Unit unit)
  {
    ISet<Tile> result = new HashSet<Tile>();
    Dictionary<Tile, int> distances = new Dictionary<Tile, int>();
    Queue<Tile> tilesToProcess = new Queue<Tile>();
    int movement = unit.Movement;

    Tile curr = unit.OccupiedTile;
    tilesToProcess.Enqueue(curr);
    distances.Add(curr, 0);

    while (tilesToProcess.Count > 0)
    {
      curr = tilesToProcess.Dequeue();
      if (distances[curr] > movement) break;

      result.Add(curr);
      Tile[] adjacentTiles = curr.GetAdjacentTiles();
      int distance = distances[curr];

      foreach (Tile tile in adjacentTiles)
      {
        if (unit.CanPass(tile))
        {
          if (!distances.ContainsKey(tile))
          {
            tilesToProcess.Enqueue(tile);
            distances.Add(tile, distance + tile.Cost);
          } 
          else if (distances[tile] > distance + tile.Cost)
          {
            tilesToProcess.Enqueue(tile);
            distances[tile] = distance + tile.Cost;
          }
        } 
        else if (tile.IsOccupied())
        {
          //TODO: units from different groups meet
        }

      }
    }

    return result;
  }

  //PRE: tile is in reach of unit
  private static void RecalculatePath(Unit unit, Tile targetTile)
  {
    //nulls in queue represent going back in path
    Stack<Tile> tilesToProcess = new Stack<Tile>();
    Dictionary<Tile, int> distances = new Dictionary<Tile, int>();

    ResetPath();
    Tile curr = unit.OccupiedTile;
    distances.Add(curr, 0);
    _path.AddLast(curr);
    tilesToProcess.Push(curr);

    while(curr != targetTile)
    {
      tilesToProcess.Push(null);

      foreach(Tile tile in curr.GetAdjacentTiles())
      {
        if (unit.CanPass(tile))
        {
          if (!distances.ContainsKey(tile))
          {
            distances.Add(tile, _cost + tile.Cost);
            tilesToProcess.Push(tile);
          }
          else if (tile.Cost + _cost < distances[tile])
          {
            distances[tile] = tile.Cost + _cost;
            tilesToProcess.Push(tile);
          }
        }
      }

      curr = tilesToProcess.Pop();
      while(curr == null || _cost + curr.Cost > unit.Movement)
      {
        if (curr == null)
        {
          _cost -= _path.Last.Value.Cost;
          _path.RemoveLast();
        }

        curr = tilesToProcess.Pop();
      }

      _path.AddLast(curr);
      _cost += curr.Cost;
    }
  }
}
