using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRunner : MonoBehaviour
{

    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, 15 * Time.deltaTime);
    }
}
