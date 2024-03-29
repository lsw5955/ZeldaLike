﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public Rigidbody2D myRigidbody;
    public float lifeTime;
    private float lifeTimeCounter;
    public float magicCost;

    private Vector3 startPosition;

    private void Start()
    {
        lifeTimeCounter = lifeTime;
    }

    public void Setup(Vector2 velocity, Vector3 direction)
    {
        myRigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
        startPosition = transform.position;
    }

    private void Update()
    {
        lifeTimeCounter -= Time.deltaTime;

        if(lifeTimeCounter <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("enemy"))
        {
            Destroy(this.gameObject);
        }        
    }
}
