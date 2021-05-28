using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
  [SerializeField]
  private float smoothness = 2f; 
  private Vector3 _defaultOffset;
  private GameObject _cursor;
  private GameObject _target;
  private Vector3 _extraOffset = Vector3.zero;

  // Start is called before the first frame update
  void Start()
  {
    _defaultOffset = new Vector3(0, 4, -4);
    _cursor = GameObject.FindGameObjectWithTag("Cursor");
    if(_cursor == null)
    {
      Debug.LogError("Cursor wasn't found");
    }
    _target = _cursor;
  }

  void FixedUpdate()
  {
    var targetPosition = _defaultOffset + _target.transform.position + _extraOffset;
    transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.fixedDeltaTime);
  }


  //camera centers at cursor
  public void ResetTarget()
  {
    _target = _cursor;
  }

  //camera centers at given game object
  public void ChangeTarget(GameObject gameObject)
  {
    _target = gameObject;
    _extraOffset = Vector3.zero;
  }

  public void ChangeTarget(GameObject gameObject, Vector3 offset)
  {
    _target = gameObject;
    _extraOffset = offset;
  }
}
