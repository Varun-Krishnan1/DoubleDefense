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


    private int waveNumber;
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

    private void EndGame()
    {
        print("GAME OVER"); 
    }

    void Update()
    {
        if(!UIset)
        {
            UIManager.instance.SetWaveUI(totalEnemiesAllowed);
            UIset = true; 
        }
    }
}
