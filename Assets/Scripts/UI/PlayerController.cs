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
    private SpriteRenderer spriteRenderer;

    // 道具
    private float increaseAngleOfBullet = 7;
    // 受伤后一秒内无敌，概率失去power
    private float invincibleSecAfterDamaged = 1;  
    private bool isInvincible = false;
    private float posibilityToLosePower = 0.5f;

    //音效
    private AudioSource audioSource;
    public AudioClip audioFire;
    public AudioClip audioGetItem;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerModel = GetComponent<PlayerModel>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 初始化玩家信息界面
        UIManager.Instance.playerInfoView.GetComponent<PlayerInfoView>().UpdateView(playerModel);
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
            // 播放攻击音效
            audioSource.PlayOneShot(audioFire);
            // 以正前方为中心，向两边散射
            for (int i = 0; i < playerModel.playerBulletAmount; i++)
            {
                // just for fun
                float rotation =  (2 * (i & 0x1) - 1) * increaseAngleOfBullet * (int)((i + 1) / 2);

                Transform bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
                bullet.Rotate(new Vector3(0, 0, rotation));
            }
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
            // Debug.Log("Player meets item: " + other.gameObject.name);
            //播放音效
            audioSource.PlayOneShot(audioGetItem);

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
        if (!isInvincible)
        {
            // 受伤无敌时间
            StartCoroutine(SetInvincibleForSenconds(invincibleSecAfterDamaged));

            StartCoroutine(FlashColor(spriteRenderer, Color.red));

            // Debug.Log("Player damaged!");
            Transform transExplosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
            playerModel.DecreasePlayerHealth(damage);
            // 受伤有可能掉Power
            if(((float)Random.Range(0, 100))/100 < posibilityToLosePower)
            {
                // 随机选择一项能力减少（攻速因为出的很少，所以降低概率也低）
                if (Random.Range(-99, 100) > 70)
                {
                    playerModel.UpdatePlayerRof(false);
                }
                else
                {
                    playerModel.UpdatePlayerBulleAmount(false);
                }
            }
            

            // 玩家死亡
            if (playerModel.playerHealth == 0)
            {
                Destroy(gameObject);
                GetComponent<PlayerModel>().SaveData();
                LevelManager.Instance.OnGameOver();
            }
        }
    }

    IEnumerator SetInvincibleForSenconds(float sec)
    {
        isInvincible = true;
        yield return new WaitForSeconds(sec);
        isInvincible = false;
    }

    void UpdatePlayerInfoView(PlayerModel playerModel)
    {
        UIManager.Instance.playerInfoView.GetComponent<PlayerInfoView>().UpdateView(playerModel);
    }

    IEnumerator FlashColor(SpriteRenderer renderer, Color targetColor)
    {
        Color originColor = renderer.color;
        renderer.color = targetColor;
        yield return new WaitForSeconds(0.2f);
        renderer.color = originColor;
    }
}
