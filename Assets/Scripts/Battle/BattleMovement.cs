using System.Collections.Generic;

public class BattleMovement
{
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
}
