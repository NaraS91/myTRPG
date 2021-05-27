using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMapMenu : MonoBehaviour
{
  [SerializeField]
  private Selectable _firsField = default;
  [SerializeField]
  private InputField _rowsCount = default;
  [SerializeField]
  private InputField _columnsCount = default;
  [SerializeField]
  private GameObject _map = default;
  [SerializeField]
  private GameObject _tileGO = default;

  private void OnEnable()
  {
    _firsField.Select();
  }

  public void CreateMap()
  {
    int rowsCount, columnsCount;
    if (!int.TryParse(_rowsCount.text, out rowsCount) ||
      !int.TryParse(_columnsCount.text, out columnsCount))
      return;

    if (rowsCount <= 0 || columnsCount <= 0)
      return;

    foreach(Transform child in _map.transform)
    {
      Destroy(child.gameObject);
    }
    for (int i = 0; i < columnsCount; i++)
    {
      for (int j = 0; j < rowsCount; j++)
      {
        GameObject t = Instantiate(_tileGO, _map.transform);
        var tile = t.GetComponent<Tile>();
        t.transform.position = new Vector3(i * tile.Width, 0, j * tile.Length);
      }
    }
  }
}
