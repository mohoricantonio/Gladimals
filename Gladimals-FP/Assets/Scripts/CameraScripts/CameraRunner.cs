using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRunner : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(Vector3.zero, Vector3.up, -30 * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(Vector3.zero, Vector3.up, 30 * Time.deltaTime);
        }
    }
}
