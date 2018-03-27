using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private static Random random = new Random();
    public static bool randomized = true;

    public CanvasGroup cg;
    PlayerController player;
    Vector3 startPos;
    Fade fadePanel;
    private static bool created = false;
    public static List<int> levelOrder;

    void Awake()
    {
        /*
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        */
        player = GameObject.FindObjectOfType<PlayerController>();
        fadePanel = GameObject.FindObjectOfType<Fade>();
        startPos = player.transform.position;
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
                LoadNextLevel();
            else
                SceneManager.LoadScene(0);
        }
        
    }
    
	// Use this for initialization
	void Start () {
        /*
        player = GameObject.FindObjectOfType<PlayerController>();
        fadePanel = GameObject.FindObjectOfType<Fade>();
        startPos = player.transform.position;
        */
	}
	
	// Update is called once per frame
	void Update () {
		
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
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        player.transform.position = startPos;
        player.gameObject.SetActive(true);
        Debug.Log("Active: " + player.gameObject.activeSelf);
        Debug.Log("Set to true" + player.transform.position);
    }


    public void LoadNextLevel()
    {
        if (randomized)
        {
            if (levelOrder.Count > 0)
            {
                int index = Random.Range(0, levelOrder.Count);
                int level = levelOrder[index];
                Debug.Log(index + " " + level);
                levelOrder.Remove(level);
                Debug.Log(levelOrder.Count);
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
        StartCoroutine(FadeCo(cg, cg.alpha, 1));
        
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

            if (percentageCompleted >= 1)
                break;

            yield return new WaitForEndOfFrame();
        }
        /*
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        */
        LoadNextLevel();
    }

}
