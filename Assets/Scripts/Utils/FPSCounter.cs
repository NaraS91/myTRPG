using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
  [SerializeField] private Text _text;

  // Update is called once per frame
  void Update()
  {
    int fps = (int)(1 / Time.unscaledDeltaTime);
    _text.text = "FPS " + fps;
  }
}
