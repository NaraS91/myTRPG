using System.Collections.Generic;
using UnityEngine;

public class UnitToAttackInput
{
  private BattleManager _battleManager;
  private InputManager _inputManager;
  private Cursor _cursor;
  private Unit _attackingUnit;
  private List<Unit> _enemiesInRange;
  private int _currentEnemy;
  private bool _setup = false;
  private bool _moving = false;

  public void SetupDependecies(BattleManager battleManager,
                               InputManager inputManager)
  {
    _cursor = battleManager.Cursor;
    _battleManager = battleManager;
    _inputManager = inputManager;
  }

  public void HandleInput()
  {
    if (!_setup)
    {
      Setup();
      _setup = true;
    }

    if (_moving)
    {
      if (_attackingUnit.isMoving)
        return;

      _moving = false;
      CombatManager.DefaultCombat(_attackingUnit,
                            _enemiesInRange[_currentEnemy]);
      _inputManager.ResetHistory();
      _inputManager.InputState = EInputState.Movement;
      _cursor.enabled = true;
      _cursor.SetCursorObjectState(true);
      _inputManager.ResetCamera();
      _battleManager.BattleTurnManager.DeactivateUnit(_attackingUnit);
      _cursor.DeselectUnit();
      _setup = false;
    } else if (_inputManager.SelectDown)
    {
      _cursor.SelectedUnit.MoveToPreviousPosition();
      _inputManager.ExecuteUnitMove();
      _moving = true;
    }
    else if (_inputManager.CancelDown)
    {
      UIManager.ShowPreviousButtons();
      _inputManager.GoToPreviousState();
      _inputManager.ResetCamera();
      _setup = false;
    }
    else if (Input.GetButtonDown("Change Enemy"))
    {
      if (Input.GetAxisRaw("Change Enemy") > 0)
      {
        _currentEnemy = (_currentEnemy + 1) % _enemiesInRange.Count;
      } 
      else
      {
        //keeps modulo positive
        _currentEnemy = ((_currentEnemy - 1) % _enemiesInRange.Count 
                        + _enemiesInRange.Count) % _enemiesInRange.Count;
      }

      CenterCameraOnEnemy();
    }
  }


  //calculates enemies in range, assigns attacking unit and set ups camera.
  private void Setup()
  {
    _attackingUnit = _battleManager.BattleMovement.CurrentUnit;
    SetEnemiesInRange(_battleManager.BattleMovement.GetAttackedUnits());
    CenterCameraOnEnemy();
  }


  public void SetEnemiesInRange(ICollection<Unit> enemiesInRange)
  {
    _enemiesInRange = new List<Unit>(enemiesInRange);
    _currentEnemy = 0;
  }


  private void CenterCameraOnEnemy()
  {
    _inputManager.SetCameraOn(_enemiesInRange[_currentEnemy].gameObject);
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
