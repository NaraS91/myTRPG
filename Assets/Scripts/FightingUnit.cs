using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingUnit : Unit
{
  //TODO: update stats!
  [SerializeField]private int _attack;
  [SerializeField] private int _defense;

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
