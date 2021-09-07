using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject panelGameOver;
    public static LevelManager Instance{get; private set;}

    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        panelGameOver.SetActive(false);
    }

    public void OnGameOver(){
        Debug.Log("Game Over");
        panelGameOver.SetActive(true);
    }

    public void RestartLevel(){
        Debug.Log("Restart Level");
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
