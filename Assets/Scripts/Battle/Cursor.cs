using UnityEngine;

public class Cursor : MonoBehaviour
{
  public int Speed;
  public GameObject Overlay;
  public Tile HoveredTile;
  public Unit SelectedUnit;
  private BattleManager _battleManager;
  private bool _hoverOverNewTile = true;
  public bool UnitIsSelected { get; private set; } = false;


  // Start is called before the first frame update
  void Start()
  {
    _battleManager = GameObject.FindGameObjectWithTag("BattleManager")
      .GetComponent<BattleManager>();
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
        if (_battleManager.BattleMovement.UnitTiles.Contains(HoveredTile))
        {
          BattleMovementUtils.HidePath();
          BattleMovementUtils.AddTile(HoveredTile, SelectedUnit);
          BattleMovementUtils.ShowPath();
        }
      }
      else
      {
        _battleManager.BattleMovement.OnNewTile(HoveredTile);
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
    return _battleManager.BattleMovement.UnitTiles.Contains(HoveredTile);
  }

  public void DeselectUnit()
  {
    UnitIsSelected = false;
    SelectedUnit = null;
  }

  //sets selected tile to match cursor, and updates _hoverOverNewTile
  void UpdateSelectedTile()
  {
    Collider[] colliders = Physics.OverlapSphere(new Vector3
      (transform.position.x, 0.0f, transform.position.z),
      0.0f, BattleManager.TILES_LAYER);

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
