using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    // Analytics
    Vector3 previousPos;
    Vector3 startPos;

    LevelManager lm;

    public int curHealth=1;
    //public int maxHealth = 5;

    // General physics variables
    public float maxSpeed = 6.9f;
    public float jumpForce = 1000.0f;
    bool facingRight = true;
    bool sliding;
    Rigidbody2D rb;
    public BoxCollider2D slidingCollider;
    BoxCollider2D myCol;

    // Variables for checking for ground
    bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.4f;
    public LayerMask whatIsGround;

    //public gameMaster gm;
    AudioSource audioFootstep;
    AudioSource audioCoin;
    AudioSource audioJump;

    // Knockback variables
    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;
    public float delay = 3;


    private int level = 1;
    private Text levelText;
    private GameObject levelImage;
    private SpriteRenderer sr;
    private Animator anim;
    
    // Use this for initialization
    void Start () {
        /*AudioSource[] aSources = GetComponents<AudioSource>();
        audioFootstep = aSources[0];
        audioCoin = aSources[1];
        audioJump = aSources[2];*/
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myCol = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        //levelImage.SetActive(false);

        curHealth = 1;
        //gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<gameMaster>();

        startPos = transform.position;

        lm = GameObject.FindObjectOfType<LevelManager>();

        Debug.Log("Start: " + startPos);
    }

    void FixedUpdate()
    {
        // Check if grounded
        //grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        grounded = Physics2D.IsTouchingLayers(myCol, whatIsGround);
        anim.SetBool("Ground", grounded);

        anim.SetFloat("vSpeed", rb.velocity.y);

        // Horizontal motion
        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));
        
        // Knockback and motion stuff
        if (knockbackCount <= 0) // You can't move while getting knocked back.
        {
            // Sliding stuff
            float slideAccell;
            /*if (Input.GetKey(KeyCode.S) && grounded && Mathf.Abs(rb.velocity.x) > 3.0f)
            {
                sliding = true;
                slideAccell = 2.0f;
                slidingCollider.enabled = true;
                myCol.enabled = false;
            }*/
            //else
            {
                sliding = false;
                slideAccell = 1.0f;
                slidingCollider.enabled = false;
                myCol.enabled = true;
            }
            anim.SetBool("Sliding", sliding);

            // MOTION
            rb.velocity = new Vector2(move * maxSpeed * slideAccell, rb.velocity.y);
            /*
             *  For logging stuff 
             * if (transform.position != previousPos)
            {
                Debug.Log(transform.position + ": at time: " + Time.time);
                previousPos = transform.position;
            }*/
        }
        else
        {
            if (knockFromRight)
                rb.velocity = new Vector2(-knockback, knockback);
            else
                rb.velocity = new Vector2(knockback, knockback);
            knockbackCount -= Time.deltaTime;
        }

        // Animation flips
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }


    void Update () {
        // Check if dead
        if (curHealth <= 0)
        {
            lm.Die();
        }

        // Check for max health
        /**if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }*/

        // Jumping
        if (grounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetBool("Ground", false);
            rb.AddForce(new Vector2(0, jumpForce));
            //audioJump.Play();
        }
    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Coin"))
        {
            Destroy(col.gameObject);
            audioCoin.Play();
            //gm.points += 1;
        }    
        
        if(col.CompareTag("Chest"))
        {
            //int numScenes = SceneManager.sceneCountInBuildSettings;
            lm.FadeOut();
            

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //gameObject.GetComponent<Collider2D>().enabled = false;

            //StartCoroutine(Wait(3.0F));
        
        }

        if (col.CompareTag("Enemy") || col.CompareTag("Killer"))
        {
            Debug.Log("Tag: " + col.tag);
            lm.Die();
            Debug.Log("Grounded: " + grounded);
        }
    }

   

    public void Damage(int dmg)
    {
        curHealth -= dmg;
       // anim.Play("FlashRed");
    }

    
    void Footstep()
    {
        //audioFootstep.Play();
    }
    
    /*IEnumerator Wait(float waitTime)
    {
        float fadeTime = GameObject.Find("GameMaster").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }*/


}
