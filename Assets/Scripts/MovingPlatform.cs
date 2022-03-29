using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]
    public Vector3 velocity;

    private bool moving = true;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Enter");
        if (collision.gameObject.tag == "Player")
        {
            moving = true;
            collision.gameObject.transform.SetParent(transform);
            Debug.Log("Enter");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
            moving = false;
            Debug.Log("Exit");
        }
    }

    private void FixedUpdate()
    {
        if(moving)
        {
            transform.position += (velocity * Time.deltaTime);
        }
    }
}
