﻿using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [SerializeField] Audio[] audios = null;

    private Dictionary<string, AudioSource> BGM = new Dictionary<string, AudioSource>();
    private Dictionary<string, AudioSource> SFX = new Dictionary<string, AudioSource>();

    private bool hasBGM = true;
    private bool hasSFX = true;

    private void Awake()
    {
        if(!instance)
            instance = this;
        DontDestroyOnLoad(this);

        foreach (Audio audio in audios)
        {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            temp.clip = audio.clip;

            if (audio.type == AudioType.BGM)
                BGM.Add(audio.audioName, temp);
            else if (audio.type == AudioType.SFX)
                SFX.Add(audio.audioName, temp);
        }
    }

    public static void PlayBGM(string audioName, bool loop)
    {
        instance.BGM[audioName].Play();
        instance.BGM[audioName].loop = loop;
    }

    public static void PlaySFX(string audioName, bool loop)
    {
        instance.SFX[audioName].Play();
        instance.SFX[audioName].loop = loop;
    }

    public void ToggleMuteSFX()
    {
        hasSFX = !hasSFX;
        foreach (AudioSource source in SFX.Values)
        {
            if (hasSFX)
                source.volume = 1f;
            else
                source.volume = 0f;
        }
    }

    public void ToggleMuteBGM()
    {
        hasBGM = !hasBGM;
        foreach (AudioSource source in BGM.Values)
        {
            if (hasBGM)
                source.volume = 1f;
            else
                source.volume = 0f;
        }
    }

    public static void StopBGM(string audioName)
    {
        instance.BGM[audioName].Stop();
    }

    public static void StopSFX(string audioName)
    {
        instance.SFX[audioName].Stop();
    }

    public void PlayClick(string audioName)
    {
        instance.SFX[audioName].Play();
    }
}