using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class EnemyConfigurations : MonoBehaviour
{

    // -- check this with file names 
    public enum EnemyFileNames
    {
        VerticalShield
    }


    public WaveConfiguration GetAllowableTypesBasedOnWave(int waveNumber)
    {

        // -- spawnerTypes 
        // 0: crawling 
        // 1: jumping 
        // 2: shield 
        List<int> allowableRandomSpawners = new List<int>();
        allowableRandomSpawners.Add(0);

        // -- allowableTypes 
        // "random" 
        // EnemyFileNames.VerticalShield; 
        
        
        List<string> allowableTypes = new List<string>();
        allowableTypes.Add("random");



        // -- holds configurations for waves 
        if (waveNumber <= 1)
        {
            return new WaveConfiguration(
                allowableRandomSpawners: allowableRandomSpawners.ToArray(),
                totSpawners: 2,
                allowableTypes: allowableTypes.ToArray(),
                timeBetweenWaves: .5f,
                randomSubWaveLength: 4f,
                goal: 5);
        }
        if(waveNumber >= 2 && waveNumber <= 3)
        {
            // -- add jumping guy 
            allowableRandomSpawners.Add(1); 

            return new WaveConfiguration(
                allowableRandomSpawners: allowableRandomSpawners.ToArray(),
                totSpawners: 3,
                allowableTypes: allowableTypes.ToArray(),
                timeBetweenWaves: .5f,
                randomSubWaveLength: 4f,
                goal: 10);
        }
        if(waveNumber >= 4 && waveNumber <= 4)
        {
            // -- double time b/w waves so they can get used to shield guy 
            allowableRandomSpawners = new List<int> { 2 }; 

            return new WaveConfiguration(
                allowableRandomSpawners: allowableRandomSpawners.ToArray(),
                totSpawners: 2,
                allowableTypes: allowableTypes.ToArray(),
                timeBetweenWaves: 2f,
                randomSubWaveLength: 4f,
                goal: 4);
        }
        if (waveNumber >= 5 && waveNumber <= 7)
        {
            /* Notes */
            // add shield configuration here and hence, drop it from max allowable so it doesn't spawn randomly 
            // also tone down random spawners

            allowableRandomSpawners = new List<int> { 0, 1 };

            allowableTypes.Add(EnemyFileNames.VerticalShield.ToString()); 


            return new WaveConfiguration(
                allowableRandomSpawners: allowableRandomSpawners.ToArray(),
                totSpawners: 3,
                allowableTypes: allowableTypes.ToArray(),
                timeBetweenWaves: 4f,
                randomSubWaveLength: 4f,
                goal: 20);
        }
        else
        {
            throw new Exception("No more waves coded for");
        }
    }

    public struct WaveConfiguration
    {
        public int[] allowableRandomSpawners;         // -- max allowable type of spawner able to spawn randomly 
        public int totalSpawners;
        public string[] allowableWaveTypes;     // -- allowable pre-set configurations available to draw from 
        public float timeBetweenSubWaves;
        public float randomSubWaveLength;
        public int goal; 

        public WaveConfiguration(int[] allowableRandomSpawners, int totSpawners, string[] allowableTypes, float timeBetweenWaves, float randomSubWaveLength, int goal)
        {
            this.allowableRandomSpawners = allowableRandomSpawners;
            this.totalSpawners = totSpawners;
            this.allowableWaveTypes = allowableTypes;
            this.timeBetweenSubWaves = timeBetweenWaves;
            this.randomSubWaveLength = randomSubWaveLength;
            this.goal = goal; 
        }
    }
}
