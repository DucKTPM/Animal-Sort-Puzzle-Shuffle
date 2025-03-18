using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
  [SerializeField] private string nameAnimal;
  [SerializeField] private SpriteRenderer animalRenderer;
  
  public string getNameAnimal()
  {
    return this.nameAnimal;
  }

  public void Click()
  {
    ChangeColor();
  }

  private void ChangeColor()
  {
    animalRenderer.color = Color.yellow;
  }

  public void RemoveClickedAnimal()
  {
    animalRenderer.color = Color.white;
  }

  public void Jump(Vector3 endPosition, float h,Tree anchorTree)
  {
    gameObject.transform.SetParent(anchorTree.transform);
    StartCoroutine(IeJump(transform.position, endPosition,h, anchorTree));
  }

  private IEnumerator IeJump(Vector3 startPosition, Vector3 endPosition, float heightMultiplier,Tree anchorTree )
  {
    float elapsedTime = 0f;
    float duration = 1f; 
    float maxHeight = Vector3.Distance(startPosition, endPosition) * heightMultiplier; // Độ cao tối đa
    
    while (elapsedTime < duration)
    {
      elapsedTime += Time.deltaTime;
      float t = elapsedTime / duration; 
        

      Vector3 position = Vector3.Lerp(startPosition, endPosition, t);

      // Tính toán vị trí Y theo quỹ đạo parabol
      position.y += maxHeight - 4 * maxHeight * (t - 0.5f) * (t - 0.5f); 

      transform.position = position;
      yield return null;
    }
    
    transform.position = endPosition; // Đảm bảo đặt đúng vị trí kết thúc
    anchorTree.ShakeTree();
  }

  public void Sleep()
  {
    Click();
  }

}
