using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelCreatorInputManager : MonoBehaviour
{
  private SimpleMenu _menu = null;
  private bool initialized = false;

  public void Init(SimpleMenu menu)
  {
    if (initialized)
      return;

    _menu = menu;
    enabled = true;
    initialized = true;
  }

  private void Awake()
  {
    enabled = false;
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
}
