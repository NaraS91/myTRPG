using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
  public const int TILES_LAYER = 1 << 8;
  public Dictionary<string, ISet<Unit>> Groups;
  public List<string> TurnQueue;
  public int ActiveGroup;
  public GameObject CursorObject;
  private Cursor Cursor;
  public bool Selected = false;

  // Start is called before the first frame update
  void Start()
  {
    Cursor = CursorObject.GetComponent<Cursor>();
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
      if (!Selected)
      {
        Cursor.enabled = false;
        Selected = true;
        ShowUnitMenu();
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
