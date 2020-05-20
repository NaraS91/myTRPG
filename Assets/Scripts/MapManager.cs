using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{

  [SerializeField] public ISet<Tile> bfsCheck;
  GameObject player;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  private ISet<Tile> findViableMoves(Unit unit)
  {
    ISet<Tile> result = new HashSet<Tile>();
    Dictionary<Tile, int> distances = new Dictionary<Tile, int>();
    Queue<Tile> queue = new Queue<Tile>();
    int movement = unit.getMovement();

    Tile curr = unit.getOccupiedTile();
    queue.Enqueue(curr);
    distances.Add(curr, 0);

    while (queue.Count > 0)
    {
      curr = queue.Dequeue();
      if (distances[curr] > movement) break;

      result.Add(curr);
      Tile[] adjacentTiles
        = curr.getAdjacentTiles().Where(t => t != null).ToArray();
      int distance = distances[curr];

      foreach (Tile tile in adjacentTiles)
      {
        if (!distances.ContainsKey(tile))
        {
          if (unit.canPass(tile))
          {
            queue.Enqueue(tile);
            distances.Add(tile, distance + 1);
          } else if (tile.isOccupied())
          {
            //TODO: handle situation when objects with different tags meet
          }
        }
      }
    }

      return result;
  }

}
