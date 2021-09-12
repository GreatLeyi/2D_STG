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

    //��������
    [SerializeField] private MusicPlayer musicPlayer;

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
        StartLevel();
    }

    private void Update() {
        levelTimer = Time.time - levelStartTime;
    }

    public void StartLevel(){
        // ��ʼ��UI
        UIManager.Instance.panelGameOver.SetActive(false);
        UIManager.Instance.playerInfoView.SetActive(true);

        // ��������
        musicPlayer.StopPlayingMusic();  // ����fade�����ʺܿ죬Զ���ڿ�ʼ
        musicPlayer.StartPlayingMusic();
        musicPlayer.SetSpatialBlend(0.0f);

        // ��ҳ�ʼ��
        player = Instantiate(prefabPlayer, spawnPoint.position, Quaternion.identity);

        // ��ʼ��ʱ
        levelStartTime = Time.time;

        // ��ʼ���ɵ���
        enemySpawner.StartSpawn();
    }
    public void OnGameOver(){
        Debug.Log("Game Over");
        levelEndTime = Time.time;
        enemySpawner.StopSpawn();
        UIManager.Instance.panelGameOver.SetActive(true);

        musicPlayer.SetSpatialBlend(0.8f);
    }

    public void RestartLevel(bool clearPrefs){
        Debug.Log("Restart Level");
        if (clearPrefs)
        {
            PlayerPrefs.DeleteAll();
        }
        // SceneManager.LoadScene(0);
        // ��Ϊ������layer�����������࣬���ﲻ����findwithtag��������
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

    public void QuitGame(){
        Debug.Log("Quit game");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    // �������ڼ���ӵ��Ƿ�ɳ�Main Camera�������ǵ����ܻ��ǻ�����ײ����
    public bool IsInMainCamera(Renderer renderer)
    {
        return mainCamera.IsObjectVisible(renderer);
    }
}
