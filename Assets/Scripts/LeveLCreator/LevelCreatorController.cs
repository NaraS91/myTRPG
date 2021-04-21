using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelCreatorController : MonoBehaviour
{
  [SerializeField]
  private GameObject _simpleMenuPrefab;
  private SimpleMenu _menu;

  // Start is called before the first frame update
  void Start()
  {
    _menu = Instantiate(_simpleMenuPrefab).GetComponent<SimpleMenu>();
    _menu.Init("Level Creator", new[] { "Add tiles", "Add unit" });
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
