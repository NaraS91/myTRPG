using UnityEngine;

public class ActionMenuInput
{
  private BattleManager _battleManager;
  private InputManager _inputManager;
  private UnitToAttackInput _unitToAttackInput;
  public Tile PreviousTile {get; set;}
  private Cursor _cursor;

  //call before using this class
  public void SetupDependecies
    (BattleManager battleManager, InputManager inputManager,
     UnitToAttackInput unitToAttackInput)
  {
    _cursor = battleManager.Cursor;
    _battleManager = battleManager;
    _inputManager = inputManager;
    _unitToAttackInput = unitToAttackInput;
  }

  public void HandleInput()
  {
    if (_inputManager.SelectDown)
    {
      switch (UIManager.ActiveButtonType())
      {
        case EButtonType.Move:
          FinishUnitMove();
          UIManager.HideButtons();
          _inputManager.InputState = InputState.Movement;
          _cursor.enabled = true;
          break;
        case EButtonType.Attack:
          _inputManager.AddStateToHistory(InputState.ActionMenu);
          UIManager.HideButtons();
          _inputManager.InputState = InputState.SelectingUnitToAttack;
          _unitToAttackInput.Setup();
          break;
        default:
          Debug.LogError("Unrecognized Button type");
          break;
      }

    } else if (_inputManager.CancelDown)
    {
      _cursor.SelectedUnit.Move(PreviousTile);
      UIManager.HideButtons();
      _inputManager.InputState = InputState.Movement;
      _cursor.enabled = true;
    } else if (_inputManager.DownDirection)
    {
      UIManager.NextButton();
    } else if (_inputManager.UpDirection)
    {
      UIManager.PreviousButton();
    }
  }

  private void FinishUnitMove()
  {
    BattleMovementUtils.HidePath();
    BattleMovementUtils.ResetPath();
    _battleManager.OverlaysManager.DisableUnitOverlays();
    _battleManager.BattleTurnManager.DeactivateUnit(_cursor.SelectedUnit);
    _cursor.DeselectUnit();
  }
}
