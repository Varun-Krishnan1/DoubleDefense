using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Timer : MonoBehaviour
{
    public GameObject fxHolder;
    public Image timerImage;
    public Weapon weapon; 

    private float progress = 0f;
    private float maxTime; 
    private float duration;
    private bool isRunning; 

    // Start is called before the first frame update
    void Start()
    {
        timerImage.fillAmount = 0;
        //StartTimer(3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        
        if (duration > 0)
        {
            progress = 1 - (duration / maxTime); 
            timerImage.fillAmount = progress;
            fxHolder.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -progress * 360));
        }
        else if (isRunning)
        {
            timerImage.fillAmount = 0f;
            this.isRunning = false;
            EndTimer(); 
        }

    }

    public void StartTimer(float duration)
    {
        weapon.InActivate(); 
        this.maxTime = duration;
        this.duration = duration;
        this.isRunning = true; 
    }

    void EndTimer()
    {
        weapon.Activate(); 
    }
}
