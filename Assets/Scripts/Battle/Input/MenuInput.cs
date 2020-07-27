using UnityEngine;

public class MenuInput : MonoBehaviour
{
  [SerializeField] private BattleManager _battleManager;
  [SerializeField] private InputManager _inputManager;
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
          MoveUnit();
          break;
        case EButtonType.Attack:
          Debug.LogError("Attack is not implemented yet");
          break;
        default:
          Debug.LogError("Unrecognized Button type");
          break;
      }

      UIManager.HideButtons();
      _inputManager.InputState = InputState.Movement;
      _cursor.enabled = true;
    } else if (_inputManager.CancelDown)
    {
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

  private void MoveUnit()
  {
    _cursor.SelectedUnit.Move(_cursor.HoveredTile);
    _battleManager.BattleTurnManager.DeactivateUnit(_cursor.SelectedUnit);
    BattleMovementUtils.HidePath();
    BattleMovementUtils.ResetPath();
    _battleManager.OverlaysManager.DisableUnitOverlays();
    _cursor.DeselectUnit();
  }
}
