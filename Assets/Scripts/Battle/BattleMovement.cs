using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMovement
{
  private static List<Tile> _path = new List<Tile>();

  public static void ResetPath()
  {
    _path.Clear();
  }

  //PRE: tile is in range of unit
  public static void AddTile(Tile tile, Unit unit)
  {
    if(unit.Movement < _path.Count + 1)
    {
      RecalculatePath(unit, tile);
    } else
    {
      _path.Add(tile);
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
        if (!distances.ContainsKey(tile))
        {
          if (unit.CanPass(tile))
          {
            tilesToProcess.Enqueue(tile);
            distances.Add(tile, distance + 1);
          }
          else if (tile.IsOccupied())
          {
            //TODO: units from different groups meet
          }
        }
      }
    }

    return result;
  }

  private static void RecalculatePath(Unit unit, Tile tile)
  {
    //TODO: implement recalculating path
    HidePath();
    ResetPath();
  }
}
