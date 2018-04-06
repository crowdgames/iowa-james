using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    /*
    private static Random random = new Random();
    public static bool randomized = true;
    private static bool created = false;
    public static List<int> levelOrder;
    */

    public CanvasGroup cg;
    PlayerController player;
    Logger log;
    Vector3 startPos;
    Fade fadePanel;
    public GameObject deathEffect;
    public GameObject coin;
    GameObject coinTextObj;
    Text coinText;
    /*
    void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        fadePanel = GameObject.FindObjectOfType<Fade>();
        cg = fadePanel.GetComponent<CanvasGroup>();
        startPos = player.transform.position;
        /*
        if (!created)
        {
            Debug.Log("Created level manager");
            DontDestroyOnLoad(this.gameObject);
            created = true;
            levelOrder = new List<int>();
            for (int i=0; i < SceneManager.sceneCountInBuildSettings-1; i++)
            {
                levelOrder.Add(i);
            }
            Debug.Log(levelOrder.Count);
            if (randomized)
            {
                if(levelOrder.Count != SceneManager.sceneCountInBuildSettings - 1)
                        LoadNextLevel();
                else
                        levelOrder.RemoveAt(SceneManager.GetActiveScene().buildIndex);
            }
            //else
             //   SceneManager.LoadScene(0);
            
        }
        
    }
    */
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        log = player.GetComponent<Logger>();
        fadePanel = GameObject.FindObjectOfType<Fade>();
        startPos = player.transform.position;
        
        coinTextObj = GameObject.FindGameObjectWithTag("CoinText");
        if (coinTextObj)
        {
            coinText = coinTextObj.GetComponent<Text>();
        }

        if(DataManager.mode == 0)
        {
            //No coins
            coinText.gameObject.SetActive(false);
            Debug.Log("No coins " + DataManager.mode);
        }
        else
        {
            Debug.Log("Mode " + DataManager.mode);
            //Path coins (==1) or random coins (==2)
            coinText.gameObject.SetActive(true);
            GenerateCoins(DataManager.mode);
        }
        
        /*
        if(Randomizer.randomized)
        {
            Debug.Log(Randomizer.levels.Count);
            LoadNextLevel();
        }
        */
    }

    public void GenerateCoins(int mode)
    {
        string path = "Assets/Coins/";
        string name = SceneManager.GetActiveScene().name;
        path += mode == 1 ? "out_" + name + "_path.txt" : "out_" + name + "_randall.txt";
        Debug.Log(path);
        StreamReader sr = new StreamReader(path);
        while(!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            float x = float.Parse(line.Split(',')[0]);
            float y = float.Parse(line.Split(',')[1]);
            Debug.Log("x: " + x + "y: " + y);
            Instantiate(coin, new Vector2(x,y), Quaternion.identity);
        }
    }
    
    public void Die()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Inside die...");
        StartCoroutine("Respawn");
        Debug.Log("After respawn");
    }

    IEnumerator Respawn()
    {
        Debug.Log("Inside respawn...");
        Debug.Log(player.transform.position);
        Instantiate(deathEffect, player.transform.position, player.transform.rotation);
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        player.transform.position = startPos;
        player.gameObject.SetActive(true);
        Debug.Log("Active: " + player.gameObject.activeSelf);
        Debug.Log("Set to true" + player.transform.position);
        log.logging = true;
    }

    /*
    public void LoadNextLevel()
    {
        if (Randomizer.randomized)
        {
            if (Randomizer.levels.Count > 0)
            {
                int index = Random.Range(0, Randomizer.levels.Count);
                int level = Randomizer.levels[index];
                Debug.Log(index + " " + level);
                Randomizer.levels.Remove(level);
                Debug.Log(Randomizer.levels.Count);
                Debug.Log("Loading level " + level);
                SceneManager.LoadScene(level);
            }
            else
                SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    */

    public IEnumerator FadeOut()
    {
        Debug.Log("Inside fade out");
        player.rb.velocity = Vector3.zero;
        player.anim.SetFloat("Speed", 0f);
        player.anim.SetFloat("vSpeed", 0f);
        yield return StartCoroutine(FadeCo(cg, cg.alpha, 1));
        //Invoke("LoadNextLevel",3f);
        Randomizer.LoadNextLevel();
        Debug.Log("Exiting fade out");
    }

    public IEnumerator FadeCo(CanvasGroup cg, float start, float end, float lerpTime = 2f)
    {
        Debug.Log("Inside fadeco");
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageCompleted = timeSinceStarted / lerpTime;

        
        while (true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageCompleted = timeSinceStarted / lerpTime;

            float curVal = Mathf.Lerp(start, end, percentageCompleted);

            cg.alpha = curVal;


            //yield return new WaitForEndOfFrame();
            yield return null;

            if (percentageCompleted >= 1)
                break;

        }
        /*
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        */
        //DestroyImmediate(deathEffect,true);
        //Randomizer.LoadNextLevel();
        Debug.Log("Exiting fadeco");
     //   yield return null;
    }

    void LoadNextLevel()
    {
        Debug.Log("Called LoadNextLevel");
        Randomizer.LoadNextLevel();
    }

}
