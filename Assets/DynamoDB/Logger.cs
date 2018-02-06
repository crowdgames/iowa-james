using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class Logger : MonoBehaviour {

    public string awsAccessKeyID = "";
    public string awsSecretAccessKey = "";
    public string tableName;
    public string primaryKey;
    public string log;
    public bool logging;

    DynamoDB.Dynode dynode;

    void Start () {
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
        dynode.AWS_ACCESS_KEY_ID = awsAccessKeyID;
        dynode.AWS_SECRET_ACCESS_KEY = awsSecretAccessKey;
        dynode.table_name = tableName;
        dynode.primary_key = primaryKey;

        //InvokeRepeating("TestLog", 2.0f, 0.5f);
    }


    // A logging function. This would be called every second,
    // OR every time the user puts in an input.
    void TestLog(string keyEvent)
    {
        // Put in ONLY item data into the Item object.
        // Do NOT put in a primary key, as Dynode will handle that for you.
        // Remember to put in the data TYPE. This is VERY IMPORTANT!

        var Item = new JSONObject();
        //Item["Log"]["S"] = log;
        Item["Person"]["S"] = MainMenu.username;
        Item["Log"]["S"] = log;
        Item["Action"]["S"] = keyEvent;
       //Debug.Log("Key logged: " + keyEvent);

        dynode.Send(Item);
    }

    // Call this function whenever user pauses the game!!!
    // Function will fire KeyUp events during pause menu, so that keys don't
    // stuck during pause time.
    public void LogPause()
    {
        TestLog("RightUp");
        TestLog("LeftUp");
        TestLog("UpUp");
        TestLog("SUp");
    }

	// Update is called once per frame
	void Update () {
        if (logging)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                TestLog("RightDown");
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
    }
}
