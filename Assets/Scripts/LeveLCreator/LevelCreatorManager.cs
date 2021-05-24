using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LevelCreatorInputManager))]
public class LevelCreatorManager : MonoBehaviour
{
  [SerializeField]
  private GameObject _simpleMenuPrefab;
  [SerializeField]
  private GameObject _mapCreationPrefab;
  [SerializeField]
  private GameObject _mapCreationCanGO;
  private LevelCreatorInputManager _inputManager;
  private string[] _buttonNames =
  {
    "Map Dimensions",
    "Default Tile",
    "Add/Remove Rows/Columns",
    "Edit Mode",
    "Save"
  };
  private SimpleMenu _menu;

  private void Awake()
  {/*
    _menu = Instantiate(_simpleMenuPrefab).GetComponent<SimpleMenu>();
    _menu.Init("Level Creator", _buttonNames);
    _inputManager = GetComponent<LevelCreatorInputManager>();*/
    //InitMapCreationMenu();
  }

  void Start()
  {
    //_inputManager.Init(_menu, _mapCreationCanGO);
  }

  // Update is called once per frame
  void Update()
  {
        
  }
  /*
  private void InitMapCreationMenu()
  {
    _mapCreationCanGO = new GameObject("Map Creation Menu");
    Canvas can = _mapCreationCanGO.AddComponent<Canvas>();
    CanvasScaler canScaler = _mapCreationCanGO.AddComponent<CanvasScaler>();

    canScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    canScaler.referenceResolution = new Vector2(1920, 1080);
    _mapCreationCanGO.AddComponent<GraphicRaycaster>();
    can.renderMode = RenderMode.ScreenSpaceOverlay;
    Instantiate(_mapCreationPrefab, can.transform);
    _mapCreationCanGO.SetActive(false);
  }*/
}
