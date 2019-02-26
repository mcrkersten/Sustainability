using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    public List<NamedAudioClip> BackgroundMusics;

    //private bool isPlaying = false;
    //private string CurrentMusicName = "";
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    static public BackgroundMusicManager GetInstance()
    {
        return instance;
    }

    public void ChangeBGMTo(string name)
    {
        StopMusic();
        PlayMusic(name);
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PlayMusic(string name)
    {
        audioSource.clip = FindMusic(name).Audio;
        audioSource.Play();
    }

    private NamedAudioClip FindMusic(string name)
    {
        foreach (var music in BackgroundMusics)
        {
            if (music.Name == name) return music;
        }
        Debug.LogWarning("There is no Background Music named " + name);
        return null;
    }
}
