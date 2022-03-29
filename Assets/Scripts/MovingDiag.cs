using UnityEngine;

public class MovingDiag : MonoBehaviour {

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Enter");
        if (collision.gameObject.tag == "Player")
        {
            //   moving = true;
            collision.gameObject.transform.SetParent(transform);
            Debug.Log("Enter");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
            // moving = false;
            Debug.Log("Exit");
        }
    }
}
