using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // -- for singleton pattern 
    public static GameManager instance = null;

    [Header("Game Statistics")]
    public int maxEnemiesAllowed;
    public float timeBetweenSubWaves; 


    private int waveNumber = 0;
    private int enemiesKilled;
    private int totalEnemiesKilled; 
    private int enemiesEndOfWaveKilled; 

    private int enemiesAllowed;
    private int totalEnemiesAllowed; 

    private int waveGoal; 

    // ---- START METHODS ----

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

    void Start()
    {
        NewWave(); 
    }

    // ---- MAIN GAME LOOP METHODS ----
    public void NewWave()
    {
        // -- keep track of total statistics for end of game 
        totalEnemiesKilled += enemiesKilled;
        totalEnemiesAllowed += enemiesAllowed;

        // -- reset other variables  
        enemiesKilled = 0;
        enemiesAllowed = 0;

        waveNumber += 1;
        waveGoal += 5;

        StartCoroutine(UIManager.instance.NewWaveUI(waveNumber, waveGoal, maxEnemiesAllowed));
    }

    // -- CALLED BY UIMANAGER AFTER NEW WAVE UI IS DONE SHOWING 
    public void NewWaveContinue()
    {
        // -- spawner setup 
        MapSetup.instance.SetupNewWave(waveNumber, 2, timeBetweenSubWaves);
    }

    private void EndGame()
    {
        print("GAME OVER");
    }


    // ---- HELPER METHODS ----

    public void AddEnemyAllowed()
    {
        enemiesAllowed += 1;

        UIManager.instance.SetEnemyAllowed(enemiesAllowed);

        if (enemiesAllowed == maxEnemiesAllowed)
        {
            EndGame(); 
        }
    }

    public void AddEnemyKilled()
    {
        enemiesKilled += 1;

        UIManager.instance.SetEnemyKilled(enemiesKilled);

        if(enemiesKilled == waveGoal)
        {
            MapSetup.instance.DestroyWave();
            NewWave(); 
        }
    }

    public void AddEndOfWaveKill()
    {
        enemiesEndOfWaveKilled += 1; 
    }






}
