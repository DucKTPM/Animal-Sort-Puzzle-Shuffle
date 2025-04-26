using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGame : MonoBehaviour
{
    [SerializeField] private GameObject gameViewWhite;
    [SerializeField] private GameObject gameViewBlack;
    [SerializeField] private AudioSource audioEffect;
    bool flag = true;
    public void Click()
    {
        if (audioEffect!=null)
        {
            audioEffect.mute = !audioEffect.mute;
        }
       
        if (flag)
        {
         gameViewWhite.SetActive(false);
         gameViewBlack.SetActive(true);
         flag = false;
        
       
        }
        else
        {
            gameViewWhite.SetActive(true);
            gameViewBlack.SetActive(false);
            flag = true;
        }
    }
}
