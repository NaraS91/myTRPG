using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
  public int Speed;
  public GameObject Overlay;
  public Tile HoveredTile;
  public Unit SelectedUnit;
  private bool _hoverOverNewTile = true;
  public bool UnitIsSelected { get; private set; } = false;

  public Material rangeOverlayMaterial;
  private ISet<Tile> _rangeTiles = new HashSet<Tile>();

  // Start is called before the first frame update
  void Start()
  {
  }

  private void FixedUpdate()
  {
    UpdateSelectedTile();
  }

  // Update is called once per frame
  void Update()
  {
    MoveCursor();
    if (_hoverOverNewTile)
    {
      if (UnitIsSelected)
      {
        if (_rangeTiles.Contains(HoveredTile))
        {
          BattleMovement.HidePath();
          BattleMovement.AddTile(HoveredTile, SelectedUnit);
          BattleMovement.ShowPath();
        }
      }
      else
      {
        ShowUnitsRange();
      }

      _hoverOverNewTile = false;
    }

  }

  public void SelectUnit()
  {
    if (HoveredTile.Occupier.Selectable) 
    { 
      SelectedUnit = HoveredTile.Occupier;
      UnitIsSelected = true;
    }
  }

  public bool IsInRangeOfSelectedUnit()
  {
    return _rangeTiles.Contains(HoveredTile);
  }
  public void DeselectUnit()
  {
    UnitIsSelected = false;
    SelectedUnit = null;
  }

  private void ShowUnitsRange()
  {
    if (_rangeTiles.Count > 0)
    {
      DisableOverlays();
    }

    if (HoveredTile.IsOccupied())
    {
      _rangeTiles = BattleMovement.FindViableMoves(HoveredTile.Occupier);
      EnableOverlays();
    }
  }

  public void DisableOverlays()
  {
    foreach (Tile tile in _rangeTiles)
    {
      tile.Overlay.SetActive(false);
    }

    _rangeTiles.Clear();
  }

  private void EnableOverlays()
  {
    foreach(Tile tile in _rangeTiles)
    {
      tile.OverlayMeshRenderer.material = rangeOverlayMaterial;
      tile.Overlay.SetActive(true);
    }
  }

  //sets selected tile to match cursor, and updates _hoverOverNewTile
  void UpdateSelectedTile()
  {
    Collider[] colliders = Physics.OverlapSphere(new Vector3
      (transform.position.x, 0.0f, transform.position.z),
      0.0f, BattleManager.TILES_LAYER);
    _hoverOverNewTile = false;

    foreach (Collider collider in colliders)
    {
      if (collider.gameObject.CompareTag("Tile"))
      {
        if (HoveredTile != collider.gameObject.GetComponent<Tile>())
        {
          HoveredTile = collider.gameObject.GetComponent<Tile>();
          Overlay.transform.position = HoveredTile.Overlay.transform.position;
          _hoverOverNewTile = true;
        }
      }
    }
  }

  private void MoveCursor()
  {
    float vertical = Input.GetAxis("Vertical") * Speed;
    float horizontal = Input.GetAxis("Horizontal") * Speed;
    transform.Translate
      (horizontal * Time.deltaTime, 0, vertical * Time.deltaTime);
  }
}
