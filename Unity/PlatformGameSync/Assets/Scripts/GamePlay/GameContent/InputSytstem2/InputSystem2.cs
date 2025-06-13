using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnKeyDown(KeyCode keyCode);

public delegate void OnKeyUp(KeyCode keyCode);

public class InputSystem2 {
    public OnKeyDown OnKeyDown;
    public OnKeyUp OnKeyUp;

    private List<KeyCode> _allKeyCode;

    public InputSystem2() {
        _allKeyCode = new();
        Array keyCodes = Enum.GetValues(typeof(KeyCode));
        for (int i = 0; i < keyCodes.Length; i++) {
            KeyCode k = (KeyCode)keyCodes.GetValue(i);
            _allKeyCode.Add(k);
        }
    }

    public void ClearLisntner() {
        OnKeyDown = null;
        OnKeyUp = null;
    }

    public void InputUpdate() {
        for (int i = 0; i < _allKeyCode.Count; i++) {
            var code = _allKeyCode[i];
            if (Input.GetKeyDown(code)) {
                OnKeyDown?.Invoke(code);
            }
            if (Input.GetKeyUp(code)) {
                OnKeyUp?.Invoke(code);
            }
        }
    }
}