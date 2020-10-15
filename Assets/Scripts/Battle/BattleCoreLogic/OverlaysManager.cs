using System.Collections.Generic;
using UnityEngine;

public class OverlaysManager
{
  private BattleMovement _battleMovement;

  private static Material _rangeOverlayMaterial;
  private static Material _enemiesOverlayMaterial;
  private static Material _attackOverlayMaterial;

  private static bool _overlaysSetUp = false;

  //loads materials and assigns battleMovement
  public void SetUp(BattleMovement battleMovement)
  {
    if (!_overlaysSetUp)
    {
      _overlaysSetUp = true;
      _rangeOverlayMaterial
        = Resources.Load("Materials/UnitRangeOverlayMaterial") as Material;
      _enemiesOverlayMaterial
        = Resources.Load("Materials/EnemiesOverlayMaterial") as Material;
      _attackOverlayMaterial
       = Resources.Load("Materials/AttackOverlayMaterial") as Material;
    }

    _battleMovement = battleMovement;
  }

  public void DisableUnitOverlays()
  {
    DisableOverlays(_battleMovement.UnitTiles);
    DisableOverlays(_battleMovement.AttackedTiles);
  }

  public void DisableAllOverlays()
  {
    DisableOverlays(_battleMovement.EnemyTiles);
    DisableOverlays(_battleMovement.UnitTiles);
    _battleMovement.EnemyUnits.Clear();
  }

  //disables overlays corresponding to the given tiles
  private void DisableOverlays(ISet<Tile> tiles)
  {
    foreach (Tile tile in tiles)
    {
      tile.Overlay.SetActive(false);
    }

    tiles.Clear();
  }

  //displays overlays
  public void EnableUnitOverlays()
  {

    foreach (Tile tile in _battleMovement.UnitTiles)
    {
      tile.OverlayMeshRenderer.material = _rangeOverlayMaterial;
      tile.Overlay.SetActive(true);
    }

    foreach (Tile tile in _battleMovement.AttackedTiles)
    {
      if(!tile.Overlay.activeSelf ||
        tile.OverlayMeshRenderer.material.Equals(_rangeOverlayMaterial))
      {
        tile.OverlayMeshRenderer.material = _attackOverlayMaterial;
        tile.Overlay.SetActive(true);
      }
    }
  }
}
