using UnityEngine;

public class Cursor : MonoBehaviour
{
  public int speed;
  public GameObject Overlay;
  public Tile HoveredTile;
  public Tile SelectedTile;

  // Start is called before the first frame update
  void Start()
  {
    
  }

  private void FixedUpdate()
  {
    UpdateSelectedTile();
  }

  // Update is called once per frame
  void Update()
  {
    moveCursor();
  }


  //sets selected tile to match cursor
  void UpdateSelectedTile()
  {
    Collider[] colliders = Physics.OverlapSphere(new Vector3
      (transform.position.x, 0.0f, transform.position.z),
      0.0f, BattleManager.TILES_LAYER);
    foreach (Collider collider in colliders)
    {
      if (collider.gameObject.CompareTag("Tile"))
      {
        SelectedTile = collider.gameObject.GetComponent<Tile>();
        Overlay.transform.position = SelectedTile.transform.position;
      }
    }
  }

  private void moveCursor()
  {
    float vertical = Input.GetAxis("Vertical") * speed;
    float horizontal = Input.GetAxis("Horizontal") * speed;
    transform.Translate
      (horizontal * Time.deltaTime, 0, vertical * Time.deltaTime);
  }
}
