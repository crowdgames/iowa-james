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
    SkillManager sm;
    GameObject errorObj;
    GameObject tryObj;
    GameObject errTryObj;
    SkipLevel skip;
    GameObject skipObj;

    void Start()
    {

        Debug.Log("inside level manager start");
        player = GameObject.FindObjectOfType<PlayerController>();
        log = player.GetComponent<Logger>();
        fadePanel = GameObject.FindObjectOfType<Fade>();
        startPos = player.transform.position;
        coinTextObj = GameObject.FindGameObjectWithTag("CoinText");
        sm = GameObject.Find("SkillManager").GetComponent<SkillManager>();
        errorObj = GameObject.Find("Error");
        skipObj = GameObject.Find("Skip");
        tryObj = GameObject.Find("TryButton");
        errTryObj = GameObject.Find("ErrorTry");
        if(skipObj)
            skip = skipObj.GetComponent<SkipLevel>();
        if (errorObj)
            errorObj.SetActive(false);

        if(DataManager.mode == 3)
        {
            //No coins
            if(coinTextObj)
                coinTextObj.SetActive(false);
            //Debug.Log("No coins " + DataManager.mode);
            DisableCoins();
        }
        else
        {
         //   Debug.Log("Mode " + DataManager.mode);
            //Designer coins (==0) or Path coins (==1) or random coins (==2)
            if(coinTextObj)
                coinTextObj.SetActive(true);
            GenerateCoins(DataManager.mode);
        }
        
    }

    public void DisableCoins()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject c in coins)
        {
            if(c)
                c.SetActive(false);
        //    Debug.Log("making coin inactive");
        }
    }

    public void GenerateCoins(int mode)
    {
        //Debug.Log("Inside generate coins with mode " + mode);
        if (mode != 0 || mode != 3)
        {
            List<Vector2> pos = new List<Vector2>();
            string name = SceneManager.GetActiveScene().name;
            if (name == "Start")
                return;
            string path = "Coins/";
            path += mode == 1 ? "out_" + name + "_path" : "out_" + name + "_randall";
            //Debug.Log(path);
            TextAsset coinData = Resources.Load<TextAsset>(path);
            string[] lines = coinData.text.Split('\n');
            for (int i = 0; i < lines.Length - 1; i++)
            {
                string line = lines[i];
                //Debug.Log("Line: " + line);
                float x = float.Parse(line.Split(',')[0]);
                float y = float.Parse(line.Split(',')[1]);
                pos.Add(new Vector2(x, y));
                //Debug.Log("x: " + x + "y: " + y);
            }
            if (mode == 1)   // Path
            {
                DisableCoins();
                foreach (Vector2 p in pos)
                {
                    Instantiate(coin, p, Quaternion.identity);
                    //Debug.Log("instantiating path coin");
                }
            }
            else if (mode == 2)            //Randall
            {
                DisableCoins();

                for (int i = 0; i < DataManager.NCOINS; i++)
                {
                    int index = Random.Range(0, pos.Count);
                    Instantiate(coin, pos[index], Quaternion.identity);
                    //Debug.Log("instantiating random coin");
                    pos.RemoveAt(index);
                }
            }
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
        player.transform.parent = null;
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        player.transform.position = startPos;
        player.gameObject.SetActive(true);
        //Debug.Log("Active: " + player.gameObject.activeSelf);
        //Debug.Log("Set to true" + player.transform.position);
        log.logging = true;
    }

    public IEnumerator FadeOut()
    {
        Debug.Log("Inside fade out");
        player.rb.velocity = Vector3.zero;
        player.anim.SetFloat("Speed", 0f);
        player.anim.SetFloat("vSpeed", 0f);
        string level = SceneManager.GetActiveScene().name;
        float score = Mathf.Max(0f, 1f - ((0.1f * player.deathCount)));
        Debug.Log("SCORE: " + score);
        sm.score = score;
        sm.level = level;
        yield return sm.ReportAndRequest();
        Debug.Log("NEXT LEVEL: " + sm.server_data);
        string next_level = "";
        try
        {
            next_level = sm.server_data.Substring(sm.server_data.IndexOf("Level"), 8);
        }
        catch
        {
            if(sm.server_data != "ERROR")
                next_level = "Level_End";
        }
        if (sm.server_data != "ERROR")
        {
            yield return StartCoroutine(FadeCo(cg, cg.alpha, 1));
            SceneManager.LoadScene(next_level);
        }
        else
        {
            player.canMove = true;
            player.canMove = false;
        }
    }

    public IEnumerator FadeCo(CanvasGroup cg, float start, float end, float lerpTime = 2f)
    {
     //   Debug.Log("Inside fadeco");
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
        //Debug.Log("Exiting fadeco");
     //   yield return null;
    }

    void LoadNextLevel()
    {
        Debug.Log("Called LoadNextLevel");
        Randomizer.LoadNextLevel();
    }

    public IEnumerator ShowError()
    {
        errorObj.SetActive(true);
        errTryObj.SetActive(false);
        tryObj.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(3.0f);
        errTryObj.SetActive(true);
        tryObj.GetComponent<Button>().interactable = true;
    }

    public void Recontact()
    {
        HideError();
        Debug.Log("Recontact");
        Debug.Log("REQ: " + sm.server_request);
        Debug.Log("ERR: " + sm.server_error);
        if (sm.server_error == "StartGame")
            sm.RegisterAndGetFirstMatch();
        else if (sm.server_error == "ReportAndRequest")
            StartCoroutine(FadeOut()); //StartCoroutine(sm.ReportAndRequest());
        else if (sm.server_error == "SkipLevel")
            skip.Skip();
    }

    public void HideError()
    {
        if (errorObj)
            errorObj.SetActive(false);

    }
}
