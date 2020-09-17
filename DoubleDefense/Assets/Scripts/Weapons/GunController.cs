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
    public Animator explosionAnimator; 
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
        explosionAnimator.SetBool("isShooting", false);


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
            }
        }
    }

    void DragStart()
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

        Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
        
        // -- so we don't get a null reference exception 
        if(touchedCollider)
        {
            if (touchedCollider.gameObject == crosshair)
            {
                crosshair.transform.position = touchPosition;
                isMovingCrosshair = true;
            }
        }

    }

    void Dragging()
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        crosshair.transform.position = touchPosition;

    }

    void DragRelease()
    {
        // -- array of colliders that is in overlap circle of crosshair 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(crosshair.transform.position, crosshair_radius);

        // -- determine what to do to overlapped colliders 
        CrosshairReleaseAction(colliders); 

    }

    void CrosshairReleaseAction(Collider2D[] colliders)
    {
        // -- array of enemies that is in overlap circle of crosshair 
        //List<Enemy> enemies = new List<Enemy>();

        CrosshairObject firstObjectFound = null; 

        for (int i = 0; i < colliders.Length; i++)
        {
            CrosshairObject crossObject = colliders[i].gameObject.GetComponent<CrosshairObject>();

            // -- if it is a crosshair object 
            if (crossObject)
            {
                BreakableShield shield = crossObject.GetComponent<BreakableShield>(); 
                if(shield)
                {
                    // -- override first object with shield then break loop 
                    firstObjectFound = shield;
                    break; 
                }
            }

            // -- if you haven't got a first object set it to this one 
            if(!firstObjectFound)
            {
                firstObjectFound = crossObject; 
            }
        }

        // -- if it is targetable and it's not already marked and you have enough shots left 
        if (firstObjectFound.isTargetable() && !firstObjectFound.isMarked() && shotsLeft > 0)
        {
            StartCoroutine(ShootAfterDelay(firstObjectFound));
            shotsLeft -= 1;
            bulletCounter.SetBulletNumber(shotsLeft);
        }

        // -- if shots are now zero then start timer
        if (shotsLeft == 0)
        {
            timer.StartTimer(reloadTime);
        }

        // -- move it back to start position 
        crosshair.transform.position = crosshairStartPos;
    }

    // -- adds mark then waits a delay time then shoots 
    private IEnumerator ShootAfterDelay(CrosshairObject crossObject)
    {
        crossObject.ShowMark(); 
        yield return new WaitForSeconds(timeTillShot);
        ShootObject(crossObject); 
    }

    // -- actual shooting in this function 
    private void ShootObject(CrosshairObject crossObject)
    {
        // -- gun animations 
        animator.SetBool("isShooting", true);
        explosionAnimator.SetBool("isShooting", true); 

        // -- if still there 
        if(crossObject)
        {
            crossObject.TakeDamage(damage);
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
