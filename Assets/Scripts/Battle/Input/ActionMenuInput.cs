using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionMenuInput
{
  private BattleManager _battleManager;
  private InputManager _inputManager;
  private UnitToAttackInput _unitToAttackInput;
  private Cursor _cursor;
  private bool _moving = false;

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
    if (_moving)
    {
      if (_cursor.SelectedUnit.isMoving)
        return;

      _battleManager.BattleTurnManager.DeactivateUnit(_cursor.SelectedUnit);
      _inputManager.InputState = EInputState.Movement;
      _cursor.DeselectUnit();
      _cursor.SetCursorObjectState(true);
      _cursor.enabled = true;
      _inputManager.ResetCamera();
      _moving = false;
    }
    else if (_inputManager.SelectDown)
    {
      switch (UIManager.ActiveButtonType())
      {
        case EButtonType.Move:
          _cursor.SelectedUnit.MoveToPreviousPosition();
          _inputManager.ExecuteUnitMove();
          UIManager.HideButtons();
          _moving = true;
          break;
        case EButtonType.Attack:
          _inputManager.AddStateToHistory(EInputState.ActionMenu);
          UIManager.HideButtons();
          _inputManager.InputState = EInputState.SelectingUnitToAttack;
         // _unitToAttackInput.Setup();
          break;
        default:
          Debug.LogError("Unrecognized Button type");
          break;
      }

    } else if (_inputManager.CancelDown)
    {
      _cursor.SelectedUnit.MoveToPreviousPosition();
      UIManager.HideButtons();
      _inputManager.InputState = EInputState.Movement;
      _cursor.enabled = true;
    } else if (_inputManager.DownDirection)
    {
      UIManager.NextButton();
    } else if (_inputManager.UpDirection)
    {
      UIManager.PreviousButton();
    }
  }
}
