using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
 [SerializeField]  private Egg egg;
   [SerializeField] private Animator animator;
   [SerializeField] private Animal animal;
   public void SetEgg(Egg egg)
   {
      this.egg = egg;
   }

   public void SetAnimal(Animal animal)
   {
       this.animal = animal;
   }

   public void HideHammer()
   {
       gameObject.transform.parent = null;
       StartCoroutine(IeUpdateHideHamer());

   }

   private IEnumerator IeUpdateHideHamer()
   {  
       
       Vector3 pos = egg.gameObject.transform.position;
       pos.y += 0.2f;
       pos.x += 0.2f;
       while (Vector3.Distance(pos,transform.position) > 0.1f)
       {
         
           transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 1);
           yield return null;
       }
       animator.SetTrigger("BreakEgg");
       yield return new WaitForSeconds(1f);
      
       egg.BreakEgg();
       animal.SetEggEmpty();
      // yield return new WaitForSeconds(0.5f);
       Destroy(egg.gameObject);
       gameObject.SetActive(false);
       Destroy(gameObject);
   }
}
