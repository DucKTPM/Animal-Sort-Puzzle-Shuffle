using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSetting : MonoBehaviour
{
    [SerializeField] private GameObject overLayBlocker;

    public void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            GameManager.Instance.PauseGame = true;
            overLayBlocker.SetActive(true);
        }
    }

    public void Hide()
    {
       // gameObject.SetActive(false);
        if (gameObject.activeSelf)
        {
            StartCoroutine(ScaleOut());
            overLayBlocker.SetActive(false);
            GameManager.Instance.PauseGame = false; 
        }
    }
    [SerializeField] float duration = 0.3f;
  
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
}
