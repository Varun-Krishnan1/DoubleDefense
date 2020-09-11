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
    public float crosshair_radius; 

    [Header("Components")]
    public GameObject crosshair; 
    public BulletCounter bulletCounter; 
    public Animator animator;
    public Timer timer;

    // -- for bow to know if moving crosshair 
    public bool isMovingCrosshair;


    // -- touch and reloading 
    private Vector2 crosshairStartPos; 
    private float curReloadTime; 
    private Touch touch;  
    private int shotsLeft;

    void Start()
    {
        shotsLeft = maxShots;
        crosshairStartPos = crosshair.transform.position; 
    }

    void Update()
    {
        animator.SetBool("isShooting", false);  // -- reset animation variable after shooting 

        curReloadTime -= Time.deltaTime;

        // -- getting touch input 
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (shotsLeft > 0)
            {
                // -- when they start pressing check if they are on crosshair 
                if (touch.phase == TouchPhase.Began)
                {
                    DragStart(); 
                }

                // -- if they are on crosshair and dragging 
                if (touch.phase == TouchPhase.Moved && isMovingCrosshair)
                {
                    Dragging(); 
                }

                // -- when they release crosshair 
                if (touch.phase == TouchPhase.Ended && isMovingCrosshair)
                {
                    DragRelease(); 
                    isMovingCrosshair = false; 
                }

                // -- only drag if touching crosshair 

                //StartCoroutine(ShootAfterDelay(touchedCollider.gameObject));
                //shotsLeft -= 1;
                //bulletCounter.SetBulletNumber(shotsLeft);


                //// -- if shots are now zero then start timer
                //if (shotsLeft == 0)
                //{
                //    timer.StartTimer(reloadTime);
                //}

            }
        }
    }

    void DragStart()
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

        Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
        if (touchedCollider.gameObject == crosshair)
        {
            crosshair.transform.position = touchPosition;
            isMovingCrosshair = true;
        }
    }

    void Dragging()
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        crosshair.transform.position = touchPosition;

    }

    void DragRelease()
    {
        // -- array of enemies that is in overlap circle of crosshair 
        List<Enemy> enemies = new List<Enemy>(); 

        Collider2D[] colliders = Physics2D.OverlapCircleAll(crosshair.transform.position, crosshair_radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            print(colliders[i].gameObject); 
            if (colliders[i].gameObject != this.gameObject)
            {
                Enemy checkIfEnemy = colliders[i].gameObject.GetComponent<Enemy>(); 
                if(checkIfEnemy)
                {
                    enemies.Add(checkIfEnemy); 
                }
            }
        }

        // -- if they release on enemy then mark the enemy 
        for(int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = enemies[i]; 
            if(!enemy.isMarked())
            {
                StartCoroutine(ShootAfterDelay(enemy.gameObject));
                shotsLeft -= 1;
                bulletCounter.SetBulletNumber(shotsLeft);


                // -- if shots are now zero then start timer
                if (shotsLeft == 0)
                {
                    timer.StartTimer(reloadTime);
                }
            }
        }


        // -- move it back to start position 

        crosshair.transform.position = crosshairStartPos; 
        isMovingCrosshair = false;

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

        // -- show bullet counter at full capacity again 
        bulletCounter.SetBulletNumber(shotsLeft);

        // -- show crosshair 
        crosshair.SetActive(true);


    }

    // -- CALLED BY TIMER DO NOT CALL MANUALLY 

    public override void InActivate()
    {
        Color c = this.GetComponent<SpriteRenderer>().color;
        c.a = .5f;
        this.GetComponent<SpriteRenderer>().color = c;

        // -- hide crosshair while reloading
        crosshair.SetActive(false); 
    }
}
