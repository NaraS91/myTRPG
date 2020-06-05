﻿using UnityEngine;

public class Unit : MonoBehaviour
{
  public int Health { get; set; } = 50;
  public int Movement { get; set; } = 1;
  public bool Flyier { get; set; } = false;
  public Tile OccupiedTile { get; set; }
  public string Group { get; set; }
  public bool Selectable { get; set; }
  public bool Selected;

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
        if (collider.gameObject.GetComponent<Tile>().SetOccupier(this.gameObject))
        {
          if(OccupiedTile != null) FreeTile();
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
    return !(tile.IsOccupied() && tile.GetOccupier().CompareTag(gameObject.tag)) 
      && (tile.Walkable || (tile.Flyable && Flyier));
  }


}
