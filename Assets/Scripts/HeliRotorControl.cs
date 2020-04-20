using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliRotorControl : MonoBehaviour
{
    private float m_rotorSpeed;

    public float RotorSpeed
    {
        get { return m_rotorSpeed; }
        set { m_rotorSpeed = Mathf.Clamp(value, 0, 3000); }
    }

    private float rotationDegree;
    private Vector3 originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

        rotationDegree += RotorSpeed * Time.deltaTime;
        rotationDegree = rotationDegree % 360;
        transform.localRotation = Quaternion.Euler(originalRotation.x, rotationDegree, originalRotation.z);
    }
}
