using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipLibrary : MonoBehaviour
{
    public static AudioClipLibrary instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    public List<NamedAudioClip> BackgroundMusics;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    static public AudioClipLibrary GetInstance()
    {
        return instance;
    }

    public AudioClip GetAudioFromLibrary(string name)
    {
        foreach (var music in BackgroundMusics)
        {
            if (music.Name == name) return music.Audio;
        }
        Debug.LogWarning("There is no Background Music named " + name);
        return null;
    }
}

