using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -- BASE CLASS ALL WEAPONS INHERIT FROM 
public abstract class Weapon : MonoBehaviour
{


    public abstract void InActivate(); 

    public abstract void Activate(); 
}