using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;

public class ShowKeyboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NonNativeKeyboard.Instance.PresentKeyboard();
    }
}
