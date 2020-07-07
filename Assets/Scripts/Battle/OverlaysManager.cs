using System.Collections.Generic;
using UnityEngine;

public class OverlaysManager
{
  private ISet<Unit> _enemyUnits = new HashSet<Unit>();
  private ISet<Tile> _enemyTiles = new HashSet<Tile>();
  private ISet<Tile> _unitTiles = new HashSet<Tile>();

  private static Material _rangeOverlayMaterial;
  private static Material _enemiesOverlayMaterial;
  private static Material _attackOverlayMaterial;

  private static bool _overlaysSetUp = false;

  public void LoadMaterials()
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
  }

  public void OnNewTile(Tile tile)
  {
    if (_unitTiles.Count > 0)
    {
      DisableOverlays(_unitTiles);
    }

    if (tile.IsOccupied())
    {
      _unitTiles = BattleMovement.FindViableMoves(tile.Occupier);
      EnableOverlays(_unitTiles, _rangeOverlayMaterial);
    }
  }

  public void DisableAllOverlays()
  {
    DisableOverlays(_enemyTiles);
    DisableOverlays(_unitTiles);
    _enemyUnits.Clear();
  }

  public bool UnitTilesContain(Tile tile)
  {
    return _unitTiles.Contains(tile);
  }

  private void DisableOverlays(ISet<Tile> tiles)
  {
    foreach (Tile tile in tiles)
    {
      tile.Overlay.SetActive(false);
    }

    tiles.Clear();
  }

  private void EnableOverlays(ISet<Tile> tiles, Material material)
  {
    foreach (Tile tile in tiles)
    {
      tile.OverlayMeshRenderer.material = material;
      tile.Overlay.SetActive(true);
    }
  }
}
