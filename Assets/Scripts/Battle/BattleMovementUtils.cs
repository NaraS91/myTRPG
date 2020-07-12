using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BattleMovementUtils
{
  private static LinkedList<Tile> _path = new LinkedList<Tile>();
  private static int _cost = 0;

  private class PathTile
  {
    public PathTile Prev { get; }
    public Tile Curr { get; }
    public int Cost { get; }
    public PathTile(PathTile prev, Tile curr, int cost)
    {
      Prev = prev;
      Curr = curr;
      Cost = cost;
    }
  }

  public static void ResetPath()
  {
    _path.Clear();
    _cost = 0;
  }


  //adds tile to the _path if its in range of unit
  //or recalculates the _path to include the tile if its in range of unit
  public static void AddTile(Tile tile, Unit unit)
  {
    if (!unit.CanPass(tile))
    {
      return;
    }

    if(tile == unit.OccupiedTile)
    {
      ResetPath();
    }
    else if (_path.Contains(tile))
    {
      while (_path.Count != 0 && _path.Last.Value != tile)
      {
        _cost -= _path.Last.Value.Cost;
        _path.RemoveLast();
      }
    }
    else if (unit.Movement < _cost + tile.Cost || (_path.Count > 0 &&
             !_path.Last.Value.GetAdjacentTiles().Contains(tile)) ||
             (_path.Count == 0 &&
             !tile.GetAdjacentTiles().Contains(unit.OccupiedTile)))
    {
      RecalculatePath(unit, tile);
    } 
    else
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

  //PRE:: minAttackRange <= maxAttackRange
  public static ISet<Tile> FindAttackedTiles
    (ISet<Tile> viableMoves, int minAttackRange, int maxAttackRange)
  {
    ISet<Tile> result = new HashSet<Tile>();
    foreach(Tile tile in viableMoves)
    {
      for(int i = minAttackRange; i <= maxAttackRange; i++)
      {
        result.UnionWith(tile.FindTiles(i));
      }
    }

    return result;
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
    //PathTile represents path in a similar way to linkedList
    PriorityQueue<PathTile> tilesToProcess = new PriorityQueue<PathTile>();
    Dictionary<Tile, int> distances = new Dictionary<Tile, int>();

    PathTile curr = new PathTile(null, unit.OccupiedTile, 0);
    distances.Add(curr.Curr, 0);

    while(curr.Curr != targetTile)
    {
      foreach (Tile tile in curr.Curr.GetAdjacentTiles())
      {
        if (unit.CanPass(tile))
        {
          if (!distances.ContainsKey(tile))
          {
            distances.Add(tile, curr.Cost + tile.Cost);
            tilesToProcess.Push(new PathTile(curr, tile, curr.Cost + tile.Cost)
              , -(distances[tile] + AproxDistance(tile, targetTile)));
          }
          else if (curr.Cost + tile.Cost < distances[tile])
          {
            distances[tile] = curr.Cost + tile.Cost;
            tilesToProcess.Push(new PathTile(curr, tile, curr.Cost + tile.Cost)
              , -(distances[tile] + AproxDistance(tile, targetTile)));
          }
        }
      }

      curr = tilesToProcess.Pop();
    }

    RecreatePath(curr);
  }

  private static void RecreatePath(PathTile endTile)
  {
    ResetPath();
    PathTile curr = endTile;
    _cost = endTile.Cost;

    while(curr.Prev != null)
    {
      _path.AddFirst(curr.Curr);
      curr = curr.Prev;
    }
  }

  private static int AproxDistance(Tile from, Tile to)
  {
    return (int) Math.Abs(from.transform.position.x - to.transform.position.x)
      + (int) Math.Abs(from.transform.position.z - to.transform.position.z);
  }
}
