using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 15.0f;

    void Update()
    {
        transform.position += speed * Time.deltaTime* new Vector3(0,1,0);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
    }
}
