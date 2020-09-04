using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public float spawnRate;
    public GameObject enemy; 

    private float spawnRateTime; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnRateTime -= Time.deltaTime; 
        if(spawnRateTime <= 0)
        {
            Instantiate(enemy, this.transform.position, this.transform.rotation);
            spawnRateTime = spawnRate; 
        }
    }
}
