using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

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
    public bool canMove;
    public bool canDie;
    bool jumpPressed;
    string[] killers;
    public int deathCount;

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

    HCGManager hcgm;

    //UI text aspects
    /*
    public GameObject gameOverUI;
    public Text relevantItems;
    public Text irrelevantItems;
    public Sprite openChest;
    public GameObject[] sceneHCHItems;
    public GameObject itemMismatchUI;
    public Image irrelevantImage;
    public GameObject gameCompleteUI;

    //Inventory script
    InventoryManager inventoryManager;
    int inventoryCount = 0;
    public GameObject chest;
    public int inventoryLimit;
    public GameObject collectable = null;
    string collectableName;
    public CanvasGroup cg;

    ItemsGenerator itemgenerator;
    */
    public bool touched = false;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        logger = GetComponent<Logger>();
        
        killers = new string[] { "Killer", "RisingSpikes", "Spikes", "Bullet", "Ninja", "Lava", "Pit" };

        curHealth = 1;

        startPos = transform.position;

        lm = GameObject.FindObjectOfType<LevelManager>();
        hcgm = GameObject.FindObjectOfType<HCGManager>();
        
        canMove = true;
        canDie = true;
        jumpPressed = false;

        deathCount = 0;

        coins = 0;
        coinTextObj = GameObject.FindGameObjectWithTag("CoinText");
        if (DataManager.mode != 3)
        {
            if (coinTextObj)
            {
                coinText = coinTextObj.GetComponent<Text>();
                coinText.text = "Coins: " + coins + "/" + DataManager.NCOINS; //+ "\tLevel: " + (SceneManager.GetActiveScene().buildIndex + 1) + "/" + (SceneManager.sceneCountInBuildSettings - 1); //+ " ID: " + logger.dynode.player_id;
            }
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
        if (DataManager.mode == 4)
        {
            //Save HCG Item
            if (col.CompareTag("HCGItem"))//.gameObject.tag == "HCGItem" && gameObject.tag =="Player")
            {
                string item = col.gameObject.name;
                Sprite sprite = col.gameObject.GetComponent<SpriteRenderer>().sprite;
                Destroy(col.gameObject);
                hcgm.CollectItem(item,sprite);
            }
        }

        //if (DataManager.mode != 0)
        {
            if (col.CompareTag("Coin"))
            {
                Destroy(col.gameObject);
                coins++;
                //Debug.Log("ID: " + logger.dynode.player_id);
                if (coinText)
                    coinText.text = "Coins: " + coins + "/" + DataManager.NCOINS; // + "\tMode: " + DataManager.mode;// + "\tLevel: " + (SceneManager.GetActiveScene().buildIndex + 1) + "/" + (SceneManager.sceneCountInBuildSettings - 1); //+ " ID: " + logger.dynode.player_id;
                if (coins == DataManager.NCOINS)
                {
                    coinText.color = Color.green;
                }
                logger.LogCoins(coins);
            }
        }    
        
        if(col.CompareTag("Chest"))
        {
            if (hcgm.relevant_count == hcgm.items.Length / 2)
            {
                //Debug.Log("Coins: " + coins);
                logger.LogWin(coins);
                logger.LogMatch("win");
                canMove = false;
                canDie = false;
                StartCoroutine(lm.FadeOut());
            }
            else
            {
                StartCoroutine(hcgm.ShowRelevant());
            }
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

    void Footstep()
    {

    }
    
}
