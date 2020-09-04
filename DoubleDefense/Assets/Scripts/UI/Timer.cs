using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Timer : MonoBehaviour
{
    public GameObject fxHolder;
    public Image timerImage;

    private float progress = 0f; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timerImage.fillAmount = progress;
        fxHolder.rotation = Quaternion.Euler(new Vector3(0f, 0f, -progress * 360)); 
    }

    void StartTimer()
    {

    }
}
