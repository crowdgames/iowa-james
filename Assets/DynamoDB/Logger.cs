using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using UnityEngine.SceneManagement;

public class Logger : MonoBehaviour
{
    string awsAccessKeyID = Credentials.awsAccessKeyID;
    string awsSecretAccessKey = Credentials.awsSecretAccessKey;
    string tableName = Credentials.tableName;
    string primaryKey = Credentials.primaryKey;
    
    public static float X;
    public static float Y;
   // public string log;
    public bool logging;
    int interval = 1;
    string run_id;
    float nextTime = 0;
    private float time = 0.0f;
    float interpolationPeriod = 1.0f;
    public DynamoDB.Dynode dynode;
    int deathCount;

    void Awake()
    {
        logging = true;
        run_id = generateID();
        // Create a session-unique, persistent object for logging.
        // If it already exists (from a previous run), then refind it.
        GameObject ddb = GameObject.Find("DynamoDB");
        if(ddb)
        {
            dynode = ddb.GetComponent<DynamoDB.Dynode>();
        }
        else
        {
            GameObject DynodeObject = new GameObject("DynamoDB");
            dynode = DynodeObject.AddComponent<DynamoDB.Dynode>();
        }

        dynode.AWS_ACCESS_KEY_ID = awsAccessKeyID;
        dynode.AWS_SECRET_ACCESS_KEY = awsSecretAccessKey;
        dynode.table_name = tableName;
        dynode.primary_key = primaryKey;

        deathCount = 0;

        //InvokeRepeating("TestLog", 2.0f, 0.5f);
    }


    // A logging function. This would be called every second,
    // OR every time the user puts in an input.
    void LogPosition(string positionx, string positiony)
    {
        // Put in ONLY item data into the Item object.
        // Do NOT put in a primary key, as Dynode will handle that for you.
        // Remember to put in the data TYPE. This is VERY IMPORTANT!
        if (logging)
        {
            DataManager.index++;
            var Item = new JSONObject();
            var obj = new JSONObject();

            Item["Event"]["S"] = "Pos";
            Item["X"]["S"] = positionx;
            Item["Y"]["S"] = positiony;
            Item["run_id"]["S"] = run_id;
            Item["Index"]["S"] = DataManager.index.ToString();
            Item["play_time"]["S"] = DataManager.play_time.ToString();
            dynode.Send(Item);
        }
    }

    public string generateID()
    {
        return Guid.NewGuid().ToString();
    }


    public void LogWin(int coins)
    {
        if (logging)
        {
            logging = false;
            DataManager.index++;
            var Item = new JSONObject();
            Item["Event"]["S"] = "Win";
            Item["run_id"]["S"] = run_id;
            Item["Index"]["S"] = DataManager.index.ToString();
            Item["coins"]["S"] = coins.ToString();
            Item["play_time"]["S"] = DataManager.play_time.ToString();
            dynode.Send(Item);
            Debug.Log("Win logged: " + coins);
        }
    }

    public void LogCoins(int coins)
    {
        if (logging)
        {
            DataManager.index++;
            var Item = new JSONObject();
            Item["Event"]["S"] = "Coin";
            Item["Index"]["S"] = DataManager.index.ToString();
            Item["coins"]["S"] = coins.ToString();
            Item["play_time"]["S"] = DataManager.play_time.ToString();
            dynode.Send(Item);
            //Debug.Log("Coin logged: " + coins );
        }
    }

    public void LogDeath(string tag, int count, float x, float y)
    {
        if (logging)
        {
            DataManager.index++;
            deathCount++;
            var Item = new JSONObject();
            Item["Event"]["S"] = "Death";
            Item["X"]["S"] = x.ToString();
            Item["Y"]["S"] = y.ToString();
            Item["Killer"]["S"] = tag;
            Item["Count"]["S"] = count.ToString();
            Item["run_id"]["S"] = run_id;
            Item["Index"]["S"] = DataManager.index.ToString();
            Item["play_time"]["S"] = DataManager.play_time.ToString();
            dynode.Send(Item);
            Debug.Log("Death logged");
            logging = false;
            run_id = generateID();
        }
    }

    public void LogMatch(string result)
    {
        if(logging)
        {
            DataManager.index++;
            var Item = new JSONObject();
            Item["Event"]["S"] = "Match";
            Item["Index"]["S"] = DataManager.index.ToString();
            Item["Result"]["S"] = result;
            Item["play_time"]["S"] = DataManager.play_time.ToString();
            dynode.Send(Item);
        }
    }
    

    // Call this function whenever user pauses the game!!!
    // Function will fire KeyUp events during pause menu, so that keys don't
    // stuck during pause time.
    public void LogPause()
    {
        //TestLog("RightUp", "x");
    }

    // Update is called once per frame
    void Update()
    {

        X = transform.position.x;

        Y = transform.position.y;
        if (logging)

        {
            // if (Time.time >= nextTime)
            // {
            time += Time.deltaTime;

            if (time >= interpolationPeriod)
            {
                time = 0.0f;
                //do something here every interval seconds
                string sx = "" + X;

                string sy = "" + Y;
                string position = sx + ", " + sy;
                LogPosition(sx, sy);
                Scene scene = SceneManager.GetActiveScene();
                nextTime += interval;
                
            }
           
        }
    }
}
