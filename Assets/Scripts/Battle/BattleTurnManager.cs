using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{

  public Dictionary<int, ISet<Unit>> Groups;
  public int ActiveGroup { get; private set; }
  private int _numberOfGroups = 0;
  private int _numberOfActiveUnits;
  private BattleManager _battleManager;

  private void Awake()
  {
    _battleManager = GetComponent<BattleManager>();
    Groups = new Dictionary<int, ISet<Unit>>();
  }

  // Start is called before the first frame update
  void Start()
   {
    GameObject[] units 
      = GameObject.FindGameObjectsWithTag(BattleManager.UNIT_TAG);
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

  // Update is called once per frame
  void Update()
  {
    if (_numberOfActiveUnits == 0)
    {
      BeginNextTurn();
    }
  }

  public void DeactivateUnit(Unit unit)
  {
    if (unit.Group == ActiveGroup && unit.Selectable)
    {
      unit.Selectable = false;
      _numberOfActiveUnits--;
    }
    else
    {
      throw new InvalidOperationException(@"unit is not in the active group
                                          or is not selectable");
    }
  }

  private void BeginNextTurn()
  {
    ActiveGroup = (ActiveGroup + 1) % _numberOfGroups;
    _numberOfActiveUnits = 0;
    foreach (Unit unit in Groups[ActiveGroup])
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
