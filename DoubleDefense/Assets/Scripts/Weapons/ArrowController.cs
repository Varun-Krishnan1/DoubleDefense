using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    public float pullbackLength = .2f;


    private int damage;
    private bool isLoaded = false; 

    void Start()
    {
    }

    void Update()
    {
        
    }

    public bool getLoaded()
    {
        return isLoaded; 
    }
    public void SetDamage(int damage)
    {
        this.damage = damage; 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>(); 
        if(enemy)
        {
            enemy.TakeDamage(damage); 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            DestroyArrow(); 
        }
    }

    void DestroyArrow()
    {
        Destroy(gameObject); 
    }
    
    public void ReleaseAnimation()
    {
        this.GetComponent<Animator>().SetBool("isShot", true); 
    }

    public void LoadAnimation()
    {
        this.transform.position = this.transform.position + new Vector3(-pullbackLength, 0, 0);
        isLoaded = true; 
    }

    public void PutBackAnimation()
    {
        print("HERE"); 
        this.transform.position = this.transform.position + new Vector3(pullbackLength, 0, 0);
        isLoaded = false; 
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
