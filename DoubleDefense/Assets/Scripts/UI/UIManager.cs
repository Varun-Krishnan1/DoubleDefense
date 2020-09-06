using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // -- for singleton pattern 
    public static UIManager instance = null;
   
    [Header("UI Elements")]
    public TextMeshProUGUI totalEnemiesText;
    public TextMeshProUGUI enemiesAllowedText;
    public TextMeshProUGUI enemiesKilledText;

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
    
    public void SetWaveUI(float totalEnemiesAllowed)
    {
        totalEnemiesText.SetText(totalEnemiesAllowed.ToString());
    }

    public void SetEnemyAllowed(float enemiesAllowed)
    {
        // -- increment text counter 
        enemiesAllowedText.SetText(enemiesAllowed.ToString());
    }

    public void SetEnemyKilled(float enemiesKilled)
    {
        enemiesKilledText.SetText(enemiesKilled.ToString()); 
    }
}
