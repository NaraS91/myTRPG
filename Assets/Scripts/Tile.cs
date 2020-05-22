using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{

  public bool Walkable { get; set; } = true;
  public bool Flyable { get; set; } = true;
  public bool Current { get; set; } = false;
  public int Cost { get; } = 1;
  [SerializeField] private GameObject _occupier;
  private Tile _forwardTile;
  private Tile _rightTile;
  private Tile _leftTile;
  private Tile _backTile;

  // Start is called before the first frame update
  void Start()
  {
    FindAdjacentTiles();
  }

  //some tiles may be null
  public Tile[] GetAdjacentTiles()
  {
    return new Tile[4] {_forwardTile, _rightTile, _backTile, _leftTile}
      .Where(t => t != null).ToArray();
  }

  public GameObject GetOccupier()
  {
    return _occupier;
  }

  // return false if tile is already occupied by other object
  public bool SetOccupier(GameObject occupier)
  {
    if (IsOccupied() && this._occupier != occupier)
    {
      return false;
    }

    this._occupier = occupier;
    return true;
  }

  public void RemoveOccupier()
  {
    _occupier = null;
  }

  public bool IsOccupied()
  {
    return _occupier != null;
  }

  private void FindAdjacentTiles()
  {
    Collider[] adjacentColliders
      = Physics.OverlapSphere(transform.position, transform.localScale.x * 1.1f);

    foreach (Collider collider in adjacentColliders)
    {
      if (collider.gameObject != null && collider.gameObject.CompareTag("Tile"))
      {
        GameObject tile = collider.gameObject;
        float deltaX = tile.transform.position.x - transform.position.x;
        float deltaZ = tile.transform.position.z - transform.position.z;
        switch (deltaX * 3 + deltaZ)
        {
          case 3:
            _rightTile = tile.GetComponent<Tile>(); break;
          case -3:
            _leftTile = tile.GetComponent<Tile>(); break;
          case 1:
            _forwardTile = tile.GetComponent<Tile>(); break;
          case -1:
            _backTile = tile.GetComponent<Tile>(); break;
          default:
            break;
        }

      }
    }
  }
}
