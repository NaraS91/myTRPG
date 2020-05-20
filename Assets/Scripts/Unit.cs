using UnityEngine;

public class Unit : MonoBehaviour
{
  [SerializeField] private int health = 50;
  [SerializeField] private int movement = 1;
  [SerializeField] private bool flyier = false;
  [SerializeField] private Tile occupiedTile;
  [SerializeField] private int militant_side;

  // Start is called before the first frame update
  void Start()
  {
    setTile();
  }

  // Update is called once per frame
  void Update()
  {
        
  }

  public Tile getOccupiedTile()
  {
    return occupiedTile;
  }

  public int getMovement()
  {
    return movement;
  }

  public int getMilitantSide()
  {
    return militant_side;
  }

  public void freeTile()
  {
    occupiedTile.removeOccupier();
    occupiedTile = null;
  } 

  public bool setTile()
  {
    Collider[] colliders = Physics.OverlapSphere(
      new Vector3(transform.position.x, 0.0f, transform.position.z), 0.0f);

    foreach (Collider collider in colliders)
    {
      if(collider.gameObject != null && collider.gameObject.tag.Equals("Tile"))
      {
        if (collider.gameObject.GetComponent<Tile>().setOccupier(this.gameObject))
        {
          if(occupiedTile != null) freeTile();
          occupiedTile = collider.gameObject.GetComponent<Tile>();
          return true;
        }
      }
    }

    return false;
  }

  public bool canPass (Tile tile)
  {
    return !(tile.isOccupied() && tile.getOccupier().tag != gameObject.tag) 
      && (tile.isWalkable() || (tile.isFlyable() && flyier));
  }


}
