using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField]
    KeyCode SpeedUp = KeyCode.R;
    [SerializeField]
    KeyCode SpeedDown = KeyCode.F;
    [SerializeField]
    KeyCode Forward = KeyCode.W;
    [SerializeField]
    KeyCode Back = KeyCode.S;
    [SerializeField]
    KeyCode Left = KeyCode.A;
    [SerializeField]
    KeyCode Right = KeyCode.D;

    private KeyCode[] keyCodes;

    public enum PressedKeyCode
    {
        SpeedUpPressed,
        SpeedDownPressed,
        ForwardPressed,
        BackPressed,
        LeftPressed,
        RightPressed,
    }

    public Action<PressedKeyCode[]> KeyPressed;

    private void Awake()
    {
        keyCodes = new[]
        {
            SpeedUp,
            SpeedDown,
            Forward,
            Back,
            Left,
            Right
        };
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        var pressedKeyCode = new List<PressedKeyCode>();
        for (int i = 0; i < keyCodes.Length; i++)
        {
            var keyCode = keyCodes[i];
            if (Input.GetKey(keyCode))
            {
                pressedKeyCode.Add((PressedKeyCode)i);
            }
        }

        if (KeyPressed != null)
        {
            KeyPressed(pressedKeyCode.ToArray());
        }
    }

}
