using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BreakEgg()
    {
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
