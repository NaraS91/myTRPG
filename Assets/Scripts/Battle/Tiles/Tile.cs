using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
  public bool Walkable { get; protected set; } = true;
  public bool Flyable { get; protected set; } = true;
  public bool Current { get; protected set; } = false;
  public int Cost { get; protected set; } = 1;
  public int Height { get; protected set; } = 1;
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
    SetProperties();
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


  //returns tiles distant by exactly 'distance'
  public ISet<Tile> FindTiles(int distance)
  {
    ISet<Tile> result = new HashSet<Tile>();
    int x = (int) transform.position.x;
    int z = (int) transform.position.z;

    for(int i = -distance; i <= distance; i++)
    {
      int tileXIndex = i + x + BattleManager.XOffset;
      if(tileXIndex >= 0 &&
        tileXIndex < BattleManager.MapTiles.GetLength(1))
      {
        int tileZIndex
          = z + BattleManager.ZOffset + distance - Math.Abs(i);
        if(tileZIndex >= 0 &&
          tileZIndex < BattleManager.MapTiles.GetLength(0))
        {
          result.Add(BattleManager.MapTiles[tileZIndex, tileXIndex]);
        }

        tileZIndex 
          = z + BattleManager.ZOffset - distance + Math.Abs(i);
        if (tileZIndex >= 0 &&
          tileZIndex < BattleManager.MapTiles.GetLength(0))
        {
          result.Add(BattleManager.MapTiles[tileZIndex, tileXIndex]);
        }
      }
    }

    return result;
  }


  private void FindAdjacentTiles()
  {
    int x = (int) transform.position.x;
    int z = (int) transform.position.z;
    if (x + BattleManager.XOffset + 1 < BattleManager.MapTiles.GetLength(1))
    {
      RightTile = BattleManager.MapTiles[z + BattleManager.ZOffset,
                                         x + BattleManager.XOffset + 1];
    }

    if (x + BattleManager.XOffset - 1 >= 0)
    {
      LeftTile = BattleManager.MapTiles[z + BattleManager.ZOffset,
                                        x + BattleManager.XOffset - 1];
    }

    if (z + BattleManager.ZOffset + 1 < BattleManager.MapTiles.GetLength(0))
    {
      ForwardTile = BattleManager.MapTiles[z + BattleManager.ZOffset + 1,
                                           x + BattleManager.XOffset];
    }

    if (z + BattleManager.ZOffset - 1 >= 0)
    {
      BackTile = BattleManager.MapTiles[z + BattleManager.ZOffset - 1,
                                         x + BattleManager.XOffset];
    }
  }

  //function designed to be overwritten by childs of Tile class to
  //change default values at Start().
  protected virtual void SetProperties() { }
}
