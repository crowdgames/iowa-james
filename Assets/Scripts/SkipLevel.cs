using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipLevel : MonoBehaviour {

    LevelManager lm;
    SkillManager sm;
    Logger log;

	// Use this for initialization
	void Start () {
        lm = GameObject.FindObjectOfType<LevelManager>();
        sm = GameObject.FindObjectOfType<SkillManager>();
        log = GameObject.FindObjectOfType<PlayerController>().GetComponent<Logger>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Skip()
    {
        log.LogMatch("loss");
        StartCoroutine("SkipCo");
    }
    
    
    public IEnumerator SkipCo()
    {
        Debug.Log("SKIPPING LEVEL");
        /*
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        */
        // Randomizer.LoadNextLevel();
        string level = SceneManager.GetActiveScene().name;
        sm.score = 0f;
        sm.level = level;
        yield return sm.ReportAndRequest();
        try
        {
            string next_level = sm.server_data.Substring(sm.server_data.IndexOf("Level"), 8);
            SceneManager.LoadScene(next_level);
        }
        catch
        {
            if (sm.server_data != "ERROR")
                SceneManager.LoadScene("Level_End");
            else
                sm.server_error = "SkipLevel";
        }
        
    }
}
