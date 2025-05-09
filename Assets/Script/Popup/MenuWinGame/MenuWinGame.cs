using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MenuWinGame : MonoBehaviour
{
  [SerializeField] private List<Animal> listAnimal;
  [SerializeField] private Transform viewAnimal;
  
  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void Hide()
  {
    if (gameObject.activeSelf)
    {
      StartCoroutine(ScaleOut());
      DesTroyAimal();
    }
   
  }
  [SerializeField] float duration = 1f;
  
  IEnumerator ScaleOut()
  {
    float time = 0f;
    Vector3 startScale = transform.localScale;

    while (time < duration)
    {
      time += Time.deltaTime;
      float t = time / duration;
      float scale = Mathf.Max(0f, EaseInBack(1 - t)); 
      transform.localScale = Vector3.one * scale;
      yield return null;
    }

    transform.localScale = Vector3.zero;
    gameObject.SetActive(false);
  }

  float EaseInBack(float t)
  {
    float c1 = 1.70158f;
    float c3 = c1 + 1;
    return c3 * t * t * t - c1 * t * t;
  }
  private void DesTroyAimal()
  {
    if (animal != null)
    Destroy(animal.gameObject);
  }
  Animal animal = new Animal();
  public void SpawnAnimal(int indexAnimal)
  {
  
  }
}
