using System.Collections.Generic;
using UnityEngine;

public class UnitToAttackInput : MonoBehaviour
{
  [SerializeField] private BattleManager _battleManager;
  [SerializeField] private InputManager _inputManager;
  [SerializeField] private CameraMover _cameraMover;
  private Cursor _cursor;
  private Unit _attackingUnit;
  private List<Unit> _enemiesInRange;
  private int _currentEnemy;
  private GameObject _cameraGO;

  private void Awake()
  {
    _cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
    if(_cameraGO == null)
    {
      Debug.LogError("Main Camera wasnt found");
    }

    GameObject cursorGO = GameObject.FindGameObjectWithTag("Cursor");
    if(cursorGO == null)
    {
      Debug.LogError("Cursor wasnt found");
    }

    _cursor = cursorGO.GetComponent<Cursor>();
  }

  public void HandleInput()
  {
    if (_inputManager.SelectDown)
    {
      CombatManager.DefaultCombat(_attackingUnit,
                                  _enemiesInRange[_currentEnemy]);
      FinishUnitMove();
      _cameraMover.ResetTarget();
      _inputManager.ResetHistory();
      _inputManager.InputState = InputState.Movement;
      _cursor.enabled = true;

    } else if (_inputManager.CancelDown)
    {
      UIManager.ShowPreviousButtons();
      _inputManager.GoToPreviousState();
      _cameraMover.ResetTarget();
    } else if (_inputManager.LeftDirection)
    {
      if(_currentEnemy > 0)
      {
        _currentEnemy--;
        CenterCameraOnEnemy();
      }
    } else if (_inputManager.RightDirection)
    {
      if(_currentEnemy < _enemiesInRange.Count - 1)
      {
        _currentEnemy++;
        CenterCameraOnEnemy();
      }
    }
  }


  //use before transitioning from other input state.
  //calculates enemies in range, assigns attacking unit and set ups camera.
  public void Setup()
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
    _cameraMover.ChangeTarget(_enemiesInRange[_currentEnemy].gameObject);
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
