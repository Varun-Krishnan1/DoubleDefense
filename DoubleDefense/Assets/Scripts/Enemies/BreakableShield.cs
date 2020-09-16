using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableShield : CrosshairObject
{
    public Enemy parentSlime; 

    public void Break()
    {
        // -- allow parent to now be targetable by gun 
        parentSlime.isTarget = true; 

        // -- destroy it 
        Destroy(this.gameObject); 
    }

    public override void TakeDamage(int damage)
    {
        health -= 1; 
        if(health <= 0)
        {
            this.Break(); 
        }
    }
}
