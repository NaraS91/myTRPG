using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

  public Dictionary<string, ISet<Unit>> Groups;
  public List<string> TurnQueue;
  public int activeGroup;
  public Object overlay;
  public Tile selectedTile { get; set; }

  // Start is called before the first frame update
  void Start()
  {
    activeGroup = -1;
   // BeginNextTurn();
    if (!SelectStartingTile())
    {
      print("tile at position (0, 0, 0) is required");
    } else
    {
      SetOverlay();
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  private bool SelectStartingTile()
  {
    Collider[] colliders = Physics.OverlapSphere(new Vector3(0, 0, 0), 0);
    foreach(Collider collider in colliders)
    {
      if(collider.gameObject.GetComponent<Tile>() != null)
      {
        selectedTile = collider.gameObject.GetComponent<Tile>();
      }
    }

    return selectedTile != null;
  }

  private void SetOverlay()
  {
    Object.Instantiate(overlay, selectedTile.transform);
  }

  private void BeginNextTurn()
  {
    activeGroup = (activeGroup + 1) % TurnQueue.Count;
    foreach(Unit unit in Groups[TurnQueue[activeGroup]])
    {
      unit.Selectable = true;
    }
  }

  private void EndTurn()
  {
    foreach (Unit unit in Groups[TurnQueue[activeGroup]])
    {
      unit.Selectable = false;
    }
  }

}
