﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            other.GetComponent<BoxCollider2D>().isTrigger = true;
            other.GetComponent<pot>().smash();
        }
        else if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if(hit!=null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);

                if (other.gameObject.CompareTag("enemy") && other.isTrigger)
                {
                    //Debug.Log("我执行了!");
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    hit.GetComponent<Enemy>().Knock(hit, knockTime,damage);
                }
                else if (other.gameObject.CompareTag("Player"))
                {
                    //Debug.Log("我撞了玩家");
                    if (other.GetComponent<PlayerMovement>().currentState != PlayerState.stagger)
                    {
                        //Debug.Log("玩家开始滑行");
                        hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                        hit.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                    }
                }
            }
        }
    }
}
