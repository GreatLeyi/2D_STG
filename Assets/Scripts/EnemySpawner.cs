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
    // 每一帧检查波次时间
    public void CheckSpawn(){
        bool spawnFinished = true;
        // 轮询每个波次，可优化
        for(int i = 0; i < infoWaves.Length; i++){
            spawnFinished &= infoWaves[i].hasSpawned;
            if (LevelManager.Instance.levelTimer > infoWaves[i].spawnTime && !infoWaves[i].hasSpawned)
            {
                StartCoroutine(SpawnWave(i, infoWaves[i].spawnTimeGap));
                infoWaves[i].hasSpawned = true;
            }
        }
        if (spawnFinished)
        {
            OnSpawnFinished();
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

        // 重置波次生成状态
        for (int i = 0; i < infoWaves.Length; i++)
        {
            infoWaves[i].hasSpawned = false;
        }
    }
    public void StopSpawn(){
        canSpawn = false;
        StopAllCoroutines();
    }
    public void OnSpawnFinished()
    {
        Debug.Log("Spawn Finished !");
        // TODO: 出Boss
    }
}
