using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //子弹基本属性
    public float speed = 3.0f;
    public float rotateRate = 0.0f;
    public bool isSniper = false;  // 是自机狙
    public int damage = 1;
    // 子弹初始方向
    public Vector3 initDirection = new Vector3(0, 1, 0);

    // public bool isPlayerBullet = false;  // 用于将画面外的玩家子弹清除，防止提前杀怪

    private void Start() {
        if(isSniper){
            if(LevelManager.Instance.player != null)
            {
                initDirection = (LevelManager.Instance.player.transform.position - transform.position).normalized;
            }
        }
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotateRate * Time.deltaTime));
        transform.Translate(speed * Time.deltaTime * initDirection);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DeathSideBoundary") || 
            other.gameObject.layer == LayerMask.NameToLayer("BulletBoundary"))
        {
            // Debug.Log("Bullet exits DeathSideBoundary");
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
