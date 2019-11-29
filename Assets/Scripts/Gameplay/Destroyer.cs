using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private int destroyInSec;

    private void Start()
    {
        Invoke("Destroy", destroyInSec);
        
    }
    
    void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
