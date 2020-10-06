using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{

  public Dictionary<int, ISet<Unit>> Groups;
  public int ActiveGroup { get; private set; }
  //groups in an alliance cant attack each other
  private HashSet<Tuple<int, int>> _alliances = new HashSet<Tuple<int, int>>();
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

  //Deactivated unit can no longer move this turn
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

  //chekcs if 2 fighting groups are in alliance.
  public bool InAlliance(int group1, int group2)
  {
    Tuple<int, int> pair1 = new Tuple<int, int>(group1, group2);
    Tuple<int, int> pair2 = new Tuple<int, int>(group2, group1);
    return _alliances.Contains(pair1) || _alliances.Contains(pair2);
  }

  private void BeginNextTurn()
  {
    ActiveGroup = (ActiveGroup + 1) % _numberOfGroups;
    _numberOfActiveUnits = 0;
    foreach (Unit unit in Groups[ActiveGroup])
    {
      if (unit.Health != 0)
      {
        unit.Selectable = true;
        _numberOfActiveUnits++;
      }
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
