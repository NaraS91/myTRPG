﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BattleMovementUtils
{
  public static LinkedList<Tile> Path { get; private set; } = new LinkedList<Tile>();
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
    Path.Clear();
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
    else if (Path.Contains(tile))
    {
      while (Path.Count != 0 && Path.Last.Value != tile)
      {
        _cost -= Path.Last.Value.Cost;
        Path.RemoveLast();
      }
    }
    else if (unit.Movement < _cost + tile.Cost || (Path.Count > 0 &&
             !Path.Last.Value.GetAdjacentTiles().Contains(tile)) ||
             (Path.Count == 0 &&
             !tile.GetAdjacentTiles().Contains(unit.OccupiedTile)))
    {
      RecalculatePath(unit, tile);
    } 
    else
    { 
      Path.AddLast(tile);
      _cost += tile.Cost;
    }
  }

  
  public static void ShowPath()
  {
    foreach(Tile tile in Path)
    {
      tile.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }
  }


  public static void HidePath()
  {
    foreach (Tile tile in Path)
    {
      tile.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }
  }

  //PRE:: minAttackRange <= maxAttackRange
  //returns tiles that are in range of unit attack
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

  // returns tiles unit can move to
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

      }
    }

    return result;
  }

  //PRE: tile is in reach of unit
  //returns viable path from unit to targetTile (A* algorithm)
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

  //translates path encoded in PathTile type to classes linkedlist _path
  private static void RecreatePath(PathTile endTile)
  {
    ResetPath();
    PathTile curr = endTile;
    _cost = endTile.Cost;

    while(curr.Prev != null)
    {
      Path.AddFirst(curr.Curr);
      curr = curr.Prev;
    }
  }

  //heuristic function to underevaluate the real distance between tiles
  //here its just geometric distance between middles of the tiles
  private static int AproxDistance(Tile from, Tile to)
  {
    return (int) Math.Abs(from.transform.position.x - to.transform.position.x)
      + (int) Math.Abs(from.transform.position.z - to.transform.position.z);
  }
}
