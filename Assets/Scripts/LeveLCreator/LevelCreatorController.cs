using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelCreatorController : MonoBehaviour
{
  [SerializeField]
  private GameObject _simpleMenuPrefab;
  private string[] _buttonNames =
  {
    "Map Dimensions",
    "Default Tile",
    "Add/Remove Rows/Columns",
    "Edit Mode",
    "Save"
  };
  private SimpleMenu _menu;

  // Start is called before the first frame update
  void Start()
  {
    _menu = Instantiate(_simpleMenuPrefab).GetComponent<SimpleMenu>();
    _menu.Init("Level Creator", _buttonNames);
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
