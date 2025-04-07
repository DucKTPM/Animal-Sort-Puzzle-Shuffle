using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MenuWinGame : MonoBehaviour
{
  [SerializeField] private List<Animal> listAnimal;
  [SerializeField] private GameObject viewAnimal;
  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
    DesTroyAimal();
  }

  private void DesTroyAimal()
  {
    if (animal != null)
    Destroy(animal.gameObject);
  }
  Animal animal = new Animal();
  public void SpawnAnimal(int indexAnimal)
  {
   animal = Instantiate(listAnimal[indexAnimal],viewAnimal.transform.position, Quaternion.identity);
   var scale = viewAnimal.transform.localScale;
   animal.transform.localScale = new Vector3(scale.x+0.1f, scale.y+0.1f, 1);
   animal.AddComponent<SortingGroup>();
   var sortingGroup = animal.GetComponent<SortingGroup>();
   sortingGroup.sortingOrder = 2;

  }
}
