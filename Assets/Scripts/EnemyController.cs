using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform prefabExplosion;
    
    void Update()
    {
        transform.position += speed * Time.deltaTime* new Vector3(0,-1,0);
    }

     void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            Transform transExplosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity);  
            Destroy(gameObject);
        }
    }
}
