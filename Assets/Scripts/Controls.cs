using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    //Controls: 
    KeyCode SpeedUp = KeyCode.R;
    KeyCode SpeedDown = KeyCode.F;
    KeyCode Forward = KeyCode.W;
    KeyCode Back = KeyCode.S;
    KeyCode Left = KeyCode.A;
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
