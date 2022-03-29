using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SkillManager : MonoBehaviour {

    public string server_data = "";
    public string server_request = "";
    public string server_error = "";
    public string level = "";
    public string scenario = "";
    public float score = 0f;
    public int finished = 0;
    public float score_game = 0f;
    public float score_task = 0f;
    public int rel = 0;
    public int irrel = 0;
    
    public string http;

    Dictionary<string, string> mapping;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        mapping = new Dictionary<string, string>();
        mapping.Add("sports", "Sports Store");
        mapping.Add("grocery", "Grocery Store");
        mapping.Add("pastry", "Pastry Shop");
        mapping.Add("hardware", "Hardware Store");
        mapping.Add("clothing", "Clothing Store");
        mapping.Add("Sports Store", "sports");
        mapping.Add("Grocery Store", "grocery");
        mapping.Add("Pastry Shop", "pastry");
        mapping.Add("Hardware Store", "hardware");
        mapping.Add("Clothing Store", "clothing");
        
        http = "http";
    }

    public IEnumerator RegisterPlayer(int trurat=1500)
    {
        Debug.Log("REGISTER PLAYER");
        //string reg_player = "http://" + DataManager.host + "/register?q={\"id\":\"" + DataManager.player_id + "\",\"type\":\"player\",\"trurat\":" + trurat + "}";
        //server_request = "http://" + DataManager.host + "/register?q={\"id\":\"" + DataManager.player_id + "\",\"type\":\"player\",\"trurat\":" + trurat + "}";
        server_request = http + "://" + DataManager.host + "/register?q={\"id\":\"" + DataManager.player_id + "\",\"type\":\"player\"}";
        //server_request = "http://" + DataManager.host + "/register?q={\"id\":\"" + DataManager.player_id + "\",\"type\":\"player\"}"; 
        Debug.Log(server_request);
        yield return StartCoroutine(ContactServer());
    }

    public IEnumerator ContactServer()
    {
        Debug.Log(server_request);
        UnityWebRequest www = UnityWebRequest.Get(server_request);
        www.certificateHandler = new AcceptAllCertificates();
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            server_data = "ERROR";
            //LevelManager lm = GameObject.Find("Character").GetComponent<Logger>();
            LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            StartCoroutine(lm.ShowError());
        }
        else if (www.isHttpError)
        {
            Debug.Log("WWW HTTP ERROR: " + www.error);
            server_data = "ERROR";
            //LevelManager lm = GameObject.Find("Character").GetComponent<Logger>();
            LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            StartCoroutine(lm.ShowError());
        }
        else
        {
            LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            lm.HideError();
            server_data = www.downloadHandler.text;
        }
    }
    
    public IEnumerator ReportAndRequest()
    {
        //Debug.Log("INSIDE REPORTANDREQUEST");
        string token = DateTime.UtcNow.ToString();
        Debug.Log("Token: " +  token);
        if (DataManager.matchmaking == 0)
        {
            // string report = "http://" + DataManager.host + "/reportMatch?q={\"token\":\"" + token + "\",\"id1\":\"" + DataManager.player_id + "\",\"id2\":\"" + level + "\",\"score1\":\"" + score + "\",\"finished\":\"" + finished + "\"}";
            server_request = http + "://" + DataManager.host + "/reportMatch?q={\"token\":\"" + token + "\",\"id1\":\"" + DataManager.player_id + "\",\"id2\":\"" + level + "\",\"score1\":\"" + score + "\",\"finished\":\"" + finished + "\"}";
            Debug.Log("***REPORT****: " + server_request);
        }
        else
        {
            string task = mapping[scenario];
            //Debug.Log("Task: " + task + "\tLevel: " + level);
            task = task + "_" + level.Substring(level.LastIndexOf("_") + 1, 1);
            level = level.Substring(0, level.LastIndexOf("_"));
            Debug.Log(task + "\t" + level);
            //string report = "http://" + DataManager.host + "/reportMatch?q={\"token\":\"" + token + "\",\"id1\":\"" + DataManager.player_id + "\",\"id2\":\"" + level + "\",\"id3\":\"" + task + "\",\"score_game\":\"" + score_game + "\",\"score_task\":\"" + score_task + "\",\"relevant\":\"" + rel + "\",\"irrelevant\":\"" + irrel + "\"}";
            server_request = http + "://" + DataManager.host + "/reportMatch?q={\"token\":\"" + token + "\",\"id1\":\"" + DataManager.player_id + "\",\"id2\":\"" + level + "\",\"id3\":\"" + task + "\",\"score_game\":\"" + score_game + "\",\"score_task\":\"" + score_task + "\",\"relevant\":\"" + rel + "\",\"irrelevant\":\"" + irrel + "\"}";
            Debug.Log("***REPORT****: " + server_request);
        }
        //yield return StartCoroutine(ContactServer(report));
        yield return StartCoroutine(ContactServer());
        Debug.Log("DATA FROM REPORT: " + server_data);
        if (server_data != "ERROR")
        {
            //string request = "http://" + DataManager.host + "/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
            server_request = http + "://" + DataManager.host + "/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
            Debug.Log("***REQ***: " + server_request);
            yield return StartCoroutine(ContactServer());
            Debug.Log("DATA FROM REQUEST: " + server_data);
        }
        else
            server_error = "ReportAndRequest";
    }

    public void RegisterAndGetFirstMatch()
    {
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        yield return StartCoroutine(RegisterPlayer());
        if (server_data != "ERROR")
            StartCoroutine(RequestMatch());
        else
            server_error = "StartGame";
    }

    public IEnumerator RequestMatch()
    {
        Debug.Log("REQUESTING A MATCH");
        //string request = "http://" + DataManager.host + "/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
        server_request = http + "://" + DataManager.host + "/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
        Debug.Log(server_request);
        yield return StartCoroutine(ContactServer());
        Debug.Log("DATA FROM REQUEST: " + server_data);
        string first_level = "";
        if(DataManager.matchmaking == 0)
            first_level = server_data.Substring(server_data.IndexOf("Level"), 10);
        else
        {
            first_level = ParseRequestResponse();
        }

        SceneManager.LoadScene(first_level);
    }

    public string ParseRequestResponse()
    {
        string lev = "";
        string after_data1 = server_data.Substring(server_data.IndexOf("data1") + 9);
        int index = after_data1.IndexOf("\"");
        lev = server_data.Substring(server_data.IndexOf("data1") + 9, index);
        string scen = "";
        if (DataManager.decoupled == 1)
        {
            string after_data2 = server_data.Substring(server_data.IndexOf("data2") + 9);
            index = after_data2.IndexOf("\"");
            scen = server_data.Substring(server_data.IndexOf("data2") + 9, index);

            int num_items = int.Parse(scen.Substring(scen.IndexOf("_") + 1));
            scen = scen.Substring(0, scen.IndexOf("_"));
            scenario = mapping[scen];
            lev = lev + "_" + num_items;
        }
        else
        {
            scen = DataManager.coupled_mapping[lev];
            Debug.Log("Lev: " + lev + "\tScen: " + scen);
            int num_items = int.Parse(scen.Substring(scen.IndexOf("_") + 1));
            scen = scen.Substring(0, scen.IndexOf("_"));
         
            scenario = mapping[scen];
            lev = lev + "_" + num_items;
            Debug.Log("Level: " + lev + "\tScen: " + scenario);
        }
        return lev;
    }

    public class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            //return base.ValidateCertificate(certificateData);
            return true;
        }
    }
}
