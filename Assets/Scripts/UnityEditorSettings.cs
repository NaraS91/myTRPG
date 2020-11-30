using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityEditorSettings : MonoBehaviour
{
  public int FPS_LIMIT;

  void Start()
  {
    Application.targetFrameRate = FPS_LIMIT;
  }
}
