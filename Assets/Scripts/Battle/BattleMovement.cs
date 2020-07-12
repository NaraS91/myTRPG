using System.Collections.Generic;

public class BattleMovement
{
  public ISet<Tile> UnitTiles { get; private set; } = new HashSet<Tile>();
  public ISet<Tile> AttackedTiles { get; private set; } = new HashSet<Tile>();
  public ISet<Unit> EnemyUnits { get; private set; } = new HashSet<Unit>();
  public ISet<Tile> EnemyTiles { get; private set; } = new HashSet<Tile>();

  private OverlaysManager _overlaysManager;

  public BattleMovement(OverlaysManager overlaysManager)
  {
    _overlaysManager = overlaysManager;
  }

  public void OnNewTile(Tile tile)
  {
    if (UnitTiles.Count > 0)
    {
      _overlaysManager.DisableOverlays(UnitTiles);
    }

    if (tile.IsOccupied())
    {
      UnitTiles = BattleMovementUtils.FindViableMoves(tile.Occupier);
      AttackedTiles = BattleMovementUtils.FindAttackedTiles(UnitTiles, 1, 1);
      _overlaysManager.EnableUnitOverlays();
      //TODO:....
      // EnableOverlays(_attackedTiles.Except(_unitTiles),
      //  _attackOverlayMaterial);
    }
  }
}
