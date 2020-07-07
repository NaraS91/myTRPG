using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
  public int Health { get; set; } = 50;
  public int Movement { get; set; } = 4;
  public bool Flyier { get; set; } = false;
  public Tile OccupiedTile { get; set; }
  public int Group;
  public bool Selectable { get; set; } = false;
  public bool Selected;

  private void Awake()
  {
    tag = BattleManager.UNIT_TAG;
  }

  // Start is called before the first frame update
  void Start()
  {
    UpdateTile();
  }

  public void FreeTile()
  {
    OccupiedTile.RemoveOccupier();
    OccupiedTile = null;
  } 

  //frees tile currently occupied and sets occupied tile to correct tile
  public bool UpdateTile()
  {
    Collider[] colliders = Physics.OverlapSphere(
      new Vector3(transform.position.x, 0.0f, transform.position.z),
      0.0f, BattleManager.TILES_LAYER);

    foreach (Collider collider in colliders)
    {
      if(collider.gameObject != null && collider.gameObject.CompareTag("Tile"))
      {
        Tile overlapingTile = collider.gameObject.GetComponent<Tile>();
        if (overlapingTile.SetOccupier(this))
        {
          if(OccupiedTile != null && OccupiedTile != overlapingTile) 
            FreeTile();
          OccupiedTile = collider.gameObject.GetComponent<Tile>();
          return true;
        }
      }
    }

    return false;
  }

  //checks if unit can pass given tile
  public bool CanPass (Tile tile)
  {
    return !(tile.IsOccupied() && tile.Occupier.Group != Group) 
      && (tile.Walkable || (tile.Flyable && Flyier));
  }


  //Move unit to selected Tile
  public void Move(Tile tile)
  {
    Vector3 newUnitPosition = tile.transform.position;
    newUnitPosition.y = transform.position.y;
    transform.position = newUnitPosition;
    UpdateTile();
  }

}
