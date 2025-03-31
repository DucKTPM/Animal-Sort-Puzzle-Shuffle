using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [SerializeField] private Lock _lock;
    public Lock Lock => _lock;
    public void UnLockCage()
    {
        StartCoroutine(IeUnLockCage());
        _lock.Hide();
    }

    private IEnumerator IeUnLockCage()
    {
       yield return new WaitForSeconds(0.5f);
       gameObject.SetActive(false);
    }
}
