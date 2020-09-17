using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System; 

public class ConfigSaveLoad : MonoBehaviour
{
    // -- for singleton pattern 
    public static ConfigSaveLoad instance = null;

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
    
    public ConfigurationObject LoadConfigFile(string filename)
    {
        if (!File.Exists(filename))
        {
            throw new Exception("Configuration File Not Found!");
        }
        BinaryFormatter bf = new BinaryFormatter();

        // -- loading data 
        FileStream loadFile = File.Open(filename, FileMode.Open);
        ConfigurationObject loadedConfObject = bf.Deserialize(loadFile) as ConfigurationObject;
        loadFile.Close();


        // -- printing out data to double check 
        //for (int i = 0; i < loadedConfObject.spawners.Count; i++)
        //{
        //    SpawnerSaveObject loadedSO = loadedConfObject.spawners[i];
        //    print("Spawner " + i + ":");
        //    print(loadedSO.spawnRate);
        //    print(loadedSO.position[0]);
        //    print(loadedSO.position[1]);
        //    print(loadedSO.enemyType);
        //}

        return loadedConfObject; 
    }

    public void SaveConfigFile(GameObject[] spawners, float spawnLength, string filename)
    {
        // -- create config object to save 
        ConfigurationObject confObject = new ConfigurationObject();

        // -- for each spawner 
        foreach (GameObject spawner in spawners)
        {
            // -- create new save object of that spawner 
            SpawnerSaveObject so = new SpawnerSaveObject();

            // -- add spawner variables -- 

            // spawn rate 
            so.spawnRate = (float)spawner.GetComponent<EnemySpawner>().spawnRate;

            // 2D position 
            so.position[0] = spawner.gameObject.transform.position.x;
            so.position[1] = spawner.gameObject.transform.position.y;

            // enemy type (must get child because parent is container) 
            so.enemyType = spawner.GetComponent<EnemySpawner>().enemy
                .transform.GetChild(0).GetComponent<Enemy>().GetEnemyType();

            // -- add object to configuration object 
            confObject.spawners.Add(so);
        }

        // -- spawn length 
        confObject.spawnLength = spawnLength; 

        // -- saving data 
        BinaryFormatter bf = new BinaryFormatter();
        string savepath = Application.persistentDataPath + "/" + filename + ".bytes"; 
        FileStream saveFile = File.Create(savepath);
        bf.Serialize(saveFile, confObject);
        saveFile.Close();

        print("Configuration saved in: " + savepath); 
    }

    [Serializable]
    public class SpawnerSaveObject
    {
        public float spawnRate;
        public float[] position = new float[2];
        public string enemyType;
    }

    [Serializable]
    public class ConfigurationObject
    {
        public List<SpawnerSaveObject> spawners = new List<SpawnerSaveObject>();
        public float spawnLength; 

    }
}
