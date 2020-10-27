using UnityEngine;

public class MovementInput : MonoBehaviour
{
  [SerializeField] private BattleManager _battleManager;
  [SerializeField] private InputManager _inputManager;
  [SerializeField] private ActionMenuInput _menuInput;
  private Cursor _cursor;

  private void Start()
  {
    _cursor = _battleManager.Cursor;
  }


  public void HandleInput()
  {
    if (_inputManager.SelectDown)
    {
      if (_cursor.UnitIsSelected)
      {
        if (_cursor.IsInRangeOfSelectedUnit())
        {
          if (!_cursor.HoveredTile.IsOccupied())
          {
            _inputManager.InputState = InputState.ActionMenu;
            _menuInput.PreviousTile = _cursor.SelectedUnit.OccupiedTile;
            _cursor.enabled = false;
            _cursor.SelectedUnit.Move(_cursor.HoveredTile);
            //TODO: show only viable buttons
            UIManager.ShowButtons(2, new string[] { "Move", "Attack" });
          }
          else
          {
            //TODO: unit tries to move on occupied tile
          }
         }
      }
      else if (_cursor.HoveredTile.IsOccupied())
      {
        if (_cursor.HoveredTile.Occupier.Group ==
          _battleManager.BattleTurnManager.ActiveGroup)
        {
          _cursor.SelectUnit();
        }
        else
        {
          //TODO: pressing select on unit from other group
        }
      }
    } 
    else if (_inputManager.CancelDown)
    {
      BattleMovementUtils.HidePath();
      BattleMovementUtils.ResetPath();
      _battleManager.OverlaysManager.DisableUnitOverlays();
      _cursor.DeselectUnit();
    }
  }
}

