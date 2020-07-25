using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
  [SerializeField] private Text _text;
  [SerializeField] private GameObject _backgroundImage;
  [SerializeField] private GameObject _hoveredBackgroundImage;
  [SerializeField] private float[] _textRGB;
  [SerializeField] private float[] _hoveredTextRGB;

  private void Awake()
  {
    if(_text == null || _backgroundImage == null || _hoveredTextRGB == null ||
       _hoveredBackgroundImage == null || _textRGB == null)
    {
      Debug.Log("Missing some button componenet");
      enabled = false;
    }

    if(_textRGB.Length != 3 || _hoveredTextRGB.Length != 3)
    {
      Debug.Log("wrong text RGB array size");
      enabled = false;
    }
  }

  public void HoveredOver()
  {
    _text.color
      = new Color(_hoveredTextRGB[0], _hoveredTextRGB[1], _hoveredTextRGB[2]);
    _backgroundImage.SetActive(false);
    _hoveredBackgroundImage.SetActive(true);
  }

  public void NotHoveredOver()
  {
    _text.color = new Color(_textRGB[0], _textRGB[1], _textRGB[2]);
    _backgroundImage.SetActive(true);
    _hoveredBackgroundImage.SetActive(false);
  }

  public void SetText(string text)
  {
    _text.text = text;
  }

  public void SetPosition(int x, int y, int z)
  {
    GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);
  }
}
