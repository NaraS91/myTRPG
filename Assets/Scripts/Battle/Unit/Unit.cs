﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Unit : MonoBehaviour
{
  public int Level;
  public int Health;
  public int Movement;
  public int Attack;
  public int Magic;
  public int Defense;
  public int Resist;
  public int Speed;
  public int Dexterity;
  public int Exp { get; private set; }
  public int ExpForLevelUp { get; private set; }
  public Weapon EquippedWeapon { get; private set; }

  public bool Flyier { get; private set; } = false;
  public Tile OccupiedTile { get; private set; }
  public int Group;
  public bool Selectable { get; set; } = false;
  public bool Selected;

  public bool isMoving { get; private set; } = false;
  private Tile _previousTile;
  

  private void Awake()
  {
    tag = BattleManager.UNIT_TAG;
    EquippedWeapon = new Weapon(5, EDamageType.Physical);
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
    _previousTile = OccupiedTile;
    Vector3 newUnitPosition = tile.transform.position;
    newUnitPosition.y = transform.position.y;
    transform.position = newUnitPosition;
    UpdateTile();
  }

  public void MoveToPreviousPosition()
  {
    Move(_previousTile);
  }

  public void StartMoveCoroutine(LinkedList<Tile> path)
  {
    _previousTile = OccupiedTile;
    StartCoroutine(PrettyMove(path));
  }

  //moves unit using unit animation
  IEnumerator PrettyMove(LinkedList<Tile> path)
  {
    isMoving = true;
    int speed = 4;
    Vector3 unitPosition = transform.position;

    foreach(Tile tile in path)
    {
      if (tile.Equals(OccupiedTile))
      {
        continue;
      }

      Vector3 tilePosition = tile.transform.position;
      bool isInTheTilesCenter = unitPosition.x == tilePosition.x &&
                                unitPosition.z == tilePosition.z;
      while (!isInTheTilesCenter)
      {
        UnitUtils.MoveTowards(this, tilePosition, speed * Time.deltaTime);
        unitPosition = transform.position;
        isInTheTilesCenter = unitPosition.x == tilePosition.x &&
                             unitPosition.z == tilePosition.z;
        yield return null;
      }

      yield return tile.Visit(this);
    }
    UpdateTile();
    isMoving = false;
  }

  //calculates current attack power of unit (includeing equipped weapon)
  public int AttackPower()
  {
    if(EquippedWeapon == null)
    {
      return 0;
    }

    switch (EquippedWeapon.DamageType)
    {
      case EDamageType.Magical:
        return EquippedWeapon.Power + Magic;
      case EDamageType.Physical:
        return EquippedWeapon.Power + Attack;
      default:
        Debug.LogError("damage type not implemented");
        return -1;
    }
  }

  public void GainExp(int expGained)
  {
    if(expGained < 0)
    {
      Debug.LogError("Unit cannot gain negative number of exp");
    } else
    {
      Exp += expGained;
    }

    while(Exp >= ExpForLevelUp)
    {
      Exp -= ExpForLevelUp;
      LevelUp();
    }
  }

  private void LevelUp()
  {
    Level++;
  }

}
