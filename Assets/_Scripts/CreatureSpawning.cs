using System.Collections;
using System.Xml.Schema;
using UnityEngine;

public class CreatureSpawning : MonoBehaviour
{
    [SerializeField]
    GameObject[] CreaturePrefabs;
    public float totalDuration, perWaveTime; 
    public void DoWaves(int creaturesPerWave, float duration, float perWaveDuration)
    {
        StartCoroutine(DoWave(duration, perWaveDuration, creaturesPerWave));
    }

    public void DoSpawn(int creaturesPerWave)
    {
        DoWaves(creaturesPerWave, totalDuration, perWaveTime);
           
    }

    IEnumerator DoWave(float duration, float thisWaveTime, int creatureSpawnCount)
    {
        float spawnRadius = 15f;
        for(int creatureIndex = 0; creatureIndex < creatureSpawnCount; creatureIndex++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = new Vector3(offset.x, 5f + transform.position.y, offset.y);
            GameObject creatureToSpawn = CreaturePrefabs[Random.Range(0, CreaturePrefabs.Length)];
            Instantiate(creatureToSpawn, spawnPos + transform.position, Quaternion.identity);

        }

        Debug.Log("Spawning!");

        float elapsedTime = 0f;
        while(elapsedTime < thisWaveTime)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log("Elapsed time: " + elapsedTime);
            Debug.Log("elapsed (" + elapsedTime + ") < waveTime(" + thisWaveTime +") = " + (elapsedTime < thisWaveTime));
            yield return null;
        }

        Debug.Log("Done waiting!");

        duration -= Time.deltaTime;
        if(duration > thisWaveTime)
        {
            Debug.Log("New coroutine!");
            StartCoroutine(DoWave(duration, thisWaveTime, creatureSpawnCount));
        }
    }
}
