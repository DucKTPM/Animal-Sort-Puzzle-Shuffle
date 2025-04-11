using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSourceBackground;
    [SerializeField] private AudioSource audioSourceEffect;
    [SerializeField] private AudioClip backGroundAudio1;
    [SerializeField] private AudioClip backGroundAudio2;
    [SerializeField] private AudioClip backGroundAudio3;
    [SerializeField] private AudioClip addCoinAudio;
    [SerializeField] private AudioClip jumpAudio;
    [SerializeField] private AudioClip jumpedAudio;
    [SerializeField] private AudioClip clickAudio;
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip loseAudio;
    [SerializeField] private AudioClip jumpOut;


    private void OnEnable()
    {
	    instance = this;
        PlayBackGroundSound();
    }

    public void PlayLoseAudio()
    {
        audioSourceEffect.PlayOneShot(loseAudio);
    }

    public void PlayWinAudio()
    {
        audioSourceEffect.PlayOneShot(winAudio);
    }

    public void PlayClickSound()
    {
        audioSourceEffect.PlayOneShot(clickAudio);
    }

    private void PlayBackGroundSound()
    {
       audioSourceBackground.clip = backGroundAudio1;
       audioSourceBackground.Play();
       audioSourceEffect.loop = true;
       StartCoroutine(IePlayAudioBase());
    }

    private IEnumerator IePlayAudioBase()
    { audioSourceEffect.PlayOneShot(backGroundAudio2);
        while (true)
        {
            yield return new WaitForSeconds(50);
            audioSourceEffect.PlayOneShot(backGroundAudio2);
        }
    }


    public void PlayAddCoin()
    {
        Debug.Log("Play Add Coin");
        audioSourceEffect.PlayOneShot(addCoinAudio);
    }
    
    public void PlayJump()
    {
        audioSourceEffect.PlayOneShot(jumpAudio);
    }

    public void PlayJumped()
    {
        audioSourceEffect.PlayOneShot(jumpedAudio);
    }

    public void JumOut()
    {
        audioSourceEffect.PlayOneShot(jumpOut);
    }
}
