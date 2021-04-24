using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelCreatorInputManager))]
public class LevelCreatorManager : MonoBehaviour
{
  [SerializeField]
  private GameObject _simpleMenuPrefab;
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
  {
    _menu = Instantiate(_simpleMenuPrefab).GetComponent<SimpleMenu>();
    _menu.Init("Level Creator", _buttonNames);
    _inputManager = GetComponent<LevelCreatorInputManager>();
  }

  void Start()
  {
    _inputManager.Init(_menu);
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
