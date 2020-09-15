using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public float spawnRate;
    public GameObject enemy; 

    private float spawnRateTime;
    private bool isSpawning = true; 

    // Update is called once per frame
    void Update()
    {
        spawnRateTime -= Time.deltaTime; 

        if(isSpawning)
        {
            if (spawnRateTime <= 0)
            {
                Instantiate(enemy, this.transform.position, this.transform.rotation);
                spawnRateTime = spawnRate;
            }
        }

    }

    public void StartSpawning()
    {
        this.isSpawning = true; 
    }

    public void StopSpawning()
    {
        this.isSpawning = false; 
    }
}
