using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class BattleManager : MonoBehaviour
{
  public const int TILES_LAYER = 1 << 8;
  public const string UNIT_TAG = "Unit";

  public Dictionary<int, ISet<Unit>> Groups;
  public int ActiveGroup;
  public static GameObject DefaultOverlay;
  public GameObject CursorObject;
  private int _numberOfGroups = 0;
  private int _numberOfActiveUnits;
  private Cursor _cursor;
  public bool MenuIsUp { get; private set; } = false;

  private void Awake()
  {
    _cursor = CursorObject.GetComponent<Cursor>();
    DefaultOverlay = _cursor.Overlay;

    Groups = new Dictionary<int, ISet<Unit>>();
  }

  // Start is called before the first frame update
  void Start()
  {
    GameObject[] units = GameObject.FindGameObjectsWithTag(UNIT_TAG);
    foreach (GameObject unitGO in units)
    {
      Unit unit = unitGO.GetComponent<Unit>();
      if (!Groups.ContainsKey(unit.Group))
      {
        Groups.Add(unit.Group, new HashSet<Unit>());
        _numberOfGroups++;
      }
      Groups[unit.Group].Add(unit);
    }

    ActiveGroup = -1;
    BeginNextTurn();
  }

  private void FixedUpdate()
  {
  }

  // Update is called once per frame
  void Update()
  {
    HandleInput();
    if(_numberOfActiveUnits == 0)
    {
      BeginNextTurn();
    }
    
  }

  void ShowUnitMenu()
  {

  }

  void HandleInput()
  {
    if (Input.GetButtonDown("Select"))
    {
      if (!MenuIsUp)
      {
        if (_cursor.UnitIsSelected)
        {
          if (_cursor.IsInRangeOfSelectedUnit())
          {
            _cursor.SelectedUnit.Move(_cursor.HoveredTile);
            _cursor.SelectedUnit.Selectable = false;
            _numberOfActiveUnits--;
            BattleMovement.HidePath();
            _cursor.DisableOverlays();
            _cursor.DeselectUnit();
          }
        }
        else if (_cursor.HoveredTile.IsOccupied())
        {
          if (_cursor.HoveredTile.Occupier.Group == ActiveGroup)
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
      } else
      {
        //TODO: ui management
      }
    }
  }

  private void BeginNextTurn()
  {
    ActiveGroup = (ActiveGroup + 1) % _numberOfGroups;
    _numberOfActiveUnits = 0;
    foreach(Unit unit in Groups[ActiveGroup])
    {
      unit.Selectable = true;
      _numberOfActiveUnits++;
    }
  }

  private void EndTurn()
  {
    foreach (Unit unit in Groups[ActiveGroup])
    {
      unit.Selectable = false;
    }
  }

}
