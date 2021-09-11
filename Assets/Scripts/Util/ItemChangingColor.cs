using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChangingColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.green, Mathf.PingPong(Time.time, 0.5f));
    }
}
