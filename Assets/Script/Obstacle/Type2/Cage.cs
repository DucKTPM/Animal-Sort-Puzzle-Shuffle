using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [SerializeField] private Lock _lock;
    public Lock Lock => _lock;
    public void UnLockCage()
    { _lock.Hide();
        StartCoroutine(IeUnLockCage());
       
    }

    private IEnumerator IeUnLockCage()
    {
        Debug.Log("ok");
       yield return new WaitForSeconds(0.5f);
       gameObject.SetActive(false);
    }
}
