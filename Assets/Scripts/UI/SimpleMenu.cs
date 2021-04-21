using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleMenu : MonoBehaviour
{
  private bool init = false;

  [SerializeField]
  private GameObject _menuPrefab;
  [SerializeField]
  private GameObject _buttonPrefab;
  [SerializeField]
  private int X_RESOLUTION = 1920;
  [SerializeField]
  private int Y_RESOLUTION = 1080;
  private const int GAP = 120;
  private GameObject _canvasGO;

  private GameObject _menuGO;
  private Button[] _buttons;

  public void Init(string menuName, string[] buttonNames)
  {
    if (init)
      return;

    init = true;
    InitializeCanvas();

    _menuGO = Instantiate(_menuPrefab, _canvasGO.transform);
    _buttons = new Button[buttonNames.Length];

    for(int i = 0; i < buttonNames.Length; i++)
    {
      _buttons[i] = Instantiate(_buttonPrefab, _menuGO.transform).GetComponent<Button>();
      _buttons[i].transform.Translate(0, -GAP * i , 0);
      _buttons[i].SetText(buttonNames[i]);
      _buttons[i].NotHoveredOver();
    }

    _buttons[0].HoveredOver();
    init = true;
  }

  private void InitializeCanvas()
  {
    _canvasGO = new GameObject();
    _canvasGO.name = "Canvas";

    _canvasGO.AddComponent<Canvas>();
    _canvasGO.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

    _canvasGO.AddComponent<CanvasScaler>();
    CanvasScaler myCanvasScaler = _canvasGO.GetComponent<CanvasScaler>();
    myCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    myCanvasScaler.referenceResolution
      = new Vector2(X_RESOLUTION, Y_RESOLUTION);

    _canvasGO.AddComponent<GraphicRaycaster>();
  }
}
