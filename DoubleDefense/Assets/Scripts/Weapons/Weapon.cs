using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -- BASE CLASS ALL WEAPONS INHERIT FROM 
public abstract class Weapon : MonoBehaviour
{

    public void InActivate()
    {
        Color c = this.GetComponent<SpriteRenderer>().color;
        c.a = .5f;
        this.GetComponent<SpriteRenderer>().color = c; 
    }

    public void Activate()
    {
        Color c = this.GetComponent<SpriteRenderer>().color;
        c.a = 1f;
        this.GetComponent<SpriteRenderer>().color = c;
    }
}