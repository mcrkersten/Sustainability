using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public bool isHorizonControlRotate = true;
    public float rotationSpeed = 50.0f;
    public float movementSpeed = 50.0f;
    public float limitOfRotationRightCrood;
    public float limitOfRotationLeftCrood;

    private new Rigidbody rigidbody;
    private GameObject meshObject;

    private float rotationDest;
    private float rotationStart;
    private float lerpTime = 0;

    enum RotateStateCode
    {
        Left,Right,middle
    }
    private RotateStateCode rotateState;
    private RotateStateCode RotateState
    {
        get
        {
            return rotateState;
        }
        set
        {
            if(value != rotateState)
            {
                ChangeRotateStateTo(value);
            }
            rotateState = value;
        }
    }

    void ChangeRotateStateTo(RotateStateCode destState)
    {
        rotationStart = meshObject.transform.rotation.eulerAngles.z;

        switch(destState)
        {
            case RotateStateCode.Left:   rotationDest = limitOfRotationLeftCrood;   break;
            case RotateStateCode.Right:  rotationDest = limitOfRotationRightCrood;   break;
            case RotateStateCode.middle: rotationDest = 0; break;
        }

        lerpTime = 0;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshObject = transform.Find("ShipMesh").gameObject;
    }

    void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");

        Vector3 rotationCoord = new Vector3(0, horizontalAxis * Time.deltaTime * rotationSpeed);

        if (isHorizonControlRotate)
        {
            if (horizontalAxis != 0)
                if (horizontalAxis < 0)
                {
                    RotateState = RotateStateCode.Right;
                }
                else
                {
                    RotateState = RotateStateCode.Left;
                }
            else RotateState = RotateStateCode.middle;

            transform.Rotate(rotationCoord);

            ProcessRotateState();
        }

        if (Input.GetAxis("Vertical") != 0)
            rigidbody.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed);
    }

    void ProcessRotateState()
    {
        if (lerpTime <= 1)
        {
            float rotateAngle = Mathf.LerpAngle(rotationStart, rotationDest, lerpTime);
            meshObject.transform.localEulerAngles = new Vector3(0, 0, rotateAngle);
            Debug.Log("dest" + rotationDest + " current" + rotateAngle + " state" + rotateState);
            lerpTime += Time.deltaTime;
        }
    }
}
