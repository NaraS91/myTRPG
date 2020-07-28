using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
  public Vector3 CameraOffset;
  private GameObject _cursor;
  private GameObject _target;

  // Start is called before the first frame update
  void Start()
  {
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
    transform.position = CameraOffset + _target.transform.position;
  }

  public void ResetTarget()
  {
    _target = _cursor;
  }

  public void ChangeTarget(GameObject gameObject)
  {
    _target = gameObject;
  }
}
