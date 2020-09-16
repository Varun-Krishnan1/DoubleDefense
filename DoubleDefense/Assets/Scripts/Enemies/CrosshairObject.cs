using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -- BASE CLASS FOR ALL OBJECTS THAT CAN BE TARGETED BY THE CROSSHAIR 
public abstract class CrosshairObject : MonoBehaviour
{
    [Header("CrosshairObject")]
    public GameObject mark;
    public int health;
    public bool isTarget = true; 

    protected bool marked;

    public bool isMarked()
    {
        return this.marked;
    }


    public void ShowMark()
    {
        mark.SetActive(true);
        marked = true;
    }

    public abstract void TakeDamage(int damage); 

    public bool isTargetable()
    {
        return this.isTarget; 
    }
}
