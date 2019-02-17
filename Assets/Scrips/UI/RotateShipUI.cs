using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShipUI : MonoBehaviour
{
    public float rotationSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate((Vector3.up * Time.deltaTime) * rotationSpeed, Space.World);
    }
}
