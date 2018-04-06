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
    float interpolationPeriod = 0.1f;
    DynamoDB.Dynode dynode;
    int deathCount;

    void Start()
    {
        logging = true;
        run_id = generateID();
        // Create a session-unique, persistent object for logging.
        // If it already exists (from a previous run), then refind it.
        if ((GameObject.Find("DynamoDB")))
        {
            dynode = GameObject.Find("DynamoDB").GetComponent<DynamoDB.Dynode>();
        }
        else
        {
            GameObject DynodeObject = new GameObject("DynamoDB");
            dynode = DynodeObject.AddComponent<DynamoDB.Dynode>();
        }

        // Set Dynode's parameters
        /*
        dynode.AWS_ACCESS_KEY_ID = "";
        dynode.AWS_SECRET_ACCESS_KEY = "";
        dynode.table_name = "";
        dynode.primary_key = "";
        */
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
            //Item["Log"]["S"] = log;
            //Item["Persn"]["S"] = MainMenu.username;
            Item["Event"]["S"] = "Pos";
            Item["X"]["S"] = positionx;
            Item["Y"]["S"] = positiony;
            Item["run_id"]["S"] = run_id;
            Item["Index"]["S"] = DataManager.index.ToString();
            //Item["Y coordinate"]["S"] = keyEvent;



            //Debug.Log("Key logged: " + keyEvent);

            dynode.Send(Item);
        }
    }

    public string generateID()
    {
        return Guid.NewGuid().ToString();
    }


    public void LogWin()
    {
        if (logging)
        {
            logging = false;
            DataManager.index++;
            var Item = new JSONObject();
            Item["Event"]["S"] = "Win";
            Item["run_id"]["S"] = run_id;
            Item["Index"]["S"] = DataManager.index.ToString();
            dynode.Send(Item);
            Debug.Log("Win logged");
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
            Item["Count"]["S"] = deathCount.ToString();
            Item["run_id"]["S"] = run_id;
            Item["Index"]["S"] = DataManager.index.ToString();
            dynode.Send(Item);
            Debug.Log("Death logged");
            run_id = generateID();
            logging = false;
        }
    }

    /* void TestLog2(string keyEvent2)
     {
         var Item = new JSONObject();
         Item["Log"]["S"] = log;
         Item["Person"]["S"] = MainMenu.username;
         Item["Log"]["S"] = log;

         dynode.Send(Item);
     }*/

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
        //Debug.Log("X= " + transform.position.x);
        //T = System.DateTime.Now;

        //Debug.Log("Y= " + transform.position.y);
        //Debug.Log("T= " + System.DateTime.Now);
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
                // Debug.Log(sx);
               // Debug.Log(sy);
                LogPosition(sx, sy);
                Scene scene = SceneManager.GetActiveScene();
                //Debug.Log("Active scene is '" + scene.name + "'.");
                nextTime += interval;
                
            }
            /*if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                TestLog("RightDown");
            }
            if (X = transform.position.x)
            {
                TestLog(X);
            }
             if (Input.GetKeyUp(KeyCode.RightArrow))
             {
                 TestLog("RightUp");
             }
             if (Input.GetKeyDown(KeyCode.LeftArrow))
             {
                 TestLog("LeftDown");
             }
             if (Input.GetKeyUp(KeyCode.LeftArrow))
             {
                 TestLog("LeftUp");
             }
             if (Input.GetKeyDown(KeyCode.UpArrow))
             {
                 TestLog("UpDown");
             }
             if (Input.GetKeyUp(KeyCode.UpArrow))
             {
                 TestLog("UpUp");
             }
             if (Input.GetKeyDown(KeyCode.S))
             {
                 TestLog("SDown");
             }
             if (Input.GetKeyUp(KeyCode.S))
             {
                 TestLog("SUp");
             }
         }
     }*/
            //}
        }
    }
}
