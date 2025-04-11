using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
   [SerializeField] private int numberOfCoins;
   
   
   public int NumberOfCoins => numberOfCoins;

   private void OnEnable()
   {
      StartCoroutine(MoveToTotalCoin());
   }
   public float angle = 15f;        
   public float speed = 15f;        
   private float t = 0f;

   void Update()
   {
      t += Time.deltaTime * speed;
      float rotationZ = Mathf.Sin(t) * angle;
      transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
   } 
   private IEnumerator MoveToTotalCoin()
   {  
      yield return new WaitForSeconds(0.5f);
      Vector3 anchorGameManager = GameManager.Instance.totalPanel.transform.position;
      while (Vector3.Distance(transform.position, anchorGameManager) > 0.1f)
      {
         transform.position = Vector3.MoveTowards(transform.position, anchorGameManager, Time.deltaTime*200f);
         yield return null;
         
      }
      GameManager.Instance.AddCoin(numberOfCoins);
      Destroy(gameObject);
   }
}
