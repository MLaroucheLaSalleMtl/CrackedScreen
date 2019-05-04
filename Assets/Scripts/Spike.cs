﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            PlayerProperty.playerClass.TakeDamage(10);
            PlayerProperty.playerClass.GetKnockOff(transform.position);
        }
    }
}
