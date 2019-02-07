using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Meteorite : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(AudioClipLibrary.GetInstance().GetAudioFromLibrary("Meteorite falling"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
