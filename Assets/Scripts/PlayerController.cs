using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform prefabBullet;
    public Transform prefabExplosion;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Move(h, v);

        if(Input.GetButton("Fire1"))
        {
            FireNormal();
        }
    }

    void Move(float h, float v)
    {
        rb.velocity = new Vector2(h, v) * speed;
    }

    void FireNormal()
    {
        Transform bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")||other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Player died!");
            Transform transExplosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity); 
            Destroy(gameObject);

            LevelManager.Instance.OnGameOver();
        }
    }
    
}
