using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InfoWave
{
    public float spawnTime = 0.0f;
    public float spawnTimeGap = 1.0f;
    public Transform[] prefabEnemies;
    public Transform[] routes;
    [HideInInspector] public bool hasSpawned = false;
}
