﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public Signaler PlayerHealthSignaler;
    public Signaler reduceMagic;
    public VectorValue startPosition;
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;

    public Signaler playerHit;

    [Header("Projectile stuff")]
    public GameObject projectile;
    public Item bow;

    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlash;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;

    
    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while(temp < numberOfFlash)
        {
            temp++;
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration); 
        }

        triggerCollider.enabled = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        if(startPosition != null)
        {
            transform.position = startPosition.initialValue;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(currentState == PlayerState.interact)
        {
            return;
        }
        change = Vector3.zero;
        change.x = Input.GetAxis("Horizontal");
        change.y = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
            //Debug.Log("moveX: " + animator.GetFloat("moveX") + "|| moveY: " + animator.GetFloat("moveY"));
        }
        else if (Input.GetButtonDown("SecondWeapon") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            if(playerInventory.CheckItem(bow))
            {
                StartCoroutine(SecondAttackCo());
            }
            else
            {
                Debug.Log("我没有金刚钻 我不拦瓷器活");
            }
            //Debug.Log("moveX: " + animator.GetFloat("moveX") + "|| moveY: " + animator.GetFloat("moveY"));
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking",true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);        
        if(currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private IEnumerator SecondAttackCo()
    {
        //animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        MakeArrow();
        //animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private Vector3 getRotation()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    private void MakeArrow()
    {
        if (playerInventory.currentMagic >= projectile.GetComponent<Arrow>().magicCost)
        {
            Arrow arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY")), getRotation());
            playerInventory.ReduceMagic(arrow.magicCost);
            reduceMagic.Raise();
        }
    }

    public void RaiseItem()
    {
        if(playerInventory.currentItem != null)
        {
            if (currentState != PlayerState.interact)
            {
                animator.SetBool("receiveItem", true);
                currentState = PlayerState.interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("receiveItem", false);
                currentState = PlayerState.idle;
                receivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }        
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            change.x = Mathf.Round(change.x);
            change.y = Mathf.Round(change.y);
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }

    public void Knock(float knockTime, float damage)
    {        
        currentHealth.runtimeValue -= damage;

        PlayerHealthSignaler.Raise();
        //Debug.Log("进入PlayerMovement内的Knock方法,  currentHealth.runtimeValue : " + currentHealth.runtimeValue);

        playerHit.Raise();

        if (currentHealth.runtimeValue > 0)
        {
            //Debug.Log("启动玩家生命信号");
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null )
        {
            StartCoroutine(FlashCo());
            //Debug.Log("延迟方法执行");
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
        }
    }
}
