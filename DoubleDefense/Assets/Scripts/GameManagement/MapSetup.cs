using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class MapSetup : MonoBehaviour
{
    // -- for singleton pattern 
    public static MapSetup instance = null;

    [Header("Components")]
    public List<GameObject> spawnerTypes; 
    public Collider2D spawnBounds;

    [Header("Setup Customization")]
    public float spawnerRefresherRateTime;

    // -- for use by game manager 
    private bool waveStarted;

    // -- list of spawners instantiated 
    private List<GameObject> spawners = new List<GameObject>();

    // -- for refreshing rate 
    private float spawnerRefresherRate;

    // -- for bounds 
    private float minX, maxX, minY, maxY;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    void SetupSpawnBounds()
    {
        // -- get bounding box of enemy spawning location using collider on spawnbounds gameobject 
        Vector3 center = spawnBounds.bounds.center;

        maxX = spawnBounds.bounds.center.x + spawnBounds.bounds.extents.x;
        minX = spawnBounds.bounds.center.x - spawnBounds.bounds.extents.x;
        maxY = spawnBounds.bounds.center.y + spawnBounds.bounds.extents.y;
        minY = spawnBounds.bounds.center.y - spawnBounds.bounds.extents.y;
    }

    public void SetupNewWave(int totalSpawners)
    {
        waveStarted = true; 

        // -- store spawn bounds in global variable 
        SetupSpawnBounds();

        int spawnersCreated = 0; 
        // -- create one of each allowable spawner types to start wave 
        for(int i = 0; i < spawnerTypes.Count; i++)
        {
            InstantiateSpawner(i);
            spawnersCreated += 1; 
        }

        for(int i = spawnersCreated; i < totalSpawners; i++)
        {
            InstantiateRandomSpawner(); 
        }
    }

    void InstantiateRandomSpawner()
    {
        // -- the int overload for the random method is exclusive on the upper range... 
        int randomSpawnerType = Random.Range(0, spawnerTypes.Count);

        InstantiateSpawner(randomSpawnerType); 
    }

    void InstantiateSpawner(int typeNumber)
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // -- spawn enemySpawner at random location 
        GameObject spawner = Instantiate(spawnerTypes[typeNumber], new Vector3(randomX, randomY, 0), Quaternion.identity);

        spawners.Add(spawner);
    }

    void MoveRandomSpawner()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        int randomSpawner = Random.Range(0, spawners.Count); 

        spawners[randomSpawner].transform.position = new Vector3(randomX, randomY, 0);
    }

    void Update()
    {
        // -- continously change location of spawners
        spawnerRefresherRate -= Time.deltaTime; 
        if(spawnerRefresherRate <= 0 && waveStarted)
        {
            MoveRandomSpawner();
            spawnerRefresherRate = spawnerRefresherRateTime; 
        }
    }

    public void DestroyWave()
    {
        this.waveStarted = false;

        // -- destroy all spawners 
        foreach(GameObject spawner in spawners)
        {
            Destroy(spawner); 
        }

        spawners.Clear();  // -- delete objects from list

        // -- destroy all enemies 
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().EndOfWaveKill(); 
        }

    }
}
