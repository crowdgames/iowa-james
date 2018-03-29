using System.Collections;
using System.Collections.Generic;
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
    Vector3 startPos;
    Fade fadePanel;
    public GameObject deathEffect;
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
        fadePanel = GameObject.FindObjectOfType<Fade>();
        startPos = player.transform.position;

        /*
        if(Randomizer.randomized)
        {
            Debug.Log(Randomizer.levels.Count);
            LoadNextLevel();
        }
        */
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
    }


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

    public void FadeOut()
    {
        Debug.Log("Inside fade out");
        player.rb.velocity = Vector3.zero;
        player.anim.SetFloat("Speed", 0f);
        player.anim.SetFloat("vSpeed", 0f);
        StartCoroutine(FadeCo(cg, cg.alpha, 1));
        
    }

    public IEnumerator FadeCo(CanvasGroup cg, float start, float end, float lerpTime = 3f)
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

            if (percentageCompleted >= 1)
                break;

            yield return new WaitForEndOfFrame();
        }
        /*
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        */
        //DestroyImmediate(deathEffect,true);
        Randomizer.LoadNextLevel();
        Debug.Log("Called randomizer.load");
    }

}
