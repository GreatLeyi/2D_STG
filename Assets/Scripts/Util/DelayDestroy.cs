using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public float delaySecond = 1.0f;
    void Start()
    {
        Invoke("Remove", delaySecond);
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
