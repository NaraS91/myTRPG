using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
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

  // Update is called once per frame
  void Update()
  {
    transform.position 
      = _defaultOffset + _target.transform.position + _extraOffset;
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
