using UnityEngine;

[RequireComponent(typeof(BattleManager))]
public class BattleInput : MonoBehaviour
{
  private BattleManager _battleManager;
  private Cursor _cursor;
  public bool MenuIsUp { get; private set; } = false;

  private void Awake()
  {
    _battleManager = GetComponent<BattleManager>();
  }

  // Start is called before the first frame update
  void Start()
  {
    //variable created to make code cleaner
    _cursor = _battleManager.Cursor;
  }

  // Update is called once per frame
  void Update()
  {
    HandleInput();
  }

  void HandleInput()
  {
    if (Input.GetButtonDown("Select"))
    {
      if (!MenuIsUp)
      {
        if (_battleManager.Cursor.UnitIsSelected)
        {
          if (_cursor.IsInRangeOfSelectedUnit())
          {
            _cursor.SelectedUnit.Move(_cursor.HoveredTile);
            _battleManager.BattleTurnManager.DeactivateUnit(_cursor.SelectedUnit);
            BattleMovement.HidePath();
            BattleMovement.ResetPath();
            _battleManager.OverlaysManager.DisableAllOverlays();
            _cursor.DeselectUnit();
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
        else
        {
          _cursor.enabled = false;
          MenuIsUp = true;
          ShowUnitMenu();
        }
      }
      else
      {
        //TODO: ui management
      }
    }
    else if (Input.GetButtonDown("Cancel"))
    {

    }
  }

  private void ShowUnitMenu()
  {
    //TODO: implement with ui
  }
}
