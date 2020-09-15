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
    public int totalEnemiesAllowed; 


    private int waveNumber = 0;
    private int enemiesKilled;
    private int enemiesAllowed;
    private bool UIset = false; 

    // Start is called before the first frame update
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

    public void AddEnemyAllowed()
    {
        enemiesAllowed += 1;

        UIManager.instance.SetEnemyAllowed(enemiesAllowed);

        if (enemiesAllowed == totalEnemiesAllowed)
        {
            EndGame(); 
        }
    }

    public void AddEnemyKilled()
    {
        enemiesKilled += 1;

        UIManager.instance.SetEnemyKilled(enemiesKilled);
    }


    public void NewWave()
    {
        waveNumber += 1;

        StartCoroutine(UIManager.instance.NewWaveUI(waveNumber));  
    }

    // -- CALLED BY UIMANAGER AFTER NEW WAVE UI IS DONE SHOWING 
    public void NewWaveContinue()
    {
        // -- spawner setup 
        MapSetup.instance.SetupNewWave(); 
    }

    private void EndGame()
    {
        print("GAME OVER"); 
    }

    void Update()
    {
        // -- have to do this in first frame of update so UIManager is loaded into game 
        if(!UIset)
        {
            UIManager.instance.SetWaveUI(totalEnemiesAllowed);
            UIset = true; 
        }
    }

}
