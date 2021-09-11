using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoView : MonoBehaviour
{
    public static PlayerInfoView Instance { get; private set; }
    private Color initColor;

    [SerializeField] private Text txtPlayerHealth;
    [SerializeField] private Text txtPlayerRof;
    [SerializeField] private Text txtPlayerBulletAmount;

    //private int new_txtPlayerHealth;
    //private float new_txtPlayerRof;
    //private int new_txtPlayerBulletAmount;

    private void Awake()
    {
        Instance = this;
        initColor = txtPlayerHealth.color;
    }
    // update view
    public void UpdateView(PlayerModel playerModel)
    {
        // 可根据涨跌而变色的数字。应该有更好的实现方式
        txtPlayerHealth.text = playerModel.playerHealth.ToString();
        if (playerModel.healthUp)
        {
            StartCoroutine(FlashColor(txtPlayerHealth, Color.green));
        }
        else
        {
            StartCoroutine(FlashColor(txtPlayerHealth, Color.red));
        }

        txtPlayerRof.text = playerModel.playerRof.ToString();
        if (playerModel.rofUp)
        {
            StartCoroutine(FlashColor(txtPlayerRof, Color.green));
        }
        else
        {
            StartCoroutine(FlashColor(txtPlayerRof, Color.red));
        }

        txtPlayerBulletAmount.text = playerModel.playerBulletAmount.ToString();
        if (playerModel.bulletUp)
        {
            StartCoroutine(FlashColor(txtPlayerBulletAmount, Color.green));
        }
        else
        {
            StartCoroutine(FlashColor(txtPlayerBulletAmount, Color.red));
        }
       
    }

    IEnumerator FlashColor(Text text, Color color)
    {
        text.color = color;
        yield return new WaitForSeconds(0.5f);
        text.color = initColor;
    }
}
