using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class BattleMovement
{
  public ISet<Tile> UnitTiles { get; private set; } = new HashSet<Tile>();
  public ISet<Tile> AttackedTiles { get; private set; } = new HashSet<Tile>();
  public ISet<Unit> EnemyUnits { get; private set; } = new HashSet<Unit>();
  public ISet<Tile> EnemyTiles { get; private set; } = new HashSet<Tile>();
  public Unit CurrentUnit { get; private set; }

  private OverlaysManager _overlaysManager;
  private BattleTurnManager _battleTurnManager;

  public BattleMovement(OverlaysManager overlaysManager,
                        BattleTurnManager battleTurnManager)
  {
    _overlaysManager = overlaysManager;
    _battleTurnManager = battleTurnManager;
  }

  public void OnNewTile(Tile tile)
  {
    CurrentUnit = null;

    if (UnitTiles.Count > 0)
    {
      _overlaysManager.DisableUnitOverlays();
    }

    if (tile.IsOccupied())
    {
      CurrentUnit = tile.Occupier;
      UnitTiles = BattleMovementUtils.FindViableMoves(CurrentUnit);
      AttackedTiles = BattleMovementUtils.FindAttackedTiles(UnitTiles, 2, 2);
      _overlaysManager.EnableUnitOverlays();
    }
  }

  public ICollection<Unit> GetAttackedUnits()
  {
    if(CurrentUnit == null)
    {
      UnityEngine.Debug.LogError("attacking unit is not assigned");
      return null;
    }

    HashSet<Unit> attackedUnits = new HashSet<Unit>();
    int group = CurrentUnit.Group;

    ISet<Tile> attackedTiles = BattleMovementUtils.FindAttackedTiles
      (new HashSet<Tile> { CurrentUnit.OccupiedTile }, 1, 2);

    foreach(Tile tile in attackedTiles)
    {
      if (tile.IsOccupied())
      {
        int enemyGroup = tile.Occupier.Group;
        if (group != enemyGroup &&
            !_battleTurnManager.InAlliance(group, enemyGroup))
        {
          attackedUnits.Add(tile.Occupier);
        }
      }
    }

    return attackedUnits;
  }
}
