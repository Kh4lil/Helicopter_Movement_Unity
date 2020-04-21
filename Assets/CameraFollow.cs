using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    public Vector3 target_Offset;

    private void Start()
    {
        target_Offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position+target_Offset, 0.1f);
        }
   
    }
}
