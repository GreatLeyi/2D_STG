using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform prefabBullet;
    public Transform prefabExplosion;

    public GameObject boundary_lt;
    public GameObject boundary_rb;
    private float xMin, xMax,yMin,yMax;
    // Start is called before the first frame update
    void Start()
    {
        xMin = boundary_lt.transform.position.x;
        xMax = boundary_rb.transform.position.x;
        yMin = boundary_rb.transform.position.y;
        yMax = boundary_lt.transform.position.y;
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
        transform.position += new Vector3(h, v, 0) * speed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), 0);
    }

    void FireNormal()
    {
        Transform bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")||other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // TODO: Player Die
            Debug.Log("Player died!");
            Transform transExplosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity); 
            Destroy(gameObject);

            LevelManager.Instance.OnGameOver();
        }
    }
    
}
