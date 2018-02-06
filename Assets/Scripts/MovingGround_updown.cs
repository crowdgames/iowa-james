using UnityEngine;
using System.Collections;

public class MovingGround_updown : MonoBehaviour
{

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
        v.y += delta * Mathf.Cos(Time.time * speed);
        transform.position = v;
    }
}