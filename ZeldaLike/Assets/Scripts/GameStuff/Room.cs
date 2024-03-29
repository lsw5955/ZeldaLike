﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Enemy[] enemies;
    public pot[] pots;
    public MagicPowerup[] mp;
    public GameObject virtualCamera;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            foreach (Enemy em in enemies)
            {
                ChangeActivation(em, true);
            }

            foreach (pot p in pots)
            {
                ChangeActivation(p, true);
            }

            foreach (MagicPowerup m in mp)
            {
                ChangeActivation(m, true);
            }

            virtualCamera.SetActive(true);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            foreach (Enemy em in enemies)
            {
                ChangeActivation(em, false);
            }

            foreach (pot p in pots)
            {
                ChangeActivation(p, false);
            }
            
            foreach (MagicPowerup m in mp)
            {
                ChangeActivation(m, false);
            }

            virtualCamera.SetActive(false);
        }
    }

    public void ChangeActivation(Component component, bool activation)
    {
        component.gameObject.SetActive(activation);
    }
}
