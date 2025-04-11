using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageTree : MonoBehaviour
{
    [SerializeField] private Lock _lock1;
    [SerializeField] private Lock _lock2;
    
    public Lock Lock1 => _lock1;
    
    public Lock Lock2 => _lock2;
}