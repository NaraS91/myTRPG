using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(BattleManager))]
public class InputManager : MonoBehaviour
{
  [SerializeField] private BattleManager _battleManager = null;
  private Cursor _cursor;
  private CameraMover _cameraMover;
  public bool MenuIsUp { get; private set; } = false;
  public EInputState InputState { get; set; } = EInputState.Movement;
  public bool SelectDown { get; private set; }
  public bool CancelDown { get; private set; }
  public bool DownDirection { get; private set; }
  public bool UpDirection { get; private set; }
  public bool RightDirection { get; private set; }
  public bool LeftDirection { get; private set; }

  private Unit _movingUnit;
 
  public ActionMenuInput ActionMenuInput { get; private set;}
  public MovementInput MovementInput { get; private set; }
  public UnitToAttackInput UnitToAttackInput { get; private set; }
  //if current input state ends without indicating next one,
  //current input state will be assigned to enxt on the stack
  private Stack<EInputState> _previousStates = new Stack<EInputState>();

  private void Awake()
  {
    ActionMenuInput = new ActionMenuInput();
    MovementInput = new MovementInput();
    UnitToAttackInput = new UnitToAttackInput();
  }

  // Start is called before the first frame update
  void Start()
  {
    _cameraMover = _battleManager.CameraMover;
    _cursor = _battleManager.Cursor;
    ActionMenuInput.SetupDependecies(_battleManager, this, UnitToAttackInput);
    MovementInput.SetupDependecies(_battleManager, this);
    UnitToAttackInput.SetupDependecies(_battleManager, this);
  }

  // Update is called once per frame
  void Update()
  {
    if (_movingUnit == null)
    {
      ReadInput();
      HandleInput();
    } 
    else if (!_movingUnit.isMoving)
    {
      _movingUnit = null;
      _cursor.SetCursorObjectState(true);
      ResetCamera();
      _battleManager.BattleTurnManager.DeactivateUnit(_cursor.SelectedUnit);
      _cursor.DeselectUnit();
    }
  }

  public void WaitForMovingUnit(Unit unit)
  {
    _movingUnit = unit;
  }

  void HandleInput()
  {
    switch (InputState)
    {
      case EInputState.Movement:
        MovementInput.HandleInput();
        break;
      case EInputState.ActionMenu:
        ActionMenuInput.HandleInput();
        break;
      case EInputState.SelectingUnitToAttack:
        UnitToAttackInput.HandleInput();
        break;
      default:
        Debug.LogError("Invalid InputState value");
        break;
    }
  }

  public void StopInput()
  {

  }

  public void GoToPreviousState()
  {
    InputState = _previousStates.Pop();
  }

  public void AddStateToHistory(EInputState inputState)
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
    DownDirection = false;
    UpDirection = false;

    if (Input.GetButtonDown("Vertical"))
    {
      if(Input.GetAxisRaw("Vertical") > 0)
      {
        UpDirection = true;
      } else
      {
        DownDirection = true;
      }
    } 
  }

  private void ReadHorizontalAxis()
  {
    LeftDirection = false;
    RightDirection = false;

    if (Input.GetButtonDown("Horizontal"))
    {
      if (Input.GetAxisRaw("Horizontal") > 0)
      {
        RightDirection = true;
      }
      else
      {
        LeftDirection = true;
      }
    }
  }

  public void ExecuteUnitMove()
  {
    _cursor.Hide();

    SetCameraOn(_cursor.SelectedUnit.gameObject);

    BattleMovementUtils.HidePath();
    _battleManager.OverlaysManager.DisableUnitOverlays();

    _cursor.SelectedUnit.StartMoveCoroutine(new LinkedList<Tile>(BattleMovementUtils.Path));
    
    WaitForMovingUnit(_cursor.SelectedUnit);
    BattleMovementUtils.ResetPath();
  }

  public void SetCameraOn(GameObject gameObject)
  {
    _cameraMover.ChangeTarget(gameObject);
  }

  public void ShowViableButtons()
  {
    List<string> viableButtons = new List<string>();

    viableButtons.Add("Move");

    if(_battleManager.BattleMovement.GetAttackedUnits().Count > 0)
    {
      viableButtons.Add("Attack");
    }

    UIManager.ShowButtons(viableButtons.Count, viableButtons.ToArray());
  }

  public void ResetCamera()
  {
    _cameraMover.ResetTarget();
  }
}
