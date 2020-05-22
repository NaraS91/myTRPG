using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

  public List<ISet<FightingUnit>> FightingGroups { get; set; }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  private ISet<Tile> FindViableMoves(Unit unit)
  {
    ISet<Tile> result = new HashSet<Tile>();
    Dictionary<Tile, int> distances = new Dictionary<Tile, int>();
    Queue<Tile> queue = new Queue<Tile>();
    int movement = unit.GetMovement();

    Tile curr = unit.GetOccupiedTile();
    queue.Enqueue(curr);
    distances.Add(curr, 0);

    while (queue.Count > 0)
    {
      curr = queue.Dequeue();
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
            queue.Enqueue(tile);
            distances.Add(tile, distance + 1);
          } else if (tile.IsOccupied())
          {
            //TODO: handle situation when objects with different tags meet
          }
        }
      }
    }

      return result;
  }

}
