using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager soundManager;
    public AudioSource bgSource;
    public AudioClip bgIntro;
    public AudioClip bgLoop;

    public static SoundManager instance
    {
        get
        {
            if (soundManager) return soundManager;
            soundManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;
            if (!soundManager) Debug.LogError("Need an active SoundManager on a GameObject");
            return soundManager;
        }
    }

    private void Start()
    {
        instance.StartBgMusic();
    }

    void Update()
    {
        if (!instance.bgSource.isPlaying)
        {
            instance.LoopBgMusic();
        }
    }

    private void StartBgMusic()
    {
        instance.bgSource.clip = bgIntro;
        instance.bgSource.Play();
    }

    private void LoopBgMusic()
    {
        instance.bgSource.loop = true;
        instance.bgSource.clip = bgLoop;
        instance.bgSource.Play();
    }
}
