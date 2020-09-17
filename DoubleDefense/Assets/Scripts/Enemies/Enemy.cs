using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

// -- BASE CLASS ALL ENEMIES INHERIT FROM 
public abstract class Enemy : CrosshairObject
{

    [Header("Left And Jump")]
    public bool leftAndJump;
    public float jumpDelayTime;


    [Header("Crawling")]
    public bool crawling;
    public float crawlDelayTime; 


    [Header("Other")]
    public Animator animator;
    public GameObject slimeParent; 
    public ParticleSystem explosionEffect;

    // -- private booleans 
    protected bool dying; 

    // -- private floats 
    protected float jumpDelay;
    protected float crawlDelay; 
    
    // -- other 
    protected Vector3 pos; 

    protected void Start()
    {
        pos = transform.position; 
    }

    // Update is called once per frame
    protected void Update()
    {
        if(animator)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isCrawling", false);

        }

        // -- timers 
        jumpDelay -= Time.deltaTime;
        crawlDelay -= Time.deltaTime; 

        // -- movement 
        if (!dying)
        {
            if(leftAndJump && jumpDelay <= 0)
            {
                jumpDelay = jumpDelayTime; 
                LeftAndJump(); 
            }
            if(crawling && crawlDelay <= 0)
            {
                crawlDelay = crawlDelayTime;
                Crawl();
            }
        }
    }


    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(Die(endOfWaveKill: false));
        }
    }

    protected IEnumerator Die(bool endOfWaveKill)
    {

        // -- this condition prevents explosion effect from occuring multiple times 
        if (!dying)
        {

            if(!endOfWaveKill)
            {
                GameManager.instance.AddEnemyKilled();
            }
            else
            {
                GameManager.instance.AddEndOfWaveKill();
            }
            TypeSpecificDeath();

            // -- stop movement 
            dying = true;

            // -- hide sprite 
            animator.enabled = false; // -- prevents idle animation from overriding sprite renderer  
            this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
            this.mark.SetActive(false);

            // -- exploding effect
            explosionEffect.Play();

            // -- get explosion duration 
            var main = explosionEffect.main;
            yield return new WaitForSeconds(main.duration);

            // -- destroy parent container thereby destroying itself 
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    protected abstract void TypeSpecificDeath(); 


    protected void LeftAndJump()
    {
        animator.SetBool("isJumping", true);

    }

    protected void Crawl()
    {
        animator.SetBool("isCrawling", true);
    }

    public void EndOfWaveKill()
    {
        StartCoroutine(Die(endOfWaveKill: true)); 
    }

    public abstract string GetEnemyType(); 
}
