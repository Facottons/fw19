using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    [System.Serializable]
    public class Wave
    {
        public EnemyCore[] enemies;
        public int count;
        public float timeBetweenSpaws;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves;
    public enum spawnDirectionEnum { Right, Left , Random }
    public spawnDirectionEnum spawndirection;
    private Wave currentWave;
    private int currentWaveIndex;
    private Transform player;
    private bool finishedSpawning;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(StartNextWave(currentWaveIndex));
        spawndirection = spawnDirectionEnum.Right;
	}
	
	// Update is called once per frame
	void Update () {
		if(finishedSpawning  && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            finishedSpawning = false;

            if(currentWaveIndex + 1 < waves.Length)
            {
                currentWaveIndex++;
                StartCoroutine(StartNextWave(currentWaveIndex));
            }
        }
	}

    IEnumerator StartNextWave(int index)
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(index));
    }

    IEnumerator SpawnWave(int index)
    {
        currentWave = waves[index];

        for(int i=0;i<currentWave.count;i++)
        {
            if(player == null)
            {
                yield break;
            }

            EnemyCore randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpot = null;
            if(spawndirection.Equals(spawnDirectionEnum.Right))
            {
                randomSpot = spawnPoints[1];
            }
            else if(spawndirection.Equals(spawnDirectionEnum.Left))
            {
                randomSpot = spawnPoints[0];
            }
            else
            {
                randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
            }
            Instantiate(randomEnemy,randomSpot.position,randomSpot.rotation);
           
            if (i == currentWave.count - 1)
            {
                finishedSpawning = true;
            }
            else
            {
                finishedSpawning = false;
            }

            yield return new WaitForSeconds(currentWave.timeBetweenSpaws);

        }
    }
}
