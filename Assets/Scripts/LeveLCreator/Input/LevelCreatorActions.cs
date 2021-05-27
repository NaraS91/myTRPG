using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreatorActions : MonoBehaviour
{
  public static Tile[,] CreateMap(int x, int y, Tile[,] mapTiles, GameObject map, GameObject tileGO)
  {
    if (mapTiles != null) 
    {
      foreach (Tile tile in mapTiles)
      {
        if (tile != null)
          Destroy(tile.gameObject);
      }
    }

    mapTiles = new Tile[x, y];

    for (int i = 0; i < x; i++)
    {
      for (int j = 0; j < y; j++)
      {
        GameObject t = Instantiate(tileGO, map.transform);
        mapTiles[i, j] = t.GetComponent<Tile>();
        t.transform.position = new Vector3(i * mapTiles[i, j].Width, 0, j * mapTiles[i, j].Length);
      }
    }

    return mapTiles;
  }
}
