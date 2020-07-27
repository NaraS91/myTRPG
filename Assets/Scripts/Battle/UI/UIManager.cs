using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{
  private static Button[] buttons;
  private static GameObject CanvasGO; 
  public static int DisplayedButtons { get; private set; } = 0;
  private static int _activeButton = 0;
  private const int NUMBER_OF_BUTTONS = 10;
  private const int X_RESOLUTION = 1920;
  private const int Y_RESOLUTION = 1080;
  private const int X_POS_BUTTON = 225;
  private const int Y_POS_BUTTON = 90;
  private const int GAP_BETWEEN_BUTTONS = 10;
  private static readonly int[] BUTTON_RESOLUTION = { 300, 70 };
  private const string BUTTON_PATH = "Prefabs/UI/Button/Button";

  static UIManager()
  {
    InitializeCanvas();

    GameObject button = Resources.Load(BUTTON_PATH) as GameObject;
    buttons = new Button[NUMBER_OF_BUTTONS];

    for(int i = 0; i < NUMBER_OF_BUTTONS; i++)
    {
      GameObject newButton = 
        Object.Instantiate(button, CanvasGO.transform);
      buttons[i] = newButton.GetComponent<Button>();
      newButton.SetActive(false);
    }
  }

  public static void ShowButtons(int number, string[] names)
  {
    DisplayedButtons = number;
    int yPos = Y_POS_BUTTON + (GAP_BETWEEN_BUTTONS + BUTTON_RESOLUTION[1])
               * (number - 1) / 2;

    for (int i = 0; i < number; i++)
    {
      buttons[i].SetPosition(X_POS_BUTTON, yPos, 0);
      buttons[i].SetText(names[i]);
      buttons[i].gameObject.SetActive(true);
      buttons[i].NotHoveredOver();
      yPos -= GAP_BETWEEN_BUTTONS + BUTTON_RESOLUTION[1];
    }

    _activeButton = 0;
    buttons[0].HoveredOver();
  }

  public static void HideButtons()
  {
    for(int i = 0; i < DisplayedButtons; i++)
    {
      buttons[i].gameObject.SetActive(false);
    }

    DisplayedButtons = 0;
  }

  public static void NextButton()
  {
    if(_activeButton < DisplayedButtons - 1)
    {
      buttons[_activeButton++].NotHoveredOver();
      buttons[_activeButton].HoveredOver();
    }
  }

  public static void PreviousButton()
  {
    if (_activeButton > 0)
    {
      buttons[_activeButton--].NotHoveredOver();
      buttons[_activeButton].HoveredOver();
    }
  }

  public static EButtonType ActiveButtonType()
  {
    return buttons[_activeButton].Type;
  }

  private static void InitializeCanvas()
  {
    CanvasGO = new GameObject();
    CanvasGO.name = "Canvas";

    CanvasGO.AddComponent<Canvas>();
    CanvasGO.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

    CanvasGO.AddComponent<CanvasScaler>();
    CanvasScaler myCanvasScaler = CanvasGO.GetComponent<CanvasScaler>();
    myCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    myCanvasScaler.referenceResolution 
      = new Vector2(X_RESOLUTION, Y_RESOLUTION);

    CanvasGO.AddComponent<GraphicRaycaster>();
  }
}
