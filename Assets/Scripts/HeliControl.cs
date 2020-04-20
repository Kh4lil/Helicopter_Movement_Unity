using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliControl : MonoBehaviour
{
    //Variable Declarations: 
    public HeliRotorControl MainRotorControl;
    public Controls Controls;
    public Rigidbody HelicopterModel;
    private float Turn = 3f;
    private float Forward = 10f;
    private float ForwardTilt = 20f;
    private float TurnTilt = 30f;
    private float Height = 100f;
    private float turnTiltPercent = 1.5f;
    private float turnPercent = 1.3f;
    private float _engineForce;
    private Vector2 horizontalMovement = Vector2.zero;
    private Vector2 horizontalTilt = Vector2.zero;
    private float horizontalTurn = 0f;
    private bool isGround = true;

    public float EngineForce
    {
        get { return _engineForce; }
        set
        {
            MainRotorControl.RotorSpeed = value * 80;
            _engineForce = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Controls.KeyPressed += OnKeyPressed;
    }


    private void FixedUpdate()
    {
        HeliLift();
        HeliMove();
        HeliTilt();
    }

    private void HeliMove()
    {
        var turnVar = Turn * Mathf.Lerp(horizontalMovement.x, horizontalMovement.x * (turnTiltPercent - Mathf.Abs(horizontalMovement.y)), Mathf.Max(0f, horizontalMovement.y));
        horizontalTurn = Mathf.Lerp(horizontalTurn, turnVar, Time.fixedDeltaTime * Turn);
        HelicopterModel.AddRelativeForce(0f, horizontalTurn * HelicopterModel.mass, 0f);
        HelicopterModel.AddRelativeForce(Vector3.forward * Mathf.Max(0f, horizontalMovement.y * Forward * HelicopterModel.mass));
        
    }

    private void HeliLift()
    {
        var upForce = 1 - Mathf.Clamp(HelicopterModel.transform.position.y / Height, 0,1);
        upForce = Mathf.Lerp(0f, EngineForce, upForce) * HelicopterModel.mass;
        HelicopterModel.AddRelativeForce(Vector3.up * upForce);
    }

    private void HeliTilt()
    {
        horizontalTilt.x = Mathf.Lerp(horizontalTilt.x, horizontalMovement.x * TurnTilt, Time.deltaTime);
        horizontalTilt.y = Mathf.Lerp(horizontalTilt.y, horizontalMovement.y * ForwardTilt, Time.deltaTime);
        HelicopterModel.transform.localRotation = Quaternion.Euler(horizontalTilt.y, HelicopterModel.transform.localEulerAngles.y, -horizontalTilt.x);
    }

    private void OnKeyPressed(Controls.PressedKeyCode[] obj)
    {
        float tempY = 0;
        float tempX = 0;

        if (horizontalMovement.y > 0)
            tempY = -Time.fixedDeltaTime;
        else
            if (horizontalMovement.x < 0)
            tempX = Time.fixedDeltaTime;

        foreach (var pressedKeyCode in obj)
        {
            switch (pressedKeyCode)
            {
                case Controls.PressedKeyCode.SpeedUpPressed:
                    EngineForce += 0.1f;
                    break;

                case Controls.PressedKeyCode.SpeedDownPressed:
                    EngineForce -= 0.12f;
                    if (EngineForce < 0) EngineForce = 0;
                    break;

                case Controls.PressedKeyCode.ForwardPressed:

                    if (isGround) break;
                    tempY = -Time.fixedDeltaTime;
                    break;

                case Controls.PressedKeyCode.BackPressed:
                    if(isGround) break;
                    tempY = Time.fixedDeltaTime;
                    break;


                case Controls.PressedKeyCode.LeftPressed:
                    if (isGround) break;
                    tempX = +Time.fixedDeltaTime;
                    break;

                case Controls.PressedKeyCode.RightPressed:
                    if (isGround) break;
                    tempX = -Time.fixedDeltaTime;
                    break;

            }
        }

        horizontalMovement.x += tempX;
        horizontalMovement.x = Mathf.Clamp(horizontalMovement.x, -1, 1);
        horizontalMovement.y += tempY;
        horizontalMovement.y = Mathf.Clamp(horizontalMovement.y, -1, 1);

    }

    //Check if helicopter is on the ground:
    private void OnCollisionEnter()
    {
        isGround = true;
    }
    private void OnCollisionExit()
    {
        isGround = false;
    }
}
