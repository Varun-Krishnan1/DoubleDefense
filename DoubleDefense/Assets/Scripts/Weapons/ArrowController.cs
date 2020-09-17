using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    public float pullbackLength = .2f;


    private int damage;
    private bool isLoaded = false;
    private bool canDoDamage = false; 

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

        // -- if it attacks an enemy and it can do damage 
        if(enemy && canDoDamage)
        {
            enemy.TakeDamage(damage); 
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        BreakableShield shield = col.gameObject.GetComponent<BreakableShield>(); 
        if(shield)
        {
            // -- don't let it do damage as it's falling from a shield 
            canDoDamage = false; 
        }
    }
    
    public void ReleaseAnimation()
    {
        this.GetComponent<Animator>().SetBool("isShot", true);

        // -- now let it do damage 
        canDoDamage = true; 
    }

    public void LoadAnimation()
    {
        this.transform.position = this.transform.position + new Vector3(-pullbackLength, 0, 0);
        isLoaded = true; 
    }

    public void PutBackAnimation()
    {
        if(this.gameObject)
        {
            this.transform.position = this.transform.position + new Vector3(pullbackLength, 0, 0);
            isLoaded = false;
        }

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
