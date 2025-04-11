using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelPopup : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI textLevelPopup;

   public void SetTextLevelPopup(string text)
   {
      textLevelPopup.text ="LEVEL: "+text;
   }
}
