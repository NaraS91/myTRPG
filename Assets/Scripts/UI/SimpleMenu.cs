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
  private int X_RESOLUTION;
  [SerializeField]
  private int Y_RESOLUTION;
  [SerializeField]
  private int BASE_OFFSET;
  private const int GAP = 80;
  private GameObject _canvasGO;

  private bool _lastUp = false;
  private GameObject _menuGO;
  private Button[] _buttons;
  private int _activeButton = 0;

  public void Update()
  {
    handleInput();
  }

  private void handleInput()
  {
    if (Input.GetButtonDown("Vertical"))
    {
      if (Input.GetAxisRaw("Vertical") < 0)
      {
        nextButton();
        _lastUp = false;
      }
      else if (Input.GetAxisRaw("Vertical") > 0 || !_lastUp)
      {
        previousButton();
        _lastUp = true;
      } else
      {
        nextButton();
        _lastUp = false;
      }
    }
  }

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
      _buttons[i].transform.Translate(0, BASE_OFFSET - GAP * i , 0);
      _buttons[i].SetText(buttonNames[i]);
      _buttons[i].NotHoveredOver();
    }

    _buttons[0].HoveredOver();
    init = true;
  }

  private void nextButton()
  {
    _buttons[_activeButton++].NotHoveredOver();
    _activeButton %= _buttons.Length;
    _buttons[_activeButton].HoveredOver();
  }

  private void previousButton()
  {
    _buttons[_activeButton--].NotHoveredOver();
    _activeButton = (_activeButton + _buttons.Length) % _buttons.Length;
    _buttons[_activeButton].HoveredOver();
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
