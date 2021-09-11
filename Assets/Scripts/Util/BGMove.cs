using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    public float speed = 0.1f;
    SpriteRenderer BGspriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        BGspriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        BGspriteRenderer.material.mainTextureOffset += new Vector2(speed*Time.deltaTime,0);
    }
}
