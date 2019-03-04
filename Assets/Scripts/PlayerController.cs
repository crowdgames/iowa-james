using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public Sprite openChest;
    public GameObject[] sceneHCHItems;
    //Inventory script
    InventoryManager inventoryManager;
    int inventoryCount = 0;
    public GameObject tChest;
    public int inventoryLimit;
    public GameObject collectable = null;
    string collectableName;
    // Analytics
    Vector3 previousPos;

    public int curHealth;
    //public int maxHealth = 5;

    // General physics variables
    public static bool end = false;
    public float maxSpeed = 6.9f;
    public float jumpForce = 1000.0f;
    public bool facingRight = true;
    bool sliding;
    Rigidbody2D rb;
    Animator anim;
    public BoxCollider2D slidingCollider;
    BoxCollider2D myCol;

    // Variables for checking for ground
    bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.4f;
    public LayerMask whatIsGround;

    public gameMaster gm;
    AudioSource audioFootstep;
    AudioSource audioCoin;
    AudioSource audioJump;

    // Knockback variables
    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;
    public float delay = 3;


    private int level;
    private Text levelText;
    private GameObject levelImage;
    public GameObject EndUI;

    ItemsGenerator itemsgenerator;

    // Use this for initialization
    void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
        itemsgenerator = GameObject.FindGameObjectWithTag("ItemsMaster").GetComponent<ItemsGenerator>();


        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myCol = GetComponent<BoxCollider2D>();
        curHealth = 3;


        inventoryManager.DisplayHeart(GamePersistentManager.Instance.currentLives);
        FillInventoryDuringStart();
        // gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<gameMaster>();
        //sceneHCHItems = GameObject.FindGameObjectsWithTag("HCGItem");

        //EnableAllHGCItems();
    }

    void FixedUpdate()
    {
        // Check if grounded
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
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

            {
                sliding = false;
                slideAccell = 1.0f;
                slidingCollider.enabled = false;
                myCol.enabled = true;
            }
            anim.SetBool("Sliding", sliding);

            // MOTION
            rb.velocity = new Vector2(move * maxSpeed * slideAccell, rb.velocity.y);

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


    void Update()
    {



        if (GamePersistentManager.Instance.inventoryCount == inventoryLimit)
        {
            tChest.GetComponent<SpriteRenderer>().sprite = openChest;
            // tChest.SetActive(true);

        }



        // Check if dead
        if (curHealth <= 0)
        {
            // Die();
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
            if (audioJump != null)
                audioJump.Play();
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

        if (col.gameObject.tag == "HCGItem")
        {
            string objectName = col.gameObject.name;
            objectName = objectName.Replace("(Clone)", "");


            if (itemsgenerator.itemsForCurrentlocation.Contains(objectName))
            {
                inventoryManager.AddItem(col.gameObject);
                GamePersistentManager.Instance.inventoryItems.Add(objectName);
                GamePersistentManager.Instance.inventoryCount += 1;
              //  inventoryCount += 1;
            }

            else
            {
                //Die and restart
                //Restart level
                // Reduce one heart
                Destroy(col.gameObject);
                GamePersistentManager.Instance.currentLives -= 1;
                //Debug.Log(GamePersistentManager.Instance.currentLives);
                inventoryManager.DisplayHeart(GamePersistentManager.Instance.currentLives);
                Die();
            }

        }


        if (col.CompareTag("Hazard"))
        {
            Destroy(col.gameObject);
            GamePersistentManager.Instance.currentLives -= 1;
            //Debug.Log(GamePersistentManager.Instance.currentLives);
            inventoryManager.DisplayHeart(GamePersistentManager.Instance.currentLives);
            Die();
        }
        if (col.CompareTag("Coin"))
        {
            Destroy(col.gameObject);
            audioCoin.Play();
            gm.points += 1;
        }

        if (col.CompareTag("Tchest"))
        {
            GamePersistentManager.Instance.inventoryCount = GetSceneIndex(SceneManager.GetActiveScene().buildIndex+1);
            GamePersistentManager.Instance.currentLives = 3;
            GamePersistentManager.Instance.inventoryItems.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }



        if (col.CompareTag("Enemy"))
        {

            //  Die();

        }
    }


    void Die()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Damage(int dmg)
    {
        curHealth -= dmg;
        // anim.Play("FlashRed");
    }

    void Footstep()
    {
        if (audioFootstep != null)
            audioFootstep.Play();
    }

    void FillInventoryDuringStart()
    {
        //Clear the inventory
        inventoryManager.ClearInventory();

        //Debug.Log(GamePersistentManager.Instance.inventoryItems.Count);
        //Grab the items from persistent manager

        if (GamePersistentManager.Instance.inventoryItems.Count > 0)
        {
            //GameObject myObject;
            for (int i = 0; i < GamePersistentManager.Instance.inventoryItems.Count; i++)
            {
                for (int j = 0; j < GamePersistentManager.Instance.itemsList.Count; j++)
                {
                    if (GamePersistentManager.Instance.inventoryItems[i] == GamePersistentManager.Instance.itemsList[j].gameObject.name)
                    {
                        inventoryManager.AddItem(GamePersistentManager.Instance.itemsList[j].gameObject);
                    }
                }
            }
        }
    }

    int GetSceneIndex(int value)
    {
        switch (value)
        {
            case 1:
                return 5;
            case 2:
                return 7;
            case 3:
                return 10;
            case 4:
                return 12;
             case 5:
                return 14;
            default:
                return 0;
        }
    }


}
