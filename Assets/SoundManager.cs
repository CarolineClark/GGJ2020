using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    private static SoundManager soundManager;

    public AudioSource bgSource;
    public AudioClip bgIntro;
    public AudioClip bgLoop;

    public AudioSource fxSource;
    public AudioClip slideFx1;
    public AudioClip slideFx2;
    public AudioClip bounceFx1;

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

    public void PlaySlideFx()
    {
        if (Random.value < 0.5f) instance.fxSource.PlayOneShot(slideFx1);
        else instance.fxSource.PlayOneShot(slideFx2);
    }

    public void PlayBounceFx()
    {
        instance.fxSource.PlayOneShot(bounceFx1);
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
