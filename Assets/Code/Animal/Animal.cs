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

  public void Jump(Vector3 endPosition)
  {
    StartCoroutine(IeJump(transform.position, endPosition));
  }

  private IEnumerator IeJump(Vector3 transformPosition, Vector3 endPosition)
  {
    Vector3 startPosition = transform.position;
    float elapsedTime = 0f;
    float distance = Vector3.Distance(startPosition, endPosition);
    float height = distance * 0.1f;
    float arcOffset = distance * 0.3f;
    float time = 1f;
    while (elapsedTime < time)
    {
      elapsedTime += Time.deltaTime;
      float t = elapsedTime / time;

      // Di chuyển theo X-Z tuyến tính
      Vector3 position = Vector3.Lerp(startPosition, endPosition, t);

      // Công thức vòng cung
      position.y += height * (1 - Mathf.Cos(Mathf.PI * t));  // Hiệu ứng nhảy lên
      position.x += arcOffset * Mathf.Sin(Mathf.PI * t); // Tạo vòng cung nhẹ ngang

      transform.position = position;
      yield return null;
    }

    transform.position = endPosition; 
  }
}
