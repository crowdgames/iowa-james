using UnityEngine;
using System.Collections;

public class SpikeAni : MonoBehaviour
{

    public float delta = 2.5f;  // Amount to move left and right from the start point
    public float speed = 2.0f;
    private Vector3 startPos;
    public bool end=false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 v = startPos;
        v.y += Mathf.PingPong(Time.time, speed);
        transform.position = v;
    }
}