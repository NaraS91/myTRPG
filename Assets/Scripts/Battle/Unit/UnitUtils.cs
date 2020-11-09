using UnityEngine;

class UnitUtils
{
  public static void MoveTowards(Unit unit, Vector3 targetPosition, float steps)
  {
    Vector3 vectorDiff = targetPosition - unit.transform.position;
    vectorDiff.y = 0;

    vectorDiff = Vector3.ClampMagnitude(vectorDiff, steps);

    Vector3 newPos = unit.transform.position + vectorDiff;

    unit.transform.position = newPos;
  }  
}
