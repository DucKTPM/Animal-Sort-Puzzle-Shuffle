using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public void UnLockCage()
    {
        gameObject.SetActive(false);
    }
}
