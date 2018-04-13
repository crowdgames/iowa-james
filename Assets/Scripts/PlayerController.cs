using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    // Analytics
    Vector3 previousPos;
    Vector3 startPos;

    LevelManager lm;
    Logger logger;

    public int curHealth=1;
    //public int maxHealth = 5;

    // General physics variables
    public float maxSpeed = 6.9f;
    public float jumpForce = 1000.0f;
    bool facingRight = true;
    bool sliding;
    public Rigidbody2D rb;
    public BoxCollider2D slidingCollider;
    BoxCollider2D myCol;
    bool canMove;
    bool canDie;
    bool jumpPressed;
    string[] killers;
    int deathCount;

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
    public Animator anim;

    GameObject coinTextObj;
    Text coinText;
    int coins;
    int win = 0;

    float move;

    DynamoDB.Dynode dynode;

    // Use this for initialization
    void Start () {
        /*AudioSource[] aSources = GetComponents<AudioSource>();
        audioFootstep = aSources[0];
        audioCoin = aSources[1];
        audioJump = aSources[2];*/
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //myCol = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        logger = GetComponent<Logger>();
        

        killers = new string[] { "Killer", "RisingSpikes", "Spikes", "Bullet", "Ninja", "Lava", "Pit" };
        //levelImage.SetActive(false);

        curHealth = 1;
        //gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<gameMaster>();

        startPos = transform.position;

        lm = GameObject.FindObjectOfType<LevelManager>();

        //Debug.Log("Start: " + startPos);

        canMove = true;
        canDie = true;
        jumpPressed = false;
        //Debug.Log("Can move in start: " + canMove);

        deathCount = 0;

        coins = 0;
        coinTextObj = GameObject.FindGameObjectWithTag("CoinText");
        if (DataManager.mode != 0)
        {
            coinText = coinTextObj.GetComponent<Text>();
            coinText.text = "Coins: " + coins + "/" + DataManager.NCOINS; //+ "\tLevel: " + (SceneManager.GetActiveScene().buildIndex + 1) + "/" + (SceneManager.sceneCountInBuildSettings - 1); //+ " ID: " + logger.dynode.player_id;
        }
    }

    void FixedUpdate()
    {
        // Check if grounded
        //grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        grounded = Physics2D.IsTouchingLayers(groundCheck.GetComponent<Collider2D>(), whatIsGround);
        anim.SetBool("Ground", grounded);

        if (canMove)
        {
            anim.SetFloat("vSpeed", rb.velocity.y);

            // Horizontal motion
            move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", Mathf.Abs(move));
        }
        
        // Knockback and motion stuff
        if (knockbackCount <= 0) // You can't move while getting knocked back.
        {
            /*
            // Sliding stuff
            float slideAccell;
            /*if (Input.GetKey(KeyCode.S) && grounded && Mathf.Abs(rb.velocity.x) > 3.0f)
            {
                sliding = true;
                slideAccell = 2.0f;
                slidingCollider.enabled = true;
                myCol.enabled = false;
            }
            //else
            {
                sliding = false;
                slideAccell = 1.0f;
                slidingCollider.enabled = false;
                myCol.enabled = true;
            }
            anim.SetBool("Sliding", sliding);
           */
            // MOTION
            if(canMove)
                rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);
            
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
        if (move > 0 && !facingRight && canMove)
            Flip();
        else if (move < 0 && facingRight && canMove)
            Flip();

        // Jumping
        if (grounded && jumpPressed && canMove)
        {
            //Debug.Log("Jumping called");
            anim.SetBool("Ground", false);
            rb.AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
            grounded = false;
            //audioJump.Play();
            jumpPressed = false;
        }
    }


    void Update () {
        // Check if dead
        if (curHealth <= 0 && canDie)
        {
            lm.Die();
        }

        // Check for max health
        /**if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }*/
        
        if(Input.GetKeyDown(KeyCode.UpArrow) && !jumpPressed && grounded)
        {
            jumpPressed = true;
        }

        DataManager.play_time += Time.deltaTime;
        //coinText.text = "Coins: " + coins + "/" + DataManager.NCOINS + "Time: " + DataManager.play_time + " Level: " + (SceneManager.GetActiveScene().buildIndex + 1) + "/" + (SceneManager.sceneCountInBuildSettings - 1); //+ " ID: " + logger.dynode.player_id;
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
        
        if (DataManager.mode != 0)
        {
            if (col.CompareTag("Coin"))
            {
                   Debug.Log("Coin trigger called");
                    //audioCoin.Play();
                    Destroy(col.gameObject);
                    //DataManager.points++;
                    coins++;
                Debug.Log("ID: " + logger.dynode.player_id);
                if (coinText)
                    //coinText.text = "Coins: " + DataManager.points;
                    coinText.text = "Coins: " + coins + "/" + DataManager.NCOINS;// + "\tLevel: " + (SceneManager.GetActiveScene().buildIndex + 1) + "/" + (SceneManager.sceneCountInBuildSettings - 1); //+ " ID: " + logger.dynode.player_id;
                if (coins == DataManager.NCOINS)
                {
                    coinText.color = Color.green;
                }
                logger.LogCoins(coins);
            }
        }    
        
        if(col.CompareTag("Chest"))
        {
            Debug.Log("Coins: " + coins);
            logger.LogWin(coins);
            canMove = false;
            canDie = false;
            StartCoroutine(lm.FadeOut());

        }

        //if ((col.CompareTag("Enemy") || col.CompareTag("Killer")) && canDie)
        if ((System.Array.IndexOf(killers,col.tag) > -1) && canDie)
        {
            
                jumpPressed = false;
                Debug.Log("Killed by: " + col.tag);
                deathCount++;
                float pos_x = transform.position.x;
                float pos_y = transform.position.y;
                Debug.Log("Tag: " + col.tag);
                lm.Die();
                logger.LogDeath(col.tag, deathCount, pos_x, pos_y);
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
