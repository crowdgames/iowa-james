using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkillManager : MonoBehaviour {

    public string server_data = "";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RegisterPlayer(string pid, int trurat=1500)
    {
        string reg_player = "http://localhost:3004/register?q={\"id\":\"" + pid + "\",\"type\":\"player\",\"trurat\":" + trurat + "}";
        Debug.Log(reg_player);
        StartCoroutine(ContactServer(reg_player));
    }

    IEnumerator ContactServer(string rp)
    {
        Debug.Log(rp);
        UnityWebRequest www = UnityWebRequest.Get(rp);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            server_data = www.downloadHandler.text;
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
    
    public IEnumerator ReportAndRequest(int result, string level)
    {
        string token = DateTime.UtcNow.ToString();
        string report = "http://localhost:3004/reportMatch?q={\"token\":\"" + token + "\",\"id1\":\"" + DataManager.player_id + "\",\"id2\":\"" + level + "\",\"score1\":\"" + result + "\"}";
        Debug.Log("***REPORT****: " + report);
        yield return StartCoroutine(ContactServer(report));
        Debug.Log("DATA FROM REPORT: " + server_data);
        string request = "http://localhost:3004/requestMatch?q={\"id\":\"" + DataManager.player_id + "\"}";
        Debug.Log("***REQ***: " + request);
        yield return StartCoroutine(ContactServer(request));
        Debug.Log("DATA FROM REQUEST: " + server_data);
    }
}
