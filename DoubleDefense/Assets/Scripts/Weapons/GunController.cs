using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : Weapon
{
    [Header("Gun Attributes")]
    public int damage;
    public int maxShots; 
    public float reloadTime;
    public float timeTillShot;

    [Header("Components")]
    public BulletCounter bulletCounter; 
    public Animator animator;
    public Timer timer; 

    // -- touch and reloading 
    private float curReloadTime; 
    private Touch touch;  
    private int shotsLeft;

    void Start()
    {
        shotsLeft = maxShots; 
    }

    void Update()
    {
        animator.SetBool("isShooting", false);  // -- reset animation variable after shooting 

        curReloadTime -= Time.deltaTime; 

        // -- getting touch input 
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (shotsLeft > 0 && touch.phase == TouchPhase.Ended)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                // -- only shoot if touching enemy 
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                Enemy enemy = touchedCollider.gameObject.GetComponent<Enemy>(); 
                if (enemy && !enemy.isMarked())
                {
                    StartCoroutine(ShootAfterDelay(touchedCollider.gameObject));
                    shotsLeft -= 1;
                    bulletCounter.SetBulletNumber(shotsLeft); 


                    // -- if shots are now zero then start timer
                    if(shotsLeft == 0)
                    {
                        timer.StartTimer(reloadTime);
                    }

                }
            }
        }
    }

    private IEnumerator ShootAfterDelay(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().ShowMark(); 
        yield return new WaitForSeconds(timeTillShot);
        ShootEnemy(enemy); 
    }

    private void ShootEnemy(GameObject enemy)
    {
        animator.SetBool("isShooting", true); 
        if(enemy)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    // -- CALLED BY TIMER DO NOT CALL MANUALLY 
    public override void Activate()
    {
        Color c = this.GetComponent<SpriteRenderer>().color;
        c.a = 1f;
        this.GetComponent<SpriteRenderer>().color = c;
        this.shotsLeft = maxShots;

        bulletCounter.SetBulletNumber(shotsLeft);

    }
}
