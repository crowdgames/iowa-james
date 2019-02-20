using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SkillManager : MonoBehaviour {

    public string server_data = "";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator RegisterPlayer(int trurat=1500)
    {
        Debug.Log("REGISTER PLAYER");
        string reg_player = "http://" + DataManager.host + "/register?q={\"id\":\"" + DataManager.player_id + "\",\"type\":\"player\",\"trurat\":" + trurat + "}";
        Debug.Log(reg_player);
        yield return StartCoroutine(ContactServer(reg_player));
    }

    IEnumerator ContactServer(string rp)
    {
        Debug.Log(rp);
        UnityWebRequest www = UnityWebRequest.Get(rp);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("WWW ERROR: " + www.error);
        }
        else
        {
            server_data = www.downloadHandler.text;
            //byte[] results = www.downloadHandler.data;
        }
    }
    
    public IEnumerator ReportAndRequest(float score, string level)
    {
        Debug.Log("INSIDE REPORTANDREQUEST");
        string token = DateTime.UtcNow.ToString();
        string report = "http://" + DataManager.host + "/reportMatch?q={\"token\":\"" + token + "\",\"id1\":\"" + DataManager.player_id + "\",\"id2\":\"" + level + "\",\"score1\":\"" + score + "\"}";
        Debug.Log("***REPORT****: " + report);
        yield return StartCoroutine(ContactServer(report));
        Debug.Log("DATA FROM REPORT: " + server_data);
        string request = "http://" + DataManager.host + "/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
        Debug.Log("***REQ***: " + request);
        yield return StartCoroutine(ContactServer(request));
        Debug.Log("DATA FROM REQUEST: " + server_data);
    }

    public void RegisterAndGetFirstMatch()
    {
        StartCoroutine("StartGame");
    }

    public IEnumerator StartGame()
    {
        yield return StartCoroutine(RegisterPlayer());
        StartCoroutine(RequestMatch());
    }

    public IEnumerator RequestMatch()
    {
        Debug.Log("REQUESTING A MATCH");
        string request = "http://" + DataManager.host + "/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
        Debug.Log(request);
        yield return StartCoroutine(ContactServer(request));
        Debug.Log("DATA FROM REQUEST: " + server_data);
        string first_level = server_data.Substring(server_data.IndexOf("Level"), 8);
        SceneManager.LoadScene(first_level);
    }
}
