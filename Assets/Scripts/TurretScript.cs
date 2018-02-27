using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {

    public int curHealth;
    public int maxHealth;

    public float distance;
    public float wakeRange;
    public float shootInterval;
    public float bulletTimer;
    public float bulletSpeed = 70;
    public float x_comp;

    public bool awake = false;
    public bool lookingRight;

    public GameObject bullet;
    public Transform target;
    Animator anim;
    public Transform shootPointLeft, shootPointRight;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    
    void Start () {
        curHealth = maxHealth;
	}
	
	void Update () {
        RangeCheck();

        if (target.transform.position.x > transform.position.x)
            lookingRight = true;
        else
            lookingRight = false;

        anim.SetBool("Awake", awake);
        anim.SetBool("LookingRight", lookingRight);
	}

    void RangeCheck()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < wakeRange)
            awake = true;
        if (distance > wakeRange)
            awake = false;
    }

    public void Attack(bool attackingRight)
    {
        bulletTimer += Time.deltaTime;
        if (bulletTimer >= shootInterval)
        {
            Vector2 direction = new Vector2(target.transform.position.x - transform.position.x,0);
            direction.Normalize();
           
            if (!attackingRight)
            {
                GameObject bulletClone;
                bulletClone = Instantiate(bullet, shootPointLeft.transform.position, shootPointLeft.transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                bulletTimer = 0;
            }

            if (attackingRight)
            {
                GameObject bulletClone;
                bulletClone = Instantiate(bullet, shootPointRight.transform.position, shootPointRight.transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                bulletTimer = 0;
            }
        }
    }
}
