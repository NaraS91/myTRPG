using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager : MonoBehaviour
{
  public const int TILES_LAYER = 1 << 8;
  public Dictionary<string, ISet<Unit>> Groups;
  public List<string> TurnQueue;
  public int ActiveGroup;
  public static GameObject DefaultOverlay;
  public GameObject CursorObject;
  private Cursor _cursor;
  public bool MenuIsUp { get; private set; } = false;

  private void Awake()
  {
    _cursor = CursorObject.GetComponent<Cursor>();
    DefaultOverlay = _cursor.Overlay;
  }

  // Start is called before the first frame update
  void Start()
  {
    ActiveGroup = -1;

   // BeginNextTurn();
  }

  private void FixedUpdate()
  {
  }

  // Update is called once per frame
  void Update()
  {
    HandleInput();
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

        }
        else if (_cursor.HoveredTile.IsOccupied())
        {
          _cursor.SelectUnit();
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
    ActiveGroup = (ActiveGroup + 1) % TurnQueue.Count;
    foreach(Unit unit in Groups[TurnQueue[ActiveGroup]])
    {
      unit.Selectable = true;
    }
  }

  private void EndTurn()
  {
    foreach (Unit unit in Groups[TurnQueue[ActiveGroup]])
    {
      unit.Selectable = false;
    }
  }

}
