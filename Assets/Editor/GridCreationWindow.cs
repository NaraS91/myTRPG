using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridCreationWindow : EditorWindow
{
  const int ROWS_LIMIT = 20;
  const int COLUMNS_LIMIT = 20;
  const string TILE_PATH = "Prefabs/Tiles/Tile";

  int _rowsCount = 0;
  int _columsCount = 0;

  [MenuItem("Tools/create a grid")]
  public static void ShowWindow()
  {
    //Show existing window instance. If one doesn't exist, make one.
    EditorWindow.GetWindow(typeof(GridCreationWindow));
  }

  void OnGUI()
  {
    GUILayout.Label("Grid Creation", EditorStyles.boldLabel);
    _rowsCount = EditorGUILayout.IntField("Rows", _rowsCount);
    _columsCount = EditorGUILayout.IntField("Columns", _columsCount);

    if (GUILayout.Button("create"))
    {
      GameObject tile = Resources.Load(TILE_PATH) as GameObject;
      if(0 < _columsCount && _columsCount <= COLUMNS_LIMIT &&
         0 < _rowsCount && _rowsCount <= ROWS_LIMIT)
      {
        CreateGrid(tile);
      }
    }
  }

  private void CreateGrid(GameObject tile)
  {
    GameObject map = Instantiate(new GameObject("Map"));

    int height = _rowsCount;
    int width = _columsCount;

    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        Instantiate(tile, new Vector3(i - width / 2, 0.0f, j - height / 2), new Quaternion(), map.transform);
      }
    }
  }
}
