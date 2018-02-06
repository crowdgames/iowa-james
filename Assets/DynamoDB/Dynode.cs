using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.SceneManagement;

namespace DynamoDB
{

    // Session-unique, persistent middle class that handles and transports 
    // data between DDBHTTP requests and the logger.
    //
    public class Dynode : MonoBehaviour
    {

        DDBHTTP http;
        public string table_name;
        public string primary_key;
        public string AWS_ACCESS_KEY_ID;
        public string AWS_SECRET_ACCESS_KEY;

        private string session_id;
        private string run_id;
        private int run_count;
        private int action_count;
        private DateTime startTime;
        

        void Start()
        {
            // Achieve Session Persistence
            DontDestroyOnLoad(gameObject);

            // For detecting scene changes
            SceneManager.activeSceneChanged += onSceneChanged;
            // Call function on game start
            onSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());

            session_id = generateID();
            startTime = DateTime.UtcNow;

            http = gameObject.AddComponent<DDBHTTP>();
            http.action = "DynamoDB_20120810.PutItem";
            http.AWS_ACCESS_KEY_ID = AWS_ACCESS_KEY_ID;
            http.AWS_SECRET_ACCESS_KEY = AWS_SECRET_ACCESS_KEY;

            //Debug.Log("Dynode instance created...");
        }

        // Send the DDBHTTP request
        public void Send(JSONObject Item)
        {
            action_count++;
            Debug.Log(action_count);
            // This is the only time we ever create a time stamp,
            // for consistency purposes.
            DateTime stamp = DateTime.UtcNow;

            var obj = new JSONObject();
            obj["TableName"] = table_name;
            obj["Item"] = Item;
            obj["Item"]["run_count"]["S"] = run_count.ToString();
            obj["Item"]["action_count"]["S"] = action_count.ToString();
            obj["Item"]["session_id"]["S"] = session_id;
            obj["Item"]["run_id"]["S"] = run_id;
            obj["Item"][primary_key]["S"] = generateID();
            obj["Item"]["Stamp"]["S"] = (stamp - startTime).TotalSeconds.ToString();

            http.BuildWWWRequest(obj.ToString(), stamp);
            StartCoroutine(http.WaitForRequest(http.www, callback => {
                if(callback != null)
                {
                    //Debug.Log(callback);
                }
            }));
        }

        // Random ID generator
        public string generateID()
        {
            return Guid.NewGuid().ToString();
        }

        // Every time a level is loaded.
        // Make sure you don't log during non-playing levels (such as menus).
        
        private void onSceneChanged(Scene one, Scene two)
        {
            run_id = generateID();
            run_count++;

            action_count = 0;
        }
    }
}