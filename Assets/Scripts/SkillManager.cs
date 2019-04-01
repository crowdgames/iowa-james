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
    public float score = 0f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator RegisterPlayer(int trurat=1500)
    {
        Debug.Log("REGISTER PLAYER");
        string reg_player = "http://" + DataManager.host + "/register?q={\"id\":\"" + DataManager.player_id + "\",\"type\":\"player\",\"trurat\":" + trurat + "}";
        server_request = "http://" + DataManager.host + "/register?q={\"id\":\"" + DataManager.player_id + "\",\"type\":\"player\",\"trurat\":" + trurat + "}";
        Debug.Log(reg_player);
        yield return StartCoroutine(ContactServer());
    }

    public IEnumerator ContactServer()
    {
        //Debug.Log(rp);
        //UnityWebRequest www = UnityWebRequest.Get(rp);
        UnityWebRequest www = UnityWebRequest.Get(server_request);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("WWW ERROR: " + www.error);
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
            //byte[] results = www.downloadHandler.data;
        }
    }
    
    public IEnumerator ReportAndRequest()
    {
     //   Debug.Log("INSIDE REPORTANDREQUEST");
        string token = DateTime.UtcNow.ToString();
        string report = "http://" + DataManager.host + "/reportMatch?q={\"token\":\"" + token + "\",\"id1\":\"" + DataManager.player_id + "\",\"id2\":\"" + level + "\",\"score1\":\"" + score + "\"}";
        server_request = "http://" + DataManager.host + "/reportMatch?q={\"token\":\"" + token + "\",\"id1\":\"" + DataManager.player_id + "\",\"id2\":\"" + level + "\",\"score1\":\"" + score + "\"}";
        Debug.Log("***REPORT****: " + report);
        //yield return StartCoroutine(ContactServer(report));
        yield return StartCoroutine(ContactServer());
        Debug.Log("DATA FROM REPORT: " + server_data);
        if (server_data != "ERROR")
        {
            string request = "http://" + DataManager.host + "/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
            server_request = "http://" + DataManager.host + "/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
            Debug.Log("***REQ***: " + request);
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
        //Debug.Log("Inside StartGame");
        yield return StartCoroutine(RegisterPlayer());
        if (server_data != "ERROR")
            StartCoroutine(RequestMatch());
        else
            server_error = "StartGame";
        //Debug.Log("Exiting StartGame");
    }

    public IEnumerator RequestMatch()
    {
        Debug.Log("REQUESTING A MATCH");
        string request = "http://" + DataManager.host + "/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
        server_request = "http://" + DataManager.host + "/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
        Debug.Log(request);
        yield return StartCoroutine(ContactServer());
        Debug.Log("DATA FROM REQUEST: " + server_data);
        string first_level = server_data.Substring(server_data.IndexOf("Level"), 10);
        SceneManager.LoadScene(first_level);
    }
}
