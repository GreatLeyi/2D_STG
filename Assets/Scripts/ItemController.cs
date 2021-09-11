using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    private float moveSpeed = 2.0f;
    private Vector3 initDirection;

    private void Start()
    {
        // initDirection = new Vector3(Random.Range(-1, 1), Random.Range(0, -1));
        initDirection = new Vector3(0, -1);
    }
    void Update()
    {
        Move();
    }
    void Move()
    {
        transform.Translate(moveSpeed * Time.deltaTime * initDirection);
    }

}
