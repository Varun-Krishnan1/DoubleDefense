using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : Enemy
{

    public GameObject shield; 

    protected override void TypeSpecificDeath()
    {
        // -- this is for an end of wave kill because otherwise shield would 
        // -- already be destroyed 
        if(shield)
        {
            shield.SetActive(false); 
        }
    }

    public override string GetEnemyType()
    {
        return "shield"; 
    }
}
