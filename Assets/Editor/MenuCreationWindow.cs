using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuCreationWindow : EditorWindow
{
  const string TITLED_MENU_PATH = "Prefabs/UI/Menu/TitledMenu";
  const string MENU_PATH = "Prefabs/UI/Menu/Menu";
  const string BUTTON_PATH = "Prefabs/UI/Button/Button";

  const int BASE_OFFSET = 150;
  const int OFFSET_BETWEEN_BUTTONS = 80; 

  private GameObject _canvas;
  private GameObject _button;
  private bool _titled = false;
  private int _buttonsNumber = 0;

  [MenuItem("Tools/create a menu")]
  public static void ShowWindow()
  {
    //Show existing window instance. If one doesn't exist, make one.
    EditorWindow.GetWindow(typeof(MenuCreationWindow));
  }

  void OnGUI()
  {
    GUILayout.Label("Menu Creation", EditorStyles.boldLabel);
    _titled = EditorGUILayout.Toggle("Title: ", _titled);
    _buttonsNumber = EditorGUILayout.IntField("Number of buttons: ", _buttonsNumber);
    _canvas = EditorGUILayout.ObjectField("Canvas: ", _canvas, typeof(GameObject), true) as GameObject;
    _button = EditorGUILayout.ObjectField("Button: ", _button, typeof(GameObject), true) as GameObject;

    if (GUILayout.Button("create") && _buttonsNumber > 0)
    {
      if (_titled)
        CreateMenu(TITLED_MENU_PATH);
      else
        CreateMenu(MENU_PATH);
    }
  }

  private void CreateMenu(string menuPath)
  {
    var button = _button;
    if (_button == null)
      button = Resources.Load(BUTTON_PATH) as GameObject;
    GameObject menuPrefab = Resources.Load(menuPath) as GameObject;
    GameObject menuGO = Instantiate(menuPrefab, _canvas.transform);

    for (int i = 0; i < _buttonsNumber; i++)
    {
      var temp_button = Instantiate(button, menuGO.transform);
      temp_button.transform.Translate(0, BASE_OFFSET - OFFSET_BETWEEN_BUTTONS * i, 0);
    }
  }
}
