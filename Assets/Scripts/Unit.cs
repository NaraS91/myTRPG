using UnityEngine;

public class Unit : MonoBehaviour
{
  [SerializeField] private int _health = 50;
  [SerializeField] private int _movement = 1;
  [SerializeField] private bool _flyier = false;
  [SerializeField] private Tile _occupiedTile;
  [SerializeField] private int _group;

  // Start is called before the first frame update
  void Start()
  {
    UpdateTile();
  }

  public Tile GetOccupiedTile()
  {
    return _occupiedTile;
  }

  public int GetMovement()
  {
    return _movement;
  }

  public int GetGroup()
  {
    return _group;
  }

  public void FreeTile()
  {
    _occupiedTile.RemoveOccupier();
    _occupiedTile = null;
  } 

  //frees tile currently occupied and sets occupied tile to correct tile
  public bool UpdateTile()
  {
    Collider[] colliders = Physics.OverlapSphere(
      new Vector3(transform.position.x, 0.0f, transform.position.z), 0.0f);

    foreach (Collider collider in colliders)
    {
      if(collider.gameObject != null && collider.gameObject.CompareTag("Tile"))
      {
        if (collider.gameObject.GetComponent<Tile>().SetOccupier(this.gameObject))
        {
          if(_occupiedTile != null) FreeTile();
          _occupiedTile = collider.gameObject.GetComponent<Tile>();
          return true;
        }
      }
    }

    return false;
  }

  //checks if unit can pass given tile
  public bool CanPass (Tile tile)
  {
    return !(tile.IsOccupied() && tile.GetOccupier().CompareTag(gameObject.tag)) 
      && (tile.Walkable || (tile.Flyable && _flyier));
  }


}
