using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridCreationWindow : EditorWindow
{
  const int ROWS_LIMIT = 20;
  const int COLUMNS_LIMIT = 20;
  const string TILE_PATH = "Prefabs/Tiles/Tile";

  int rowsCount = 0;
  int columsCount = 0;

  // Add menu item named "My Window" to the Window menu
  [MenuItem("Tools/create a grid")]
  public static void ShowWindow()
  {
    //Show existing window instance. If one doesn't exist, make one.
    EditorWindow.GetWindow(typeof(GridCreationWindow));
  }

  void OnGUI()
  {
    GUILayout.Label("Grid Creation", EditorStyles.boldLabel);
    rowsCount = EditorGUILayout.IntField("Rows", rowsCount);
    columsCount = EditorGUILayout.IntField("Columns", columsCount);

    if (GUILayout.Button("create"))
    {
      GameObject tile = Resources.Load(TILE_PATH) as GameObject;
      if(0 < columsCount && columsCount <= COLUMNS_LIMIT &&
         0 < rowsCount && rowsCount <= ROWS_LIMIT)
      {
        CreateGrid(tile);
      }
    }
  }

  private void CreateGrid(GameObject tile)
  {
    GameObject map = Instantiate(new GameObject("Map"));

    int height = rowsCount;
    int width = columsCount;

    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        Instantiate(tile, new Vector3(i - width / 2, 0.0f, j - height / 2), new Quaternion(), map.transform);
      }
    }
  }
}
