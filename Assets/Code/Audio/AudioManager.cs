using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSourceBackground;
    [SerializeField] private AudioSource audioSourceEffect;
    [SerializeField] private AudioClip backGroundAudio1;
    [SerializeField] private AudioClip backGroundAudio2;
    [SerializeField] private AudioClip backGroundAudio3;


    private void OnEnable()
    {
	    instance = this;
        PlayBackGroundSound();
    }

    private void PlayBackGroundSound()
    {
       audioSourceBackground.clip = backGroundAudio1;
       audioSourceBackground.Play();
       audioSourceEffect.loop = true;
    }
}
