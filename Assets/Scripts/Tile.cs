using UnityEngine;

public class Tile : MonoBehaviour
{

  [SerializeField] private bool walkable = true;
  [SerializeField] private bool flyable = true;
  [SerializeField] private bool current = false;
  [SerializeField] private int cost = 1;
  [SerializeField] private GameObject occupier;
  private Tile forwardTile;
  private Tile rightTile;
  private Tile leftTile;
  private Tile backTile;

  // Start is called before the first frame update
  void Start()
  {
    findAdjacentTiles();
  }

  // Update is called once per frame
  void Update()
  {
        
  }

  //some tiles may be null
  public Tile[] getAdjacentTiles()
  {
    return new Tile[4] {forwardTile, rightTile, backTile, leftTile};
  }

  public GameObject getOccupier()
  {
    return occupier;
  }

  // return false if tile is already occupied by other object
  public bool setOccupier(GameObject occupier)
  {
    if (isOccupied() && this.occupier != occupier)
    {
      return false;
    }

    this.occupier = occupier;
    return true;
  }

  public void removeOccupier()
  {
    occupier = null;
  }

  public bool isOccupied()
  {
    return occupier != null;
  }

  public bool isWalkable()
  {
    return walkable;
  }

  public bool isFlyable()
  {
    return flyable;
  }

  private void findAdjacentTiles()
  {
    Collider[] adjacentColliders
      = Physics.OverlapSphere(transform.position, transform.localScale.x * 1.1f);

    foreach (Collider collider in adjacentColliders)
    {
      if (collider.gameObject != null && collider.gameObject.tag.Equals("Tile"))
      {
        GameObject tile = collider.gameObject;
        float deltaX = tile.transform.position.x - transform.position.x;
        float deltaZ = tile.transform.position.z - transform.position.z;
        switch (deltaX * 3 + deltaZ)
        {
          case 3:
            rightTile = tile.GetComponent<Tile>(); break;
          case -3:
            leftTile = tile.GetComponent<Tile>(); break;
          case 1:
            forwardTile = tile.GetComponent<Tile>(); break;
          case -1:
            backTile = tile.GetComponent<Tile>(); break;
          default:
            break;
        }

      }
    }
  }
}
