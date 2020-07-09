using System;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
  public const int TILES_LAYER = 1 << 8;
  public const string UNIT_TAG = "Unit";
  public const string TILE_TAG = "Tile";

  public static GameObject DefaultOverlay;
  public GameObject CursorObject;
  public static Tile[,] MapTiles { get; private set; }
  [SerializeField] public int TilesDistance { get; private set; }
  public static int XOffset { get; private set; }
  public static int ZOffset { get; private set; }

  public Cursor Cursor { get; private set; }
  public OverlaysManager OverlaysManager { get; } = new OverlaysManager();
  public BattleTurnManager BattleTurnManager { get; private set; }


  private void Awake()
  {
    OverlaysManager.LoadMaterials();
    BattleTurnManager = GetComponent<BattleTurnManager>();
    Cursor = CursorObject.GetComponent<Cursor>();
    DefaultOverlay = Cursor.Overlay;
    FindAllTiles();
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

  //initialises and populates MapTiles [0,0] is in the left bottom corner
  //i.e minZ, minX
  private void FindAllTiles()
  {
    GameObject[] tileObjects = GameObject.FindGameObjectsWithTag(TILE_TAG);
    int minZ = int.MaxValue;
    int maxZ = int.MinValue;
    int minX = int.MaxValue;
    int maxX = int.MinValue;

    foreach (GameObject tileObject in tileObjects)
    {
      minZ = Math.Min(minZ, (int)tileObject.transform.position.z);
      maxZ = Math.Max(maxZ, (int)tileObject.transform.position.z);
      minX = Math.Min(minX, (int)tileObject.transform.position.x);
      maxX = Math.Max(maxX, (int)tileObject.transform.position.x);
    }

    MapTiles = new Tile[maxZ - minZ + 1, maxX - minX + 1];

    XOffset = -minX;
    ZOffset = -minZ;

    foreach (GameObject tileObject in tileObjects)
    {
      Tile tile = tileObject.GetComponent<Tile>();
      MapTiles[(int)tile.transform.position.z - minZ,
               (int)tile.transform.position.x - minX] = tile;
    }
  }

}
