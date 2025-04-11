using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    public void Hide()
    { if (gameObject.activeSelf)
        gameObject.SetActive(false);
    }

    public void Show()
    {
       
        gameObject.SetActive(true);
    }
}
