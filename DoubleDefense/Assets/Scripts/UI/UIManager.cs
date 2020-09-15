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
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI totalEnemiesText;
    public TextMeshProUGUI enemiesAllowedText;
    public TextMeshProUGUI enemiesKilledText;

    [Header("UI Customizations")]
    public float newWaveShowTime;
    public float newWaveExitTime;

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

    public IEnumerator NewWaveUI(int waveNumber)
    {
        waveNumberText.gameObject.SetActive(true);
        waveNumberText.SetText("Wave " + waveNumber.ToString());

        yield return new WaitForSeconds(newWaveShowTime);

        waveNumberText.GetComponent<Animator>().SetBool("isExiting", true);

        // -- get's length of exitTime animation which is assumed to be second on the animation list 
        float animLength = waveNumberText.GetComponent<Animator>().runtimeAnimatorController.animationClips[1].length;

        yield return new WaitForSeconds(animLength);

        waveNumberText.gameObject.SetActive(false);


        GameManager.instance.NewWaveContinue(); 

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
