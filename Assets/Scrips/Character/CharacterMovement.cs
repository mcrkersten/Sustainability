using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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

    private IEnumerator stopSoundSmooth;
    private bool isSmoothing = false;

    private AudioSource audioSource;

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
            
        }
    }

    enum BoostSoundStateCode
    {
        Stoped,Launching,Looping
    }
    private BoostSoundStateCode boostSoundState;
    private BoostSoundStateCode BoostSoundState
    {
        get
        {
            return boostSoundState;
        }
        set
        {
            if(value != boostSoundState)
            {
                ChangeBoostStateTo(value);

            }
        }
    }

    void ChangeBoostStateTo(BoostSoundStateCode destState)
    {
        AudioClip stateSound = null;
        switch(destState)
        {
            case BoostSoundStateCode.Launching: stateSound = AudioClipLibrary.GetInstance().GetAudioFromLibrary("Engine Startup"); break;
            case BoostSoundStateCode.Looping: case BoostSoundStateCode.Stoped:   stateSound = AudioClipLibrary.GetInstance().GetAudioFromLibrary("Engine loop"); break;
        }

        audioSource.clip = stateSound;

        boostSoundState = destState;
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
        rotateState = destState;
        lerpTime = 0;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshObject = transform.Find("ShipMesh").gameObject;
        audioSource = GetComponent<AudioSource>();
        stopSoundSmooth = StopSoundSmooth();
    }

    void FixedUpdate()
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
            ProcessBoostSoundState();
        }

        if(Input.GetAxis("Vertical") != 0 && Input.GetKey(KeyCode.LeftShift))
            rigidbody.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * (movementSpeed * 5));
        else if (Input.GetAxis("Vertical") != 0)
            rigidbody.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed);
    }

    void ProcessRotateState()
    {
        if (lerpTime <= 1)
        {
            float rotateAngle = Mathf.LerpAngle(rotationStart, rotationDest, lerpTime);
            meshObject.transform.localEulerAngles = new Vector3(0, 0, rotateAngle);
            lerpTime += Time.deltaTime;
        }
    }

    void ProcessBoostSoundState()
    {
        float VerticalAxis = Input.GetAxis("Vertical");
        if (BoostSoundState == BoostSoundStateCode.Launching)
        {
            if (!audioSource.isPlaying)
            {
                BoostSoundState = BoostSoundStateCode.Looping;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (BoostSoundState == BoostSoundStateCode.Looping)
        {
            if (VerticalAxis == 0)
                BoostSoundState = BoostSoundStateCode.Stoped;
        }
        else if (BoostSoundState == BoostSoundStateCode.Stoped)
        {
            if (audioSource.isPlaying && !isSmoothing) 
                StartCoroutine(StopSoundSmooth());
            else if (VerticalAxis != 0)
            {
                BoostSoundState = BoostSoundStateCode.Launching;
                audioSource.loop = false;
                audioSource.Play();
            }
        }
    }

    IEnumerator StopSoundSmooth()
    { 
        isSmoothing = true;
        while (audioSource.volume > 0.0f && boostSoundState == BoostSoundStateCode.Stoped)
        {
            audioSource.volume -= 0.01f;
            yield return new WaitForFixedUpdate();
        }

        isSmoothing = false;
        audioSource.volume = 1.0f;
        audioSource.Stop();
        yield break;
    }
}
