using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Collision");
        if (!col.isTrigger)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<PlayerController>().Damage(1);
            }

            Destroy(gameObject);
        }
    }
}
