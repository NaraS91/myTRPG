public class Weapon
{
  public int Power { get; private set; }
  public EDamageType DamageType { get; private set; }

  public Weapon(int power, EDamageType damageType)
  {
    Power = power;
    DamageType = damageType;
  }
}
