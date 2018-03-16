using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround_Diagonal : MonoBehaviour {

    public float delta = 2.5f;  // Amount to move left and right from the start point
    public float speed = 2.0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 v = startPos;
        v.x += delta * Mathf.Sin(Time.time * speed);
        v.y += delta * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
}
