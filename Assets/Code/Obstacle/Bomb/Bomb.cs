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
        Debug.Log("bummm");
    }

    
}
