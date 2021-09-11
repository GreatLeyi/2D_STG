using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Transform prefabBullet;
    [SerializeField] private Transform prefabExplosion;

    private float fireGap;
    [SerializeField]private float fireCountDown = 0;

    private PlayerModel playerModel;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerModel = GetComponent<PlayerModel>();

        // 初始化玩家信息界面
        PlayerInfoView.Instance.UpdateView(playerModel);
        playerModel.AddEventListener(UpdatePlayerInfoView);
    }

    void Update()
    {
        HandleInput();

        fireGap = 1 / playerModel.playerRof;
        fireCountDown -= Time.deltaTime;
        fireCountDown = fireCountDown < 0 ? 0 : fireCountDown;
    }

    void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Move(horizontalInput, verticalInput);

        if (Input.GetButton("Fire1"))
        {
            TryFire();
        }
    }
    void Move(float h, float v)
    {
        rb.velocity = new Vector2(h, v) * speed;
    }
    void TryFire()
    {
        if (fireCountDown == 0)
        {
            Transform bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
            fireCountDown = fireGap;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            GetDamaged(other.GetComponent<BulletController>().damage);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GetDamaged(1);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("Player meets item: " + other.gameObject.name);
            switch (other.gameObject.tag)
            {
                case "Item_BulletAmount":
                    playerModel.UpdatePlayerBulleAmount(true);
                    break;
                case "Item_Rof":
                    playerModel.UpdatePlayerRof(true);
                    break;
            }
            Destroy(other.gameObject);
        }
    }
    void GetDamaged(int damage)
    {
        Debug.Log("Player damaged!");
        Transform transExplosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        playerModel.DecreasePlayerHealth(damage);
        // 随机选择一项能力减少
        if(Random.Range(-1, 1) > 0)  
        {
            playerModel.UpdatePlayerRof(false);
        }
        else
        {
            playerModel.UpdatePlayerBulleAmount(false);
        }

        if (playerModel.playerHealth == 0)
        {
            Destroy(gameObject);
            GetComponent<PlayerModel>().SaveData();
            LevelManager.Instance.OnGameOver();
        }
    }

    void UpdatePlayerInfoView(PlayerModel playerModel)
    {
        PlayerInfoView.Instance.UpdateView(playerModel);
    }
}
