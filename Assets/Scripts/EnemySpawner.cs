using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public Transform prefabEnemy;
    //public float spawnTimeGap = 1.0f;
    public InfoWave[] infoWaves;
    private bool canSpawn = false;
    private bool spawnFinished = false;
    private bool hasBossGenerated = false;

    // Boss
    public Transform prefabBoss;
    public GameObject bossStartPosition;
    public GameObject bossEndPosition;

    private void Update() {
        if (canSpawn)
        {
            CheckSpawn();
        }
    }
    // 每一帧检查波次时间
    public void CheckSpawn(){
        spawnFinished = true;
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
        // 重置波次生成状态
        hasBossGenerated = false;
        for (int i = 0; i < infoWaves.Length; i++)
        {
            infoWaves[i].hasSpawned = false;
        }
        canSpawn = true;
    }
    public void StopSpawn(){
        canSpawn = false;
        StopAllCoroutines();
    }
    public void OnSpawnFinished()
    {
        if (!hasBossGenerated)
        {
            Debug.Log("Spawn Finished, BOSS !");
            // 生成Boss
            Transform transBoss = Instantiate(prefabBoss, bossStartPosition.transform.position, Quaternion.identity);
            transBoss.GetComponent<BossController>().targetPosition = bossEndPosition;
            transBoss.GetComponent<BossController>().canMove = true;
            transBoss.rotation = Quaternion.Euler(0, 0, transBoss.GetComponent<BossController>().angleOffset);
            hasBossGenerated = true;
        }
    }
}
