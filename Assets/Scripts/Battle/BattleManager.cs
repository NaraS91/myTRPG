using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
  public const int TILES_LAYER = 1 << 8;
  public const string UNIT_TAG = "Unit";

  public static GameObject DefaultOverlay;
  public GameObject CursorObject;

  public Cursor Cursor { get; private set; }
  public OverlaysManager OverlaysManager { get; } = new OverlaysManager();
  public BattleTurnManager BattleTurnManager { get; private set; }


  private void Awake()
  {
    OverlaysManager.LoadMaterials();
    BattleTurnManager = GetComponent<BattleTurnManager>();
    Cursor = CursorObject.GetComponent<Cursor>();
    DefaultOverlay = Cursor.Overlay;
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  private void FixedUpdate()
  {
  }

  // Update is called once per frame
  void Update()
  {
    
    
    
  }

}
