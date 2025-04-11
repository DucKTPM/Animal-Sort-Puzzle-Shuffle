using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Vector3 targetScale = Vector3.one;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(ScaleIn());
    }

    private IEnumerator ScaleIn()
    {
        float time = 0f;
        while (time<duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float scale = EaseOutBack(t);
            transform.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    private float EaseOutBack(float t)
    {
       float c1 = 1.70158f;
       float c3 = c1 + 1;
       return  1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }
   
}
