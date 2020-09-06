using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndWall : MonoBehaviour
{
    public int totalEnemiesAllowed;

    private int enemiesAllowed; 

    void Start()
    {
        enemiesAllowed = 0;
    }

    public void AddEnemy()
    {
        enemiesAllowed += 1;

        // -- increment text counter 
        GameManager.instance.AddEnemyAllowed(); 

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print(col); 
        Enemy enemy = col.gameObject.GetComponent<Enemy>(); 
        if(enemy)
        {
            AddEnemy(); 
        }
    }

}
