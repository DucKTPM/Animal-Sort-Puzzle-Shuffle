using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
public class AnimationButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject buttonPrefab;

    private Vector3 originalScale;
    private Coroutine scaleCoroutine;

    void Start()
    {
        if (buttonPrefab != null)
        {
            originalScale = buttonPrefab.transform.localScale;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonPrefab != null)
        {
            if (scaleCoroutine != null)
                StopCoroutine(scaleCoroutine);

            scaleCoroutine = StartCoroutine(ScaleButton(originalScale, originalScale * 0.9f, 0.1f));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonPrefab != null)
        {
            if (scaleCoroutine != null)
                StopCoroutine(scaleCoroutine);

            scaleCoroutine = StartCoroutine(ScaleButton(buttonPrefab.transform.localScale, originalScale, 0.1f));
        }
    }

    private IEnumerator ScaleButton(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            buttonPrefab.transform.localScale = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        buttonPrefab.transform.localScale = to;
    }
}