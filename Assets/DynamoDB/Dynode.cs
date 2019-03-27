using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

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
        public static float X;
        public string player_id;
        private int run_id;
        private int run_count = 0;
        private int action_count;
        private DateTime startTime;
        public Scene level;
        SkillManager sm;
        
        void Awake()
        {
            
            // Achieve Session Persistence
            DontDestroyOnLoad(gameObject);
            //Debug.Log("Inside dynode awake");
            // For detecting scene changes
            SceneManager.activeSceneChanged += onSceneChanged;
            // Call function on game start
            onSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());

            GameObject smo = GameObject.Find("SkillManager");
            if(smo)
                sm = smo.GetComponent<SkillManager>();
            string test = "http://viridian.ccs.neu.edu/instructions.html?workerId=MT-8d151263359973cd187ffce12db71fe2&hitId=ensemble";
            #if UNITY_EDITOR
                    player_id = "MT-" + generateID();
                    Debug.Log("Editor PID: " + player_id);
            #else
                    string url_string = Application.absoluteURL;
                    Debug.Log("URL: " + url_string);
                    string worker_param = "";
                    if(url_string.Length > 0 && url_string.Contains("?") && url_string.Contains("&"))
                        worker_param = url_string.Split('?')[1].Split('&')[0];
                    if(worker_param.Contains("="))
                        player_id = worker_param.Substring(worker_param.IndexOf("=") + 1);
                    Debug.Log("Web PID: " + player_id);
            #endif

            DataManager.player_id = player_id;
            //DataManager.mode = player_id.ToCharArray()[player_id.Length - 1] % 4;
            DataManager.mode = 1;
            //Debug.Log(player_id.ToCharArray()[player_id.Length - 1]);
            //Debug.Log("COIN MODE: " + DataManager.mode.ToString());
            
            run_id = 1;
            //Debug.Log("Player id: " + player_id);
            startTime = DateTime.UtcNow;
            //Debug.Log("Start: " + startTime);
            
            http = gameObject.AddComponent<DDBHTTP>();
            http.action = "DynamoDB_20120810.PutItem";
            
            http.AWS_ACCESS_KEY_ID = Credentials.awsAccessKeyID;
            http.AWS_SECRET_ACCESS_KEY = Credentials.awsSecretAccessKey;
            table_name = Credentials.tableName;
            primary_key = Credentials.primaryKey;
            //Debug.Log("Dynode instance created...");
        }

        // Send the DDBHTTP request
        public void Send(JSONObject Item)
        {
            action_count++;
            //Debug.Log (action_count);
            // This is the only time we ever create a time stamp,
            // for consistency purposes.
            DateTime stamp = DateTime.UtcNow;
            Scene scene = SceneManager.GetActiveScene();
            //Debug.Log("Active scene is '" + scene.name + "'.");

            var obj = new JSONObject();
            obj["TableName"] = table_name;
            obj["Item"] = Item;
           // obj["Item"]["level_count"]["S"] = run_count.ToString();
            obj["Item"]["level"]["S"] = scene.name.ToString();
            //obj["Item"]["action_count"]["S"] = action_count.ToString();
            //obj["Item"]["run_id"]["S"] = level_count.ToString();
            
            obj["Item"]["player_id"]["S"] = player_id;
            obj["Item"]["mode"]["S"] = DataManager.mode.ToString();
            // obj["Item"]["level"]["S"] = scene;
            obj["Item"][primary_key]["S"] = generateID();
            //obj["Item"]["Stamp"]["S"] = (stamp - startTime).TotalSeconds.ToString();
            obj["Item"]["Timestamp"]["S"] = DateTime.UtcNow.ToString();
            //Debug.Log("OBJ: " + obj.ToString());
            // Scene scene = SceneManager.GetActiveScene();
            // Debug.Log("Active scene is '" + scene.name + "'.");
            http.BuildWWWRequest(obj.ToString(), stamp);
            StartCoroutine(http.WaitForRequest(http.www, callback => {
                if (callback != null)
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
            //run_id = generateID();
         //   level_count++;
           // run_id = generateID();
            startTime = DateTime.UtcNow;
            
            action_count = 0;
        }
    }
}
