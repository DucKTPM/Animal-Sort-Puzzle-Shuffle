using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyUnlock : MonoBehaviour
{
    [SerializeField] private Lock cage;
    public void HideKey()
    {
        StartCoroutine(IeEffectHideKey());
       
    }

    private IEnumerator IeEffectHideKey()
    {  
        transform.parent = null;
        while (Vector3.Distance(cage.transform.position, transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, cage.transform.position, Time.deltaTime*1f);
            yield return null;
        }
       // yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        if (GameManager.Instance.TypeEffectItem == 2)
        {
            GameManager.Instance.AnimalOnCage.UnlockCage();
        }
        else
        {
         cage.Hide();   
        }
     
        
    }

    public void SetLock(Lock cage)
    {
        this.cage = cage;
    }
    
}
