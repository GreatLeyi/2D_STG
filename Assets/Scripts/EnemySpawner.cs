using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public Transform prefabEnemy;
    //public float spawnTimeGap = 1.0f;
    public InfoWave[] infoWaves;
    private bool canSpawn = true;

    private void Update() {
        if (canSpawn)
        {
            CheckSpawn();
        }
    }
    public void CheckSpawn(){
        for(int i = 0; i < infoWaves.Length; i++){
            if(LevelManager.Instance.levelTimer > infoWaves[i].spawnTime && !infoWaves[i].hasSpawned)
            {
                StartCoroutine(SpawnWave(i, infoWaves[i].spawnTimeGap));
                infoWaves[i].hasSpawned = true;
            }
        }
    }
    IEnumerator SpawnWave(int waveIndex, float timeGap)
    {
        foreach(Transform prefabEnemy in infoWaves[waveIndex].prefabEnemies){
            prefabEnemy.position = infoWaves[waveIndex].routes[0].GetChild(0).position;  // 防止闪现
            prefabEnemy.GetComponent<EnemyController>().routes = infoWaves[waveIndex].routes;
            Transform transEnemy = Instantiate(prefabEnemy);
            yield return new WaitForSeconds(timeGap);
        }
    }
    public void StartSpawn(){
        canSpawn = true;
    }
    public void StopSpawn(){
        canSpawn = false;
        StopAllCoroutines();
    }
}
