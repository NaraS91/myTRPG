using System;
using UnityEngine;

public class CombatManager
{
  public static void DefaultCombat(Unit attacker, Unit defender)
  {
    DealDMG(attacker.Attack, attacker.EquippedWeapon.DamageType, defender);
    if(defender.Health == 0)
    {
      OnUnitDefeat(defender);
    }
  }

  //Deals dmg to RecivingUnit, caps minimum hp at 0
  private static void DealDMG
    (int attackPower, EDamageType damageType, Unit RecivingUnit)
  {
    int dmg = attackPower;

    switch (damageType)
    {
      case EDamageType.Magical:
        dmg -= RecivingUnit.Resist;
        break;
      case EDamageType.Physical:
        dmg -= RecivingUnit.Defense;
        break;
      default:
        Debug.LogError("damageType not implemented");
        break;
    }

    if (dmg < 0) return;

    RecivingUnit.Health = Math.Max(RecivingUnit.Health - dmg, 0);
  }

  private static void OnUnitDefeat(Unit unit)
  {
    unit.FreeTile();
    unit.gameObject.SetActive(false);
  }
}
