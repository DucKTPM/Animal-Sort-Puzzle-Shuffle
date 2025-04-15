using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
     private int _time; 
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Animator animator;
  

    private void OnEnable()
    {
        meshRenderer.sortingLayerName = "item";
        meshRenderer.sortingOrder = 1;
    }

    public void SetValueBomb(int x)
    {
        _time = x;
        textMesh.SetText(_time.ToString());
    }

    public void BombExploed()
    {
        GameManager.Instance.StopCrountineType1();
    }

    public void SetEffectBum()
    {
       animator.SetTrigger("Bum");
       textMesh.gameObject.SetActive(false);
       StartCoroutine(IeHideBomb());
    }

    private IEnumerator IeHideBomb()
    {
        for (int i = 0; i < 16; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }
}
