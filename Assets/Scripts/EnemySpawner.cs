using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform prefabEnemy;
    public float spawnTimeGap = 1.0f;

    void Start(){
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(true)
        {
            Transform transEnemy = Instantiate(prefabEnemy);
            transEnemy.position = new Vector3(Random.Range(-2.5f,2.5f),4.6f,0);
            yield return new WaitForSeconds(spawnTimeGap);
        }
    }
}
