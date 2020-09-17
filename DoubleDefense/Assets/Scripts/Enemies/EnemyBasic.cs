using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : Enemy
{
    protected override void TypeSpecificDeath()
    {

    }

    public override string GetEnemyType()
    {
        if(this.leftAndJump)
        {
            return "leftAndJump"; 
        }
        else
        {
            return "crawling"; 
        }
    }
}
