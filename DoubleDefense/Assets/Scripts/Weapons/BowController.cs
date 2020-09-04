using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : Weapon
{
    [Header("Bow Attributes")]
    public int damage; 
    public float power = 10f;
    public float maxDrag = 5f;
    public float reloadTime;
    public Collider2D col;
    public GameObject arrow;
    public Timer timer; 

    // -- touch controls 
    private Vector3 dragStartPos;
    private Touch touch;
    private bool moveAllowed;

    // -- reloading 
    private float curReloadTime;
    private bool reload;

    // -- arrow 
    private GameObject currentArrow;
    private LineRenderer lr;
    private Rigidbody2D arrowRB; 


    void Start()
    {
        AddNewArrow(); 
    }

    void Update()
    {

        //for(int i = 0; i < Input.touchCount; i++)
        //{
        //    Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
        //    //Debug.DrawLine(Vector3.zero, touchPos, Color.red); 
        //    touchPos.z = 0; 
        //    transform.position = touchPos; 
        //}

        // -- reloading 
        curReloadTime -= Time.deltaTime; 

        if(reload && curReloadTime < 0f)
        {
            AddNewArrow(); 

            reload = false; 
        }

        // -- getting touch input 
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            // -- if reloaded 
            if (curReloadTime < 0f)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    
                    // -- only move if touching in area of bow 
                    Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                    if (col == touchedCollider)
                    {
                        moveAllowed = true;
                        DragStart();
                    }
                }

                if (touch.phase == TouchPhase.Moved && moveAllowed)
                {
                    Dragging();
                }

                if (touch.phase == TouchPhase.Ended && moveAllowed)
                {
                    DragRelease();
                    moveAllowed = false;

                    // -- start reloading time 
                    curReloadTime = reloadTime;
                    reload = true;              // -- tell update function to reload on next frame 
                }
            }

        }

    }

    void AddNewArrow()
    {
        // -- set current arrow and its associated components 
        currentArrow = Instantiate(arrow, this.transform.position, this.transform.rotation);
        currentArrow.GetComponent<ArrowController>().SetDamage(damage); 
        lr = currentArrow.GetComponent<LineRenderer>();
        arrowRB = currentArrow.GetComponent<Rigidbody2D>();
    }


    void DragStart()
    {

        dragStartPos = Camera.main.ScreenToWorldPoint(touch.position);
        dragStartPos.z = 0f;
        lr.positionCount = 1;
        lr.SetPosition(0, dragStartPos); 
    }

    void Dragging()
    {
        
        Vector3 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
        draggingPos.z = 0f;

        // -- rotation 
        float angle = Mathf.Atan2(dragStartPos.y - draggingPos.y, dragStartPos.x - draggingPos.x) * Mathf.Rad2Deg; 
        currentArrow.transform.rotation = Quaternion.Euler(0, 0, angle); 

        // -- line renderer 
        lr.positionCount = 2;
        lr.SetPosition(1, draggingPos); 
    }

    void DragRelease()
    {
        lr.positionCount = 0; 

        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(touch.position);
        dragReleasePos.z = 0f;
        Vector3 force = dragStartPos - dragReleasePos;
        Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;

        // -- must change to dynamic for force to be added 
        arrowRB.isKinematic = false;
        arrowRB.AddForce(clampedForce, ForceMode2D.Impulse);

        // -- start UI timer 
        timer.StartTimer(reloadTime); 
    }
}
