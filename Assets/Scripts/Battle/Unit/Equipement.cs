using System.Collections.Generic;
using UnityEngine;

public class Equipement : MonoBehaviour
{
  private List<Weapon> _weapons;
  private int _maxNumberOfWeapons;
  private int _equippedWeapon;

  public Weapon GetEquippedWeapon()
  {
    return _weapons[_equippedWeapon];
  }

  public void EquipNextWeapon()
  {
    _equippedWeapon = (_equippedWeapon + 1) % _weapons.Count;
  }

  public void EquipPreviousWeapon()
  {
    //keeps modulo positive.
    _equippedWeapon = ((_equippedWeapon - 1) % _weapons.Count + _weapons.Count)
                      % _weapons.Count;
  }
}
