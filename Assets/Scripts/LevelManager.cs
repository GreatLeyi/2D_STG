using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance{get; private set;}
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject playerInfoView;

    public float levelTimer { get; private set; }
    private float levelStartTime;
    private float levelEndTime;

    [HideInInspector]
    public GameObject player;

    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        panelGameOver.SetActive(false);
        playerInfoView.SetActive(true);
        StartLevel();
    }

    private void Update() {
        levelTimer = Time.time - levelStartTime;
    }

    public void StartLevel(){
        player = Instantiate(prefabPlayer, spawnPoint.position, Quaternion.identity);
        levelStartTime = Time.time;
        enemySpawner.StartSpawn();
    }
    public void OnGameOver(){
        Debug.Log("Game Over");
        levelEndTime = Time.time;
        enemySpawner.StopSpawn();
        panelGameOver.SetActive(true);
    }

    public void RestartLevel(bool clearPrefs){
        Debug.Log("Restart Level With cleanning");
        if (clearPrefs)
        {
            PlayerPrefs.DeleteAll();
        }
        SceneManager.LoadScene(0);
    }

    public void QuitGame(){
        Debug.Log("Quit game");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
