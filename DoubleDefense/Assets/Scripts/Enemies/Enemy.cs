using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

// -- BASE CLASS ALL ENEMIES INHERIT FROM 
public abstract class Enemy : MonoBehaviour
{

    [Header("Health")]
    public int health;

    [Header("Movement")]
    public int moveSpeedX;
    public int moveSpeedY;

    [Header("Standard Movement")]
    public bool straightLeft;

    [Header("Sine Movement")]
    public bool sineWave;
    public float frequency;
    public float height; 

    [Header("Other")]
    public GameObject mark;
    public ParticleSystem explosionEffect;

    protected bool dying; 
    protected bool marked;
    protected Vector3 pos; 

    protected void Start()
    {
        pos = transform.position; 
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!dying)
        {
            if (straightLeft)
            {
                StraightLeftMovement();
            }
            else if (sineWave)
            {
                SineMovement();
            }
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public bool isMarked()
    {
        return this.marked;
    }

    protected IEnumerator Die()
    {
        // -- this condition prevents explosion effect from occuring multiple times 
        if (!dying)
        {
            // -- stop movement 
            dying = true;

            // -- hide sprite 
            this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
            this.mark.SetActive(false);

            // -- exploding effect
            explosionEffect.Play();

            // -- get explosion duration 
            var main = explosionEffect.main;
            yield return new WaitForSeconds(main.duration);


            GameManager.instance.AddEnemyKilled();
            Destroy(gameObject);
        }
    }

    public void ShowMark()
    {
        mark.SetActive(true);
        marked = true;
    }

    protected void StraightLeftMovement()
    {
        transform.Translate(-Vector3.right * moveSpeedX * Time.deltaTime);
    }

    protected void SineMovement()
    {
        pos -= transform.right * Time.deltaTime * moveSpeedX;
        transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * height; 
    }

}
