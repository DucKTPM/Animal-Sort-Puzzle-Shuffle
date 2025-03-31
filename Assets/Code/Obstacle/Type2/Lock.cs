using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField]    Animator animator;

    public void Hide()
    {
        animator.SetTrigger("Fade");
    }

    public void HideLocked()
    {
        gameObject.SetActive(false);
    }
}
