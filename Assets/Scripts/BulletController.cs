using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 15.0f;
    public Vector3 initDirection = new Vector3(0, 1, 0);
    public bool isSniper = false;  // 是自机狙
    public int damage = 1;

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
        transform.Translate(speed * Time.deltaTime * initDirection);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
