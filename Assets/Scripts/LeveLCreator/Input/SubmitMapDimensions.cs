using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class SubmitMapDimensions : MonoBehaviour
{
  private UnityEngine.UI.Button _button;
  [SerializeField]
  private GameObject _selectedTile;
  [SerializeField]
  private Text _text1;
  [SerializeField]
  private Text _text2;
  [SerializeField]
  private LevelCreatorInputManager levelCreatorInputManager;

  private void Awake()
  {
    _button = GetComponent<UnityEngine.UI.Button>();
  }

  private void Start()
  {
    _button.onClick.AddListener(
      () => levelCreatorInputManager.DelegateMapCreation
        (int.Parse(_text1.text), int.Parse(_text2.text), _selectedTile));
  }
}
