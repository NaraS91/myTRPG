using UnityEngine;

public class MovementInput
{
  private BattleManager _battleManager;
  private InputManager _inputManager;
  private Cursor _cursor;

  public void SetupDependecies
    (BattleManager battleManager, InputManager inputManager)
  {
    _battleManager = battleManager;
    _cursor = _battleManager.Cursor;
    _inputManager = inputManager;
  }


  public void HandleInput()
  {
    if (_inputManager.SelectDown)
    {
      if (_cursor.UnitIsSelected)
      {
        if (_cursor.IsInRangeOfSelectedUnit())
        {
          if (!_cursor.HoveredTile.IsOccupied() || 
              _cursor.HoveredTile.Occupier.Equals(_cursor.SelectedUnit))
          {
            _inputManager.InputState = InputState.ActionMenu;
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
        bool isUnitFromTheACtiveGroup = _cursor.HoveredTile.Occupier.Group ==
                                  _battleManager.BattleTurnManager.ActiveGroup;
        if (isUnitFromTheACtiveGroup)
        {
          _cursor.SelectUnit();
          _battleManager.BattleMovement.OnNewTile(_cursor.HoveredTile);
        }
        else
        {
          //TODO: pressing select on unit from other group
        }
      }
    } 
    else if (_inputManager.CancelDown)
    {
      if (_cursor.UnitIsSelected)
      {
        BattleMovementUtils.HidePath();
        BattleMovementUtils.ResetPath();
        _cursor.DeselectUnit();
      }

      _battleManager.OverlaysManager.DisableUnitOverlays();
    }
  }
}

