using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreatorMenu : MonoBehaviour
{
  [SerializeField]
  private GameObject _mapCreationMenu = null;

  public void OnNewMapClick()
  {
    _mapCreationMenu.SetActive(true);
    gameObject.SetActive(false);
  }
}
