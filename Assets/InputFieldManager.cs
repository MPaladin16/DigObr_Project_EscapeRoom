using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldManager : MonoBehaviour
{
    [SerializeField] private GameObject _keypadGameObject;
    [SerializeField] private NonNativeKeyboard _keypad;
    [SerializeField] private List<TMP_InputField> _inputFields;

    private bool _closed;

    private void Start()
    { 

        foreach (TMP_InputField field in _inputFields)
        {
            field.onSelect.AddListener(x => OnInputFieldSelected(field));
        }

        _keypad.OnTextSubmitted += KeepKeypadEnabled;
    }

    private void KeepKeypadEnabled(object sender, EventArgs e)
    {
        _closed = true;
    }

    private void OnInputFieldSelected(TMP_InputField field)
    {
        _keypad.InputField = field;
    }

    private void Update()
    {
        if (_closed)
        {
            _keypadGameObject.SetActive(true);
            _closed = false;
        }
    }
}
