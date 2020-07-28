using UnityEngine;

public class ActionMenuInput : MonoBehaviour
{
  [SerializeField] private BattleManager _battleManager;
  [SerializeField] private InputManager _inputManager;
  [SerializeField] private UnitToAttackInput _unitToAttackInput;
  public Tile PreviousTile {get; set;}
  private Cursor _cursor;

  private void Start()
  {
    _cursor = _battleManager.Cursor;
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
