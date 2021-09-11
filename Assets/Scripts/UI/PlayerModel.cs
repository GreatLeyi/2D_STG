using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerModel : MonoBehaviour
{
    // data
    public int playerHealth { get; private set;}
    public bool healthUp { get; private set; }  // 用于UI变色
    public float playerRof { get; private set; }  // rate of fire
    public bool rofUp { get; private set; }
    public int playerBulletAmount { get; private set; }
    public bool bulletUp { get; private set; }


    public UnityAction<PlayerModel> updateEvent { get; private set; }

    [SerializeField]
    private int defaultPlayerHealth = 3;
    //[SerializeField]
    //private int minPlayerHealth = 0;
    //[SerializeField]
    //private int maxPlayerHealth = 10;

    [SerializeField]
    private float minPlayerRof = 1.0f;
    [SerializeField]
    private float maxPlayerRof = 10.0f;
    [SerializeField]
    private float inceasementRof = 0.5f;

    [SerializeField]
    private int minPlayerBulletAmount = 1;
    [SerializeField]
    private int maxPlayerBulletAmount = 10;
    [SerializeField]
    private int inceasementBulletAmount = 1;

    // data operation
    // init
    public void Awake()
    {
        // 此场景中使用PlayerPrefs，但对于更复杂的场景可能需要用别的持久化存储方案？
        playerHealth =  defaultPlayerHealth;
        playerRof = PlayerPrefs.GetFloat("PlayerRof", minPlayerRof);
        playerBulletAmount = PlayerPrefs.GetInt("PlayerBulletAmount", minPlayerBulletAmount);

        healthUp = true;
        rofUp = true;
        bulletUp = true;
    }

    // update: health / rof(rate of fire) / bulletType，每次一级，非增即减
    public void DecreasePlayerHealth(int damage)
    {
        playerHealth -= damage;
        healthUp = false;
        AnounceUpdateInfo();
    }
    public void UpdatePlayerRof(bool isIncreased)
    {
        playerRof += isIncreased ? inceasementRof : -inceasementRof;
        playerRof = Mathf.Clamp(playerRof, minPlayerRof, maxPlayerRof);
        rofUp = isIncreased;
        AnounceUpdateInfo();
    }
    public void UpdatePlayerBulleAmount(bool isIncreased)
    {
        playerBulletAmount += isIncreased ? inceasementBulletAmount : -inceasementBulletAmount;
        playerBulletAmount = Mathf.Clamp(playerBulletAmount, minPlayerBulletAmount, maxPlayerBulletAmount);
        bulletUp = isIncreased;
        AnounceUpdateInfo();
    }
    // save
    public void SaveData()
    {
        // PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetFloat("PlayerRof", playerRof);
        PlayerPrefs.SetInt("PlayerBulletAmount", playerBulletAmount);
    }

    // inform others of update
    public void AddEventListener(UnityAction<PlayerModel> function)
    {
        updateEvent += function;
    }
    public void RemoveEventListener(UnityAction<PlayerModel> function)
    {
        updateEvent -= function;
    }
    private void AnounceUpdateInfo()
    {
        if( updateEvent!= null)
        {
            updateEvent(this);
        }
    }
}
