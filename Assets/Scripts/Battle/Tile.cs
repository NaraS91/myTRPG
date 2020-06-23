using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{

  public bool Walkable { get; set; } = true;
  public bool Flyable { get; set; } = true;
  public bool Current { get; set; } = false;
  public int Cost { get; } = 1;
  public int Height { get; private set; } = 1;
  public Unit Occupier { get; private set; }
  public Tile ForwardTile { get; private set; }
  public Tile RightTile { get; private set; }
  public Tile LeftTile { get; private set; }
  public Tile BackTile { get; private set; }
  public GameObject Overlay;
  public MeshRenderer OverlayMeshRenderer;

  // Start is called before the first frame update
  void Start()
  {
    FindAdjacentTiles();
    Overlay = Instantiate(BattleManager.DefaultOverlay, transform);
    Overlay.transform.position = transform.position;
    Overlay.SetActive(false);
    OverlayMeshRenderer = Overlay.GetComponentInChildren<MeshRenderer>();
  }

  //some tiles may be null
  public Tile[] GetAdjacentTiles()
  {
    return new Tile[4] { ForwardTile, RightTile, BackTile, LeftTile}
      .Where(t => t != null).ToArray();
  }

  // return false if tile is already occupied by other object
  public bool SetOccupier(Unit Occupier)
  {
    if (IsOccupied() && this.Occupier != Occupier)
    {
      return false;
    }

    this.Occupier = Occupier;
    return true;
  }

  public void RemoveOccupier()
  {
    Occupier = null;
  }

  public bool IsOccupied()
  {
    return Occupier != null;
  }

  private void FindAdjacentTiles()
  {
    Collider[] adjacentColliders = Physics.OverlapSphere(transform.position,
      transform.localScale.x * 1.1f, BattleManager.TILES_LAYER);

    foreach (Collider collider in adjacentColliders)
    {
      if (collider.gameObject != null)
      {
        GameObject tile = collider.gameObject;
        float deltaX = tile.transform.position.x - transform.position.x;
        float deltaZ = tile.transform.position.z - transform.position.z;
        switch (deltaX * 3 + deltaZ)
        {
          case 3:
            RightTile = tile.GetComponent<Tile>(); break;
          case -3:
            LeftTile = tile.GetComponent<Tile>(); break;
          case 1:
            ForwardTile = tile.GetComponent<Tile>(); break;
          case -1:
            BackTile = tile.GetComponent<Tile>(); break;
          default:
            break;
        }

      }
    }
  }
}
