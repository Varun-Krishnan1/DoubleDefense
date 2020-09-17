using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;

public class ConfigGenerator : MonoBehaviour
{
    public Text textObj;
    public Text secondsObj;
    public Text timer; 

    // Start is called before the first frame update
    void Start()
    {


    }

    public void SaveConfig()
    {
        // -- find all spawners 
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

        // -- get filename from input box 
        string filename = textObj.text;

        // -- get seconds from input box 
        float seconds = float.Parse(secondsObj.text);

        // -- load spawners into save file 
        ConfigSaveLoad.instance.SaveConfigFile(spawners, seconds, filename);

        //ConfigSaveLoad.instance.LoadConfigFile(Application.persistentDataPath + "/config.dat"); 
    }

    void Update()
    {
        // -- on screen timer 
        timer.text = Time.timeSinceLevelLoad.ToString(); 
    }
}
