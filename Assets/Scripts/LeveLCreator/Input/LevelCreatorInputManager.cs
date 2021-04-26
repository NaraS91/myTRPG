using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelCreatorInputManager : MonoBehaviour
{
  private SimpleMenu _menu = null;
  private bool initialized = false;
  private Tile[,] _mapTiles;
  private GameObject _map;
  [SerializeField]
  private GameObject preFab;

  public void Init(SimpleMenu menu)
  {
    if (initialized)
      return;

    _menu = menu;
    enabled = true;
    initialized = true;
    _map = new GameObject("map");
  }

  private void Awake()
  {
    enabled = false;
  }

  private void Start()
  {
    CreateMap(15, 15, preFab);
  }

  // Update is called once per frame
  void Update()
  {
    HandleInput();
  }

  private void HandleInput()
  {
    if (Input.GetButtonDown("Select"))
    {
      switch (_menu.GetActiveButton())
      {
        case EButtonType.MapDimensions:
          HandleMapDimension();
          break;
        default:
          throw new NotImplementedException();
      }
    }
  }

  private void HandleMapDimension()
  {

  }

  private void CreateMap(int x, int y, GameObject tile)
  {
    _mapTiles = new Tile[x, y];

    for(int i = 0; i < x; i++)
    {
      for(int j = 0; j < y; j++)
      {
        GameObject t = Instantiate(tile, _map.transform);
        _mapTiles[i, j] = t.GetComponent<Tile>();
        t.transform.position = new Vector3(i * _mapTiles[i, j].Width, 0, j * _mapTiles[i, j].Length);
      }
    }
  }
}
