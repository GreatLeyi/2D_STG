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
    [SerializeField] private Camera mainCamera;

    // 记录死亡次数
    private int deathTimes = 0;

    //背景音乐
    [SerializeField] private MusicPlayer musicPlayer;

    public float levelTimer { get; private set; }
    private float levelStartTime;
    private float levelEndTime;

    [HideInInspector]
    public GameObject player;

    private void Awake() {
        Instance = this;
    }
    private void Start()
    {
        UIManager.Instance.UpdateDeathTimes();
        UIManager.Instance.panelGameStart.SetActive(true);

        UIManager.Instance.panelGameOver.SetActive(false);
        UIManager.Instance.playerInfoView.SetActive(false);
        UIManager.Instance.panelGameWinning.SetActive(false);
    }

    private void Update() {
        levelTimer = Time.time - levelStartTime;
    }

    public void StartLevel(){
        // 初始化UI
        UIManager.Instance.playerInfoView.SetActive(true);

        UIManager.Instance.panelGameOver.SetActive(false);
        UIManager.Instance.panelGameWinning.SetActive(false);
        UIManager.Instance.panelGameStart.SetActive(false);

        // 播放音乐
        musicPlayer.StopPlayingMusic();  // 结束fade的速率很快，远大于开始
        musicPlayer.StartPlayingMusic();
        musicPlayer.SetSpatialBlend(0.0f);

        // 玩家初始化
        player = Instantiate(prefabPlayer, spawnPoint.position, Quaternion.identity);

        // 开始计时
        levelStartTime = Time.time;

        // 开始生成敌人
        enemySpawner.StartSpawn();
    }
    public void OnGameOver(){
        Debug.Log("Game Over");
        levelEndTime = Time.time;
        enemySpawner.StopSpawn();

        deathTimes = PlayerPrefs.GetInt("PlayerDeathTimes", 0);
        deathTimes++;
        PlayerPrefs.SetInt("PlayerDeathTimes", deathTimes);

        UIManager.Instance.UpdateDeathTimes();
        UIManager.Instance.panelGameOver.SetActive(true);

        musicPlayer.SetSpatialBlend(0.8f);
    }

    // 由UI按钮引用
    public void RestartLevel(bool clearPrefs){
        Debug.Log("Restart Level");
        UIManager.Instance.playerInfoView.SetActive(true);
        
        if (clearPrefs)
        {
            deathTimes = 0;
            PlayerPrefs.SetInt("PlayerDeathTimes", deathTimes);

            PlayerPrefs.DeleteAll();
        }
        // SceneManager.LoadScene(0);
        // 因为初期用layer区分物体种类，这里不能用findwithtag，很难受
        GameObject[] gos = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in gos)
        {
            if( go.layer == LayerMask.NameToLayer("PlayerBullet") ||
                go.layer == LayerMask.NameToLayer("Player") ||
                go.layer == LayerMask.NameToLayer("EnemyBullet") ||
                go.layer == LayerMask.NameToLayer("Enemy") ||
                go.layer == LayerMask.NameToLayer("Item")
                )
            {
                Destroy(go);
            }
        }

        StartLevel();
    }

    // 由UI按钮引用
    public void QuitGame(){
        Debug.Log("Quit game");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    // 本来用于检测子弹是否飞出Main Camera，但考虑到性能还是换用碰撞体解决
    public bool IsInMainCamera(Renderer renderer)
    {
        return mainCamera.IsObjectVisible(renderer);
    }
    public void OnGameWinning()
    {
        // TODO: 展示成功界面
        Debug.Log("Winning!");
        UIManager.Instance.panelGameWinning.SetActive(true);
    }
}
