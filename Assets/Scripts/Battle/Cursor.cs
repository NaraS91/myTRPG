using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
  public int Speed;
  public GameObject Overlay;
  public Tile HoveredTile;
  public Tile SelectedTile;
  private bool _newSelectedTile;

  public GameObject rangeOverlay;
  private List<GameObject> _unitRangeOverlays = new List<GameObject>();
  private int _activeOverlays = 0;

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
    moveCursor();
    if (_newSelectedTile)
    {
      if(_activeOverlays > 0)
      {
        disableOverlays();
      }

      if (SelectedTile.IsOccupied())
      {
        ISet<Tile> unitRangeTiles
          = BattleMovement.FindViableMoves(SelectedTile.Occupier);
        enableOverlays(unitRangeTiles);
      }

      _newSelectedTile = false;
    }

  }

  private void disableOverlays()
  {
    for (int i = 0; i < _activeOverlays; i++)
    {
      _unitRangeOverlays[i].SetActive(false);
    }
  }

  private void enableOverlays(ISet<Tile> tiles)
  {
    int diff = tiles.Count - _unitRangeOverlays.Count;
    for(int i = 0; i < diff; i++)
    {
      _unitRangeOverlays.Add(Instantiate(rangeOverlay));
    }

    int j = 0;
    foreach(Tile tile in tiles)
    {
      _unitRangeOverlays[j].transform.position = tile.transform.position;
      _unitRangeOverlays[j].SetActive(true);
      j++;
    }

    _activeOverlays = tiles.Count;
  }

  //sets selected tile to match cursor, returns true if selectedTile changed
  void UpdateSelectedTile()
  {
    Collider[] colliders = Physics.OverlapSphere(new Vector3
      (transform.position.x, 0.0f, transform.position.z),
      0.0f, BattleManager.TILES_LAYER);
    foreach (Collider collider in colliders)
    {
      if (collider.gameObject.CompareTag("Tile"))
      {
        if (SelectedTile != collider.gameObject.GetComponent<Tile>())
        {
          SelectedTile = collider.gameObject.GetComponent<Tile>();
          Overlay.transform.position = SelectedTile.transform.position;
          _newSelectedTile = true;
        }
      }
    }
  }

  private void moveCursor()
  {
    float vertical = Input.GetAxis("Vertical") * Speed;
    float horizontal = Input.GetAxis("Horizontal") * Speed;
    transform.Translate
      (horizontal * Time.deltaTime, 0, vertical * Time.deltaTime);
  }
}
