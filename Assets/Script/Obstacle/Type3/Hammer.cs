using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
 [SerializeField]  private Egg egg;
   [SerializeField] private Animator animator;
   public void SetEgg(Egg egg)
   {
      this.egg = egg;
   }

   public void HideHammer()
   {
       gameObject.transform.parent = null;
       StartCoroutine(IeUpdateHideHamer());

   }

   private IEnumerator IeUpdateHideHamer()
   {  
       
       Vector3 pos = egg.gameObject.transform.position;
       pos.y += 0.5f;
       while (Vector3.Distance(pos,transform.position) > 0.1f)
       {
         
           transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 1);
           yield return null;
       }
       animator.SetTrigger("BreakEgg");
       yield return new WaitForSeconds(1f);
       egg.BreakEgg();
       yield return new WaitForSeconds(1f);
       gameObject.SetActive(false);
       Destroy(egg.gameObject);
   }
}
