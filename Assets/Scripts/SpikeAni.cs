using UnityEngine;
using System.Collections;

public class SpikeAni : MonoBehaviour
{
    private Vector3 MovingDirection = Vector3.up;
    public float Uplimit = 3.0F;
    public float Downlimit = -3.0F;
    public float MovementSpeed = 2.0F;

    void Update()
    {
        gameObject.transform.Translate(MovingDirection * Time.deltaTime * MovementSpeed);

        if (gameObject.transform.position.y > Uplimit)
        {
            MovingDirection = Vector3.down;
        }
        else if (gameObject.transform.position.y < Downlimit)
        {
            MovingDirection = Vector3.up;
        }
    }
}