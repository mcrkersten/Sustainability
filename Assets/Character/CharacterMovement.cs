using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private bool isHorizonControlRotate = true;
    [SerializeField]
    private float rotationSpeed = 50.0f;
    [SerializeField]
    private float movementSpeed = 50.0f;

    void Update()
    {
        Vector3 rotationCoord = new Vector3(0, Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed);
        if (isHorizonControlRotate)
            transform.Rotate(rotationCoord);

        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed);
    }
}
