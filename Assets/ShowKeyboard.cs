using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;

public class ShowKeyboard : MonoBehaviour
{
    [SerializeField] private NonNativeKeyboard _keypad;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(_keypad);
        _keypad.PresentKeyboard();
    }
}
