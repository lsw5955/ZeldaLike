﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerup : Powerup
{
    public Inventory playerInventory;
    public float magicValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.currentMagic += magicValue;
            if(playerInventory.currentMagic > playerInventory.maxMagic)
            {
                playerInventory.currentMagic = playerInventory.maxMagic;
            }

            powerupSignal.Raise();
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
