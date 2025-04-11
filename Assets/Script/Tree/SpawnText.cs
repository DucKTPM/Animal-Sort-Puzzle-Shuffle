using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnText : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    
    public float duration = 0.6f;
    private float timer = 0f;
    private Vector3 targetScale;

    void OnEnable()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float scale = Mathf.Sin(t * Mathf.PI); // pop effect
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale * 1.2f, scale);
        }
    }
    public void Show()
    {
        var pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x >=0.5f)
        {
            transform.rotation = Quaternion.Euler(0f, 18f, 0f);
        }
        int rand = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[rand];
        
        
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
