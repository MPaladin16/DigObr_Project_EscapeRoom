using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeyboardTest : MonoBehaviour
{
    // Start is called before the first frame update

    private TouchScreenKeyboard keyboard;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
}
