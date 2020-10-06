using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(BattleManager))]
public class InputManager : MonoBehaviour
{
  [SerializeField] private BattleManager _battleManager;
  [SerializeField] private MovementInput _movementInput;
  [SerializeField] private ActionMenuInput _menuInput;
  [SerializeField] private UnitToAttackInput _unitToAttackInput;
  private Cursor _cursor;
  public bool MenuIsUp { get; private set; } = false;
  public InputState InputState { get; set; } = InputState.Movement;
  public bool SelectDown { get; private set; }
  public bool CancelDown { get; private set; }
  public bool DownDirection { get; private set; }
  public bool UpDirection { get; private set; }
  public bool RightDirection { get; private set; }
  public bool LeftDirection { get; private set; }
  private float _previousVertical;
  private float _previousHorizontal;
  private const float _controllerSensitivity = 0.1f;
  //if current input state ends without indicating next one,
  //current input state will be assigned to enxt on the stack
  private Stack<InputState> _previousStates = new Stack<InputState>();

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
      case InputState.SelectingUnitToAttack:
        _unitToAttackInput.HandleInput();
        break;
      default:
        Debug.LogError("Invalid InputState value");
        break;
    }
  }

  public void GoToPreviousState()
  {
    InputState = _previousStates.Pop();
  }

  public void AddStateToHistory(InputState inputState)
  {
    _previousStates.Push(inputState);
  }

  public void ResetHistory()
  {
    _previousStates.Clear();
  }

  private void ReadInput()
  {
    SelectDown = Input.GetButtonDown("Select");
    CancelDown = Input.GetButtonDown("Cancel");
    ReadHorizontalAxis();
    ReadVerticalAxis();
  }

  //TODO: simplify, see "change enemy" button in UnitToAttackInput
  private void ReadVerticalAxis()
  {
    float verticalAxis = Input.GetAxis("Vertical");

    DownDirection = false;
    UpDirection = false;

    if (verticalAxis > _controllerSensitivity)
    {
      if (_previousVertical <= _controllerSensitivity)
      {
        UpDirection = true;
      }
    }
    else if (verticalAxis < -_controllerSensitivity)
    {
      if (_previousVertical >= -_controllerSensitivity)
      {
        DownDirection = true;
      }
    }

    _previousVertical = verticalAxis;
  }

  //TODO: simplify, see "change enemy" button in UnitToAttackInput
  private void ReadHorizontalAxis()
  {
    float horizontalAxis = Input.GetAxis("Horizontal");

    LeftDirection = false;
    RightDirection = false;

    if (horizontalAxis > _controllerSensitivity)
    {
      if (_previousHorizontal <= _controllerSensitivity)
      {
        RightDirection = true;
      }
    }
    else if (horizontalAxis < -_controllerSensitivity)
    {
      if (_previousHorizontal >= -_controllerSensitivity)
      {
        LeftDirection = true;
      }
    }

    _previousHorizontal = horizontalAxis;
  }
}
