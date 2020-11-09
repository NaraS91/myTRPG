using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionMenuInput
{
  private BattleManager _battleManager;
  private InputManager _inputManager;
  private UnitToAttackInput _unitToAttackInput;
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
          _cursor.SelectedUnit.MoveToPreviousPosition();
          ExecuteUnitMove();
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
      _cursor.SelectedUnit.MoveToPreviousPosition();
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

  private void ExecuteUnitMove()
  {
    BattleMovementUtils.HidePath();
    _battleManager.OverlaysManager.DisableUnitOverlays();
    _cursor.SelectedUnit.StartMoveCoroutine(new LinkedList<Tile>(BattleMovementUtils.Path));
    _inputManager.WaitForMovingUnit(_cursor.SelectedUnit);
    BattleMovementUtils.ResetPath();
    _battleManager.BattleTurnManager.DeactivateUnit(_cursor.SelectedUnit);
    _cursor.DeselectUnit();
  }
}
