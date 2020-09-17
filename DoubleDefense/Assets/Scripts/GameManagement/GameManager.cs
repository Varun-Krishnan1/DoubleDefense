using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // -- for singleton pattern 
    public static GameManager instance = null;

    // -- get possible configurations based on wave number
    public EnemyConfigurations configs; 

    [Header("Game Statistics")]
    public int maxEnemiesAllowed;
    public int waveStartNumber;        

    private int waveNumber = 0;
    private int enemiesKilled;
    private int totalEnemiesKilled; 
    private int enemiesEndOfWaveKilled; 

    private int enemiesAllowed;
    private int totalEnemiesAllowed; 

    private int waveGoal;

    // -- this wave's characteristics 
    EnemyConfigurations.WaveConfiguration waveConfig;

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
        waveNumber = waveStartNumber - 1; 
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

        // -- get allowable configurations based on wavenumber 
        waveConfig = configs.GetAllowableTypesBasedOnWave(waveNumber);
        waveGoal = waveConfig.goal;

        StartCoroutine(UIManager.instance.NewWaveUI(waveNumber, waveGoal, maxEnemiesAllowed));
    }

    // -- CALLED BY UIMANAGER AFTER NEW WAVE UI IS DONE SHOWING 
    public void NewWaveContinue()
    {


        // -- spawner setup 
        MapSetup.instance.SetupNewWave(waveConfig);
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
