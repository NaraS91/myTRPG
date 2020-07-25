using UnityEngine;

public class MovementInput : MonoBehaviour
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
      if (_cursor.UnitIsSelected)
      {
        if (_cursor.IsInRangeOfSelectedUnit())
        {
         // uncomment to see buttons, they still dont work tho
         // UIManager.ShowButtons(2, new string[]{"Move", "Attack"});
         MoveUnit();
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

