using System;

public static class CombatManager
{

  //returns true when defenders hp goes to 0
  public static bool DefaultCombat(Unit attacker, Unit defender)
  {
    int dmg = attacker.EquippedWeapon.Power;

    switch (attacker.EquippedWeapon.DamageType)
    {
      case EDamageType.Magical:
        dmg += attacker.Magic - defender.Resist;
        break;
      case EDamageType.Physical:
        dmg += attacker.Attack - defender.Defense;
        break;
    }

    if (dmg > 0) defender.Health = Math.Max(defender.Health - dmg, 0);

    return defender.Health == 0;
  }
}
