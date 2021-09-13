using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject panelGameOver;
    public GameObject playerInfoView;
    public GameObject panelGameWinning;
    public GameObject panelGameStart;
    public Text[] txtDeathTimes;  // 可能有多个承载死亡次数的Text

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void UpdateDeathTimes()
    {
        int deathTimes = PlayerPrefs.GetInt("PlayerDeathTimes", 0);
        for (int i = 0; i < txtDeathTimes.Length; i++)
        {
            txtDeathTimes[i].text = "x " + deathTimes.ToString();
        }
    }
}
