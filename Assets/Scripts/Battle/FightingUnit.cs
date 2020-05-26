using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingUnit : Unit
{
  //TODO: update stats!
  private int Attack { get; set; }
  private int Defense { get; set; }

  // Start is called before the first frame update
  void Start()
  {
    UpdateTile();
  }

  // Update is called once per frame
  void Update()
  {
    
  }
}
