﻿using UnityEngine;

[RequireComponent(typeof(BattleManager))]
public class InputManager : MonoBehaviour
{
  [SerializeField] private BattleManager _battleManager;
  [SerializeField] private MovementInput _movementInput;
  [SerializeField] private MenuInput _menuInput;
  private Cursor _cursor;
  public bool MenuIsUp { get; private set; } = false;
  public InputState InputState { get; set; } = InputState.Movement;
  public bool SelectDown { get; private set; }
  public bool CancelDown { get; private set; }
  public bool DownDirection { get; private set; }
  public bool UpDirection { get; private set; }
  private float _previousVertical;
  private const float _controllerSensitivity = 0.1f;

  private void Awake()
  {

  }

  // Start is called before the first frame update
  void Start()
  {
    //variable created to make code cleaner
    _cursor = _battleManager.Cursor;
  }

  // Update is called once per frame
  void Update()
  {
    ReadInput();
    HandleInput();
  }

  void HandleInput()
  {
    switch (InputState)
    {
      case InputState.Movement:
        _movementInput.HandleInput();
        break;
      case InputState.ActionMenu:
        _menuInput.HandleInput();
        break;
      default:
        Debug.LogError("Invalid InputState value");
        break;
    }
  }

  private void ReadInput()
  {
    SelectDown = Input.GetButtonDown("Select");
    CancelDown = Input.GetButtonDown("Cancel");
    float verticalAxis = Input.GetAxis("Vertical");

    DownDirection = false;
    UpDirection = false;

    if (verticalAxis > _controllerSensitivity)
    {
      if (_previousVertical <= _controllerSensitivity)
      {
        UpDirection = true;
      }
    } else if (verticalAxis < -_controllerSensitivity)
    {
      if (_previousVertical >= -_controllerSensitivity)
      {
        DownDirection = true;
      }
    }

    _previousVertical = verticalAxis;
  }
}
