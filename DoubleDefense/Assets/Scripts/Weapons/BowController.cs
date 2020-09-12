﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : Weapon
{
    [Header("Bow Attributes")]
    public int damage; 
    public float power = 10f;
    public float maxDrag = 5f;
    public float reloadTime;


    [Header("Bow Components")] 
    public GameObject arrow;
    public GameObject arrowPosition; 
    public Timer timer;
    public Animator animator;
    public GunController gun; 

    // -- touch controls 
    private Vector3 dragStartPos;
    private Touch touch;

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
        animator.SetBool("isReleasing", false); // -- reset animation variable after releasing 

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

            // -- if reloaded and not on crosshair 
            if (curReloadTime < 0f && !gun.isMovingCrosshair)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    DragStart();

                    // -- animation 
                    currentArrow.GetComponent<ArrowController>().LoadAnimation();
                    animator.SetBool("isLoading", true); 
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    Dragging();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    DragRelease();

                    // -- animation 
                    animator.SetBool("isLoading", false);
                    animator.SetBool("isReleasing", true);
                }
            }

        }

    }

    void AddNewArrow()
    {
        // -- set current arrow and its associated components 
        currentArrow = Instantiate(arrow, arrowPosition.transform.position, arrowPosition.transform.rotation);
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

        // -- only do arrow if force is not (0,0) because that is a tap 
        if((Vector2)force != Vector2.zero)
        {
            // -- start arrow shooting animation 
            currentArrow.GetComponent<ArrowController>().ReleaseAnimation(); 

            // -- must change to dynamic for force to be added 
            arrowRB.isKinematic = false;
            arrowRB.AddForce(clampedForce, ForceMode2D.Impulse);

            // -- start reloading time 
            curReloadTime = reloadTime;
            reload = true;              // -- tell update function to reload on next frame 

            // -- start UI timer 
            timer.StartTimer(reloadTime);
        }
        

    }

    // -- CALLED BY TIMER DO NOT CALL MANUALLY 
    public override void Activate()
    {
        Color c = this.GetComponent<SpriteRenderer>().color;
        c.a = 1f;
        this.GetComponent<SpriteRenderer>().color = c;
    }

    // -- CALLED BY TIMER DO NOT CALL MANUALLY 

    public override void InActivate()
    {
        Color c = this.GetComponent<SpriteRenderer>().color;
        c.a = .5f;
        this.GetComponent<SpriteRenderer>().color = c;
    }
}
