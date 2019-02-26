using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NamedAudioClip
{
    [SerializeField]
    private AudioClip audioClip;
    public AudioClip Audio
    {
        get
        {
            return audioClip;
        }
        set {
            audioClip = Audio;
        }
    }

    [SerializeField]
    private string name;
    public string Name
    {
        get
        {
            return name;
        }
        set {
            name = Name;
        }
    }

    
}
