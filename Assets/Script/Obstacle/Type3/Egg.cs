using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
   [SerializeField] Animator animator;
    public void BreakEgg()
    {   
        animator.SetTrigger("Break");
    }


  
}
