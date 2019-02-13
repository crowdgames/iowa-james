using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipLevel : MonoBehaviour {

    LevelManager lm;
    SkillManager sm;

	// Use this for initialization
	void Start () {
        lm = GameObject.FindObjectOfType<LevelManager>();
        sm = GameObject.FindObjectOfType<SkillManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Skip()
    {
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
        yield return sm.ReportAndRequest(0, level);
        Debug.Log("NEXT LEVEL: " + sm.server_data);
        try
        {
            string next_level = sm.server_data.Substring(sm.server_data.IndexOf("Level"), 8);
            SceneManager.LoadScene(next_level);
        }
        catch
        {
            SceneManager.LoadScene("Level_End");
        }
        
    }
}
