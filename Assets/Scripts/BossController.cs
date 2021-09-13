using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BossController : MonoBehaviour
{
    public float angleOffset = 180;
    public Transform prefabExplosion;

    private SpriteRenderer spriteRenderer;

    // 基础属性
    public float speed = 0.01f;
    public int hp = 200;

    // 只会进场，不用别的移动。初始化位置由EnemySpwaner决定
    public GameObject targetPosition;
    [HideInInspector] public bool canMove = false;
    [SerializeField] private bool isOnTarget = false;

    // 发射子弹
    [SerializeField] private bool isVisible = false;  // 入场时才开始发射
    [SerializeField] private float periodPerRound = 5;  // 一轮攻击的持续时间
    [SerializeField] private bool inRound = false;  // 一轮攻击中要保持攻击方式
    [SerializeField] private bool fireAllowed = false;  // 用于锁开火协程
    [SerializeField] private float fireGap = 1.0f;
    [SerializeField] private float bulletSpeed = 1.0f;
    [SerializeField] private int bulletAmount = 6;
    [SerializeField] private int bulletCounter = 0;  // 一次射一颗，射到第几颗
    [SerializeField] private Transform prefabBullet;  // boss只射一种子弹，再多也只是堆砌工作量
    [SerializeField] Transform[] fireSources;  //炮口
    // 当前波次子弹的属性
    private bool canRotate = false;
    private bool isSniper = false;
    private bool onceAtOneTime = false;

    private Vector3 bossBulletDir = new Vector3(0, -1, 0);

    // 闪烁特效的锁
    private bool isFlashing = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        // Move
        if (!isOnTarget && canMove)
        {
            StartCoroutine(MoveToTarget(targetPosition.transform.position));
        }
        // Fire
        if (fireAllowed && isVisible && isOnTarget)
        {
            ExecuteRandomSkill();
        }
    }

    // 移动至目标点
    private IEnumerator MoveToTarget(Vector3 target)
    {
        float maxDistance = speed * Time.deltaTime;
        while((transform.position - target).magnitude > 0.001)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, maxDistance);
            yield return new WaitForEndOfFrame();
        }
        isOnTarget = true;
        StartFire();
    }

    // 实际上是以不同方式发射子弹
    private void ExecuteRandomSkill()
    {
        if (!inRound)
        {
            SetInRound();
            canRotate = Random.Range(0, 2) > 0;
            isSniper = Random.Range(0, 2) > 0;
            onceAtOneTime = Random.Range(0, 2) > 0;
            Invoke("SetOutOfRound", periodPerRound);
        }
        SimpleRaffle();
    }

    private void SimpleRaffle()
    {
        if (fireAllowed)
        {
            StopFire();

            for (int i = 0; i < fireSources.Length; i++)
            {
                FireFromSource(fireSources[i]);
            }

            Invoke("StartFire", fireGap);
        }
    }

    private void FireFromSource( Transform fireSource)
    {
        if (onceAtOneTime)
        {
            if(bulletCounter < bulletAmount)
            {
                ShootOneBulletFrom(fireSource, bulletCounter);
                bulletCounter++;
            }
            else
            {
                bulletCounter = 0;
            }          
        }
        else
        {
            for (int i = 0; i < bulletAmount; i++)
            {
                ShootOneBulletFrom(fireSource, i);
            }
        }
    }

    private void ShootOneBulletFrom(Transform fireSource, int bulletIndex)
    {
        Transform transBullet = Instantiate(prefabBullet, fireSource.position, Quaternion.identity);
        BulletController bullet = transBullet.GetComponent<BulletController>();
        bullet.speed = bulletSpeed;
        bullet.isSniper = isSniper;

        if (!isSniper)
        {
            bullet.initDirection = bossBulletDir;
            transBullet.Rotate(new Vector3(0, 0, 360 * (bulletIndex / (float)bulletAmount)));
        }
        if (canRotate)
        {
            bullet.rotateRate = 10.0f;
        }
    }

    private void StartFire()
    {
        fireAllowed = true;
    }
    private void StopFire()
    {
        fireAllowed = false;
    }
    private void SetInRound()
    {
        inRound = true;
    }
    private void SetOutOfRound()
    {
        inRound = false;
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }
    private void OnBecameInvisible()
    {
        isVisible = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            hp -= other.GetComponent<BulletController>().damage;
            
            StartCoroutine(FlashColor(spriteRenderer, Color.red));
                
            Destroy(other.gameObject);

            if (hp <= 0)
            {
                Transform transExplosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
                LevelManager.Instance.OnGameWinning();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator FlashColor(SpriteRenderer renderer, Color targetColor)
    {
        if (!isFlashing)
        {
            isFlashing = true;

            Color originColor = renderer.color;
            renderer.color = targetColor;
            yield return new WaitForSeconds(0.2f);
            renderer.color = originColor;

            isFlashing = false;
        }
    }
}
