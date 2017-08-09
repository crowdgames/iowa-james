using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    //private Vector3 offset;

    // Camera slide up and down stuff
    public Vector3 destination;
    public float speed = 2.0f;
    public float transitionPoint = 4.0f;
    

    void Start()
    {
        //offset = transform.position - player.transform.position;
    }
    

    void LateUpdate()
    {
        if (player.transform.position.x > 0 && player.transform.position.x <= 148.8)
        {
            Vector3 temp = transform.position;
            temp.x = player.transform.position.x;
            transform.position = temp;
        }
        //transform.position = offset + player.transform.position;
    }

    private void Update()
    {
        destination.x = transform.position.x;
        destination.z = transform.position.z;
        if (player.transform.position.y > transitionPoint)
            destination.y = 5.7f;
        else
            destination.y = 0;
        transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
    }
}