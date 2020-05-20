using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tools
{
  const int WIDTH = 10;
  const int HEIGHT = 10;

  //creates a tilemap of dimansion WIDTHxHEIGHT using tile on scene
  [MenuItem("Tools/create a grid")]
  static void Create10x10Grid()
  {
    GameObject tile = GameObject.FindGameObjectWithTag("Tile");
    GameObject map = Object.Instantiate(new GameObject("Map"));

    for(int i = 0; i < WIDTH; i++)
    {
      for(int j = 0; j < HEIGHT; j++)
      {
        Object.Instantiate(tile, new Vector3(i - WIDTH/2, 0.0f, j - HEIGHT/2), new Quaternion(), map.transform);
      }
    }

    Object.DestroyImmediate(tile);
  }


}
