using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{   
    [SerializeField] private MenuSetting menuSetting;
   // 
    public void ShowMenuSetting()
    {
        menuSetting.Show();
      
       // 
    }

  

    public void HideMenuSetting()
    {
        
       // Time.timeScale = 1f;
        menuSetting.Hide();
       // 
    }
}
