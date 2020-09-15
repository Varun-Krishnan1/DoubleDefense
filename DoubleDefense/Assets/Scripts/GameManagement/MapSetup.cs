using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetup : MonoBehaviour
{
    // -- for singleton pattern 
    public static MapSetup instance = null;

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

    public void SetupNewWave()
    {

    }
}
