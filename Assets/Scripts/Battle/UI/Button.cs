using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
  [SerializeField] private Text _text;
  [SerializeField] private GameObject _backgroundImage;
  [SerializeField] private GameObject _hoveredBackgroundImage;
  [SerializeField] private float[] _textRGB;
  [SerializeField] private float[] _hoveredTextRGB;
  public EButtonType Type { get; private set; }

  //checks if button is valid
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

  //flags this button as active
  public void HoveredOver()
  {
    _text.color
      = new Color(_hoveredTextRGB[0], _hoveredTextRGB[1], _hoveredTextRGB[2]);
    _backgroundImage.SetActive(false);
    _hoveredBackgroundImage.SetActive(true);
  }

  //button is no longer active
  public void NotHoveredOver()
  {
    _text.color = new Color(_textRGB[0], _textRGB[1], _textRGB[2]);
    _backgroundImage.SetActive(true);
    _hoveredBackgroundImage.SetActive(false);
  }

  public void SetText(string text)
  {
    if (Enum.TryParse<EButtonType>(text, out EButtonType parsedType))
    {
      Type = parsedType;
    } else
    {
      Debug.LogError("Parsing did not succeed");
      Type = EButtonType.Move;
    }


    _text.text = text;
  }

  public void SetPosition(int x, int y, int z)
  {
    GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);
  }
}
