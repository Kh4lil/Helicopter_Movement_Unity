using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliControl : MonoBehaviour
{

    public HeliRotorControl MainRotorControl;
    //public HeliRotorControl SubRotorControl;

    public Controls Controls;

    public Rigidbody HelicopterModel;

    public float Turn = 3f;
    public float Forward = 10f;
    public float ForwardTilt = 20f;
    public float TurnTilt = 30f;
    public float Height = 100f;
    public float turnTiltPercent = 1.5f;
    public float turnPercent = 1.3f;
    private float _engineForce;

    public float EngineForce
    {
        get { return _engineForce; }
        set
        {
            MainRotorControl.RotorSpeed = value * 80;
            //SubRotorControl.RotorSpeed = value * 40;

            _engineForce = value;
        }
    }

    private Vector2 hMove = Vector2.zero;
    private Vector2 hTilt = Vector2.zero;
    private float hTurn = 0f;
    public bool isGround = true;

    // Start is called before the first frame update
    void Start()
    {
        Controls.KeyPressed += OnKeyPressed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        LiftProcess();
        MoveProcess();
        TiltProcess();
    }

    private void MoveProcess()
    {
        var turnVar = Turn * Mathf.Lerp(hMove.x, hMove.x * (turnTiltPercent - Mathf.Abs(hMove.y)), Mathf.Max(0f, hMove.y));
        hTurn = Mathf.Lerp(hTurn, turnVar, Time.fixedDeltaTime * Turn);
        HelicopterModel.AddRelativeForce(0f, hTurn * HelicopterModel.mass, 0f);
        HelicopterModel.AddRelativeForce(Vector3.forward * Mathf.Max(0f, hMove.y * Forward * HelicopterModel.mass));
        
    }

    private void LiftProcess()
    {
        var upForce = 1 - Mathf.Clamp(HelicopterModel.transform.position.y / Height, 0,1);
        upForce = Mathf.Lerp(0f, EngineForce, upForce) * HelicopterModel.mass;
        HelicopterModel.AddRelativeForce(Vector3.up * upForce);
    }

    private void TiltProcess()
    {
        hTilt.x = Mathf.Lerp(hTilt.x, hMove.x * TurnTilt, Time.deltaTime);
        hTilt.y = Mathf.Lerp(hTilt.y, hMove.y * ForwardTilt, Time.deltaTime);
        HelicopterModel.transform.localRotation = Quaternion.Euler(hTilt.y, HelicopterModel.transform.localEulerAngles.y, -hTilt.x);
    }

    private void OnKeyPressed(Controls.PressedKeyCode[] obj)
    {
        float tempY = 0;
        float tempX = 0;

        if (hMove.y > 0)
            tempY = -Time.fixedDeltaTime;
        else
            if (hMove.x < 0)
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

        hMove.x += tempX;
        hMove.x = Mathf.Clamp(hMove.x, -1, 1);

        hMove.y += tempY;
        hMove.y = Mathf.Clamp(hMove.y, -1, 1);

    }

    private void OnCollisionEnter()
    {
        isGround = true;
    }

    private void OnCollisionExit()
    {
        isGround = false;
    }
}
