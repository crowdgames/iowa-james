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
            Debug.Log("Inside dynode awake");
            // For detecting scene changes
            SceneManager.activeSceneChanged += onSceneChanged;
            // Call function on game start
            onSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());

            GameObject smo = GameObject.Find("SkillManager");
            if (smo)
            {
                sm = smo.GetComponent<SkillManager>();
            }
            else
            {
                GameObject smobj = new GameObject("SkillManager");
                sm = smobj.AddComponent<SkillManager>();
            }

            player_id = "MT-" + generateID();
            DataManager.player_id = player_id;
            DataManager.mode = player_id.ToCharArray()[player_id.Length - 1] % 4;
            //DataManager.mode = 0;
            Debug.Log(player_id.ToCharArray()[player_id.Length - 1]);
            Debug.Log("COIN MODE: " + DataManager.mode.ToString());
            
            run_id = 1;
            Debug.Log("Player id: " + player_id);
            startTime = DateTime.UtcNow;
            Debug.Log("Start: " + startTime);

            //REGISTER PLAYER HERE
            //StartCoroutine(RegisterPlayer());
            sm.RegisterPlayer(player_id);
                

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

        IEnumerator RegisterPlayer()
        {
            string reg_player = "http://localhost:3004/register?q={\"id\":\"" + player_id + "\",\"type\":\"player\",\"trurat\":" + 1500 + "}";
            Debug.Log(reg_player);
            UnityWebRequest www = UnityWebRequest.Get(reg_player);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
        }
    }
}
