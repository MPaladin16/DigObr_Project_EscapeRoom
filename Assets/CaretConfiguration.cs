using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;
using TMPro;
using UnityEngine;

public class CaretConfiguration : MonoBehaviour
{
    TMP_InputField _inputField;
    void Start()
    {
        _inputField = GetComponent<TMP_InputField>();
        _inputField.onSelect.AddListener(x => ConfigureCaret());
    }

    private void ConfigureCaret()
    {
        SetCaretColorAlpha(1f);

        NonNativeKeyboard.Instance.OnClosed += Instance_OnClosed;
    }

    private void Instance_OnClosed(object sender, EventArgs e)
    {
        SetCaretColorAlpha(0f);
        NonNativeKeyboard.Instance.OnClosed -= Instance_OnClosed;
    }

    private void SetCaretColorAlpha(float value)
    {
        _inputField.customCaretColor = true;
        Color caretColor = _inputField.caretColor;
        caretColor.a = value;
        _inputField.caretColor = caretColor;
    }
}
