using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelCreatorInputManager : MonoBehaviour
{
  private SimpleMenu _menu;
  private bool initialized = false;
  private Tile[,] _mapTiles;
  private GameObject _map;
  private LevelCreatorActions _levelCreatorActions;
  private GameObject _mapCreationCanGO;

  public void Init(SimpleMenu menu, GameObject mapCreationCan)
  {
    if (initialized)
      return;

    _menu = menu;
    enabled = true;
    initialized = true;
    _map = new GameObject("map");

    _mapCreationCanGO = mapCreationCan;
  }

  private void Awake()
  {
    /*
    enabled = false;
    _levelCreatorActions = gameObject.AddComponent<LevelCreatorActions>();
    */
  }

  private void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
   // HandleInput();
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

  public void DelegateMapCreation(int x, int y, GameObject tile)
  {
    _mapTiles = _levelCreatorActions.CreateMap(x, y, _mapTiles, _map, tile);
  }

  private void HandleMapDimension()
  {
    _mapCreationCanGO.SetActive(true);
  }
}
